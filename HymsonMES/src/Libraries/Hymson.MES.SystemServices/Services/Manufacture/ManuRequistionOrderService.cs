﻿using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.Logging;
using Hymson.Logging.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.Warehouse;

using Hymson.MES.CoreServices.Bos.Manufacture;

using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Data.Repositories.WhWareHouse;
using Hymson.MES.Data.Repositories.WhWareHouse.Query;
using Hymson.MES.SystemServices.Dtos;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.Web.Framework.WorkContext;
using Newtonsoft.Json;
using System.Security.Policy;

namespace Hymson.MES.SystemServices.Services
{
    /// <summary>
    /// 业务处理层(领料单)
    /// </summary>
    public class ManuRequistionOrderService : IManuRequistionOrderService
    {
        /// <summary>
        /// 当前系统
        /// </summary>
        private readonly ICurrentSystem _currentSystem;

        /// <summary>
        /// 日志
        /// </summary>
        private readonly ITraceLogService _traceLogService;

        /// <summary>
        /// 物料仓储接口
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 仓库仓储接口
        /// </summary>
        private readonly IWhWarehouseRepository _whWarehouseRepository;

        /// <summary>
        /// 供应商仓储接口
        /// </summary>
        private readonly IWhSupplierRepository _supplierRepository;

        /// <summary>
        /// 工单信息表 仓储
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 生产领料单仓储接口
        /// </summary>
        private readonly IManuRequistionOrderRepository _requistionOrderRepository;

        /// <summary>
        /// 生产领料单明细仓储
        /// </summary>
        private readonly IManuRequistionOrderDetailRepository _requistionOrderDetailRepository;

        /// <summary>
        /// 物料库存 仓储
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        /// 仓储接口（物料台账）
        /// </summary>
        private readonly IWhMaterialStandingbookRepository _whMaterialStandingbookRepository;
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ManuRequistionOrderService(ICurrentSystem currentSystem,
            ITraceLogService traceLogService,
            IProcMaterialRepository procMaterialRepository,
            IWhWarehouseRepository whWarehouseRepository,
            IWhSupplierRepository supplierRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuRequistionOrderRepository requistionOrderRepository,
            IManuRequistionOrderDetailRepository requistionOrderDetailRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository,
            IManuSfcRepository manuSfcRepository, IManuSfcInfoRepository manuSfcInfoRepository,
            ILocalizationService localizationService)
        {
            _currentSystem = currentSystem;
            _traceLogService = traceLogService;
            _procMaterialRepository = procMaterialRepository;
            _whWarehouseRepository = whWarehouseRepository;
            _supplierRepository = supplierRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _requistionOrderRepository = requistionOrderRepository;
            _requistionOrderDetailRepository = requistionOrderDetailRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _whMaterialStandingbookRepository = whMaterialStandingbookRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _localizationService = localizationService;
        }

        /// <summary>
        /// 生产领料
        /// </summary>
        /// <param name="productionPickDto"></param>
        /// <returns></returns>
        public async Task SavePickMaterialsAsync(ProductionPickDto productionPickDto)
        {
            var siteId = _currentSystem.SiteId;

            #region  验证

            if (productionPickDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }

            //领料明细信息不能为空
            if (productionPickDto.ReceiveMaterials == null || !productionPickDto.ReceiveMaterials.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES14109));
            }

            var erpRequisitionOrder = productionPickDto.ERPRequisitionOrder.Trim();
            if (string.IsNullOrWhiteSpace(erpRequisitionOrder))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES14126));
            }

            //记录接收日志
            _traceLogService.WriteLogEntry(new TraceLogEntry
            {
                Id = productionPickDto.ERPRequisitionOrder.Replace("-", ""),
                Type = "PickMaterials",//TODO,增加枚举
                Timestamp = DateTime.Now,
                Message = "生产领料单信息接收成功",
                Data = new Dictionary<string, string>() {
                            {
                                "MessageBody",JsonConvert.SerializeObject(productionPickDto)
                            }
                     }
            });

            //验证领料单是否重复
            var manuRequistionOrderEntity = await _requistionOrderRepository.GetByCodeAsync(new ManuRequistionOrderQuery
            {
                SiteId = siteId,
                ReqOrderCode = erpRequisitionOrder
            });
            if (manuRequistionOrderEntity != null)
            {
                return;
                //throw new CustomerValidationException(nameof(ErrorCode.MES14103)).WithData("code", productionPickDto.ERPRequisitionOrder);
            }

            //验证工单是否存在
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByCodeAsync(new PlanWorkOrderQuery
            {
                SiteId = siteId,
                OrderCode = productionPickDto.OrderCode
            });
            if (planWorkOrderEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES14104)).WithData("code", productionPickDto.OrderCode);
            }

            //验证仓库是否存在,会同时从多个仓库领料
            //var codes = productionPickDto.ReceiveMaterials.Select(x => x.WareHouseCode).Distinct().ToArray();
            //var whWarehouseEntities = await _whWarehouseRepository.GetWhWarehouseEntitiesAsync(new WhWarehouseQuery
            //{
            //    SiteId = siteId,
            //    WareHouseCodes = codes
            //});
            //if (whWarehouseEntities == null || !whWarehouseEntities.Any() || whWarehouseEntities.Count() < codes.Length)
            //{
            //    // throw new CustomerValidationException(nameof(ErrorCode.MES14105)).WithData("code", productionPickDto.WareHouseCode);
            //}

            //获取物料明细
            var procMaterialsTask = GetMaterialEntitiesAsync(productionPickDto, siteId);

            //仓库
            var wareHouseCodes = productionPickDto.ReceiveMaterials.Select(x => x.WareHouseCode).Distinct().ToArray();
            var warehouseEntitiesTask = _whWarehouseRepository.GetWhWarehouseEntitiesAsync(new WhWarehouseQuery
            {
                SiteId = _currentSystem.SiteId,
                WareHouseCodes = wareHouseCodes,
            });

            //库存
            var barCodes = productionPickDto.ReceiveMaterials.Select(x => x.MaterialBarCode).ToArray();
            var inventoryEntitiesTask = _whMaterialInventoryRepository.GetByBarCodesAsync(new Data.Repositories.Warehouse.WhMaterialInventory.Query.WhMaterialInventoryBarCodesQuery
            {
                SiteId = siteId,
                BarCodes = barCodes
            });

            //供应商
            var suppierCodes = productionPickDto.ReceiveMaterials.Where(x => !string.IsNullOrWhiteSpace(x.SupplierCode)).Select(x => x.SupplierCode ?? "");
            IEnumerable<WhSupplierEntity> whSupplierEntities = new List<WhSupplierEntity>();
            if (suppierCodes.Any())
            {
                whSupplierEntities = await _supplierRepository.GetByCodesAsync(new WhSuppliersByCodeQuery
                {
                    SiteId = siteId,
                    Codes = suppierCodes
                });
            }

            var procMaterials = await procMaterialsTask;
            var warehouseEntities = await warehouseEntitiesTask;
            var inventoryEntities = await inventoryEntitiesTask;

            #endregion

            #region 组装数据

            //领料明细
            var manuRequistionOrderDetails = new List<ManuRequistionOrderDetailEntity>();
            var updateQuantityCommands = new List<UpdateQuantityCommand>();
            var addInventoryEntities = new List<WhMaterialInventoryEntity>();
            //var updateQuantitieCommands = new List<UpdateQuantityCommand>();
            var standingbookEntities = new List<WhMaterialStandingbookEntity>();
            var manuSfcEntities = new List<ManuSfcEntity>();
            var manuSfcInfoEntities = new List<ManuSfcInfoEntity>();

            var validationFailures = new List<FluentValidation.Results.ValidationFailure>();
            var requistionOrderId = IdGenProvider.Instance.CreateId();
            foreach (var production in productionPickDto.ReceiveMaterials)
            {
                var barCode = production.MaterialBarCode.Trim();
                var whWarehouse = warehouseEntities.FirstOrDefault(x => x.Code == production.WareHouseCode);
                var procMaterial = procMaterials.FirstOrDefault(x => x.MaterialCode == production.MaterialCode && x.Version == production.MaterialVersion);

                var validationFailure = new FluentValidation.Results.ValidationFailure();
                //判断物料是否都存在
                if (procMaterial == null)
                {
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", production.MaterialCode+","+production.MaterialVersion}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", production.MaterialCode + "," + production.MaterialVersion);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES14130);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                var inventoryEntity = inventoryEntities.FirstOrDefault(x => x.MaterialBarCode == barCode);
                if (inventoryEntity != null)
                {
                    //updateQuantitieCommands.Add(new UpdateQuantityCommand
                    //{
                    //    BarCode = barCode,
                    //    QuantityResidue = production.ReceivedQty,
                    //    UpdatedBy = _currentSystem.Name,
                    //    UpdatedOn = HymsonClock.Now()
                    //});
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", production.MaterialBarCode}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", production.MaterialBarCode);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES14143);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                manuRequistionOrderDetails.Add(new ManuRequistionOrderDetailEntity()
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    RequistionOrderId = requistionOrderId,
                    MaterialCode = production.MaterialCode,
                    Version = production.MaterialVersion,
                    MaterialBarCode = production.MaterialBarCode,
                    WarehouseId = whWarehouse?.Id ?? 0,
                    SupplierCode = production.SupplierCode ?? "",
                    Batch = production.MaterialBatch,
                    Qty = production.ReceivedQty,
                    ExpirationDate = production.ExpirationDate,
                    CreatedBy = _currentSystem.Name,
                    UpdatedBy = _currentSystem.Name,
                    SiteId = _currentSystem.SiteId
                });

                var supplierEntity = whSupplierEntities.FirstOrDefault(x => x.Code == production.SupplierCode);
                //验证物料条码是否存在，存在数量累加，否则新增
                addInventoryEntities.Add(new WhMaterialInventoryEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SupplierId = supplierEntity?.Id ?? 0,
                    MaterialId = procMaterial?.Id ?? 0,
                    MaterialBarCode = barCode,
                    Batch = production.MaterialBatch,
                    QuantityResidue = production.ReceivedQty,
                    ScrapQty=0,
                    ReceivedQty = production.ReceivedQty,
                    DueDate = production.ExpirationDate,
                    Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                    Source = MaterialInventorySourceEnum.WMS,
                    MaterialType = MaterialInventoryMaterialTypeEnum.PurchaseParts,
                    WorkOrderId = planWorkOrderEntity.Id,
                    SiteId = siteId,
                    CreatedBy = _currentSystem.Name,
                    UpdatedBy = _currentSystem.Name
                });

                //记录物料台账
                standingbookEntities.Add(new WhMaterialStandingbookEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    MaterialCode = procMaterial?.MaterialCode ?? "",
                    MaterialName = procMaterial?.MaterialName ?? "",
                    MaterialVersion = procMaterial?.Version ?? "",
                    MaterialBarCode = production.MaterialBarCode,
                    Batch = production.MaterialBatch,
                    Quantity = production.ReceivedQty,
                    Unit = procMaterial?.Unit ?? "",
                    SupplierId = whWarehouse?.Id ?? 0,
                    Type = WhMaterialInventoryTypeEnum.MaterialReceiving,
                    Source = MaterialInventorySourceEnum.WMS,
                    SiteId = _currentSystem.SiteId,
                    CreatedBy = _currentSystem.Name,
                    UpdatedBy = _currentSystem.Name
                });

                //记录条码和条码info信息
                var manuSfcId = IdGenProvider.Instance.CreateId();
                manuSfcEntities.Add(new ManuSfcEntity
                {
                    Id = manuSfcId,
                    SFC = barCode,
                    IsUsed = YesOrNoEnum.No,
                    Type= SfcTypeEnum.NoProduce,
                    Qty = production.ReceivedQty,
                    SiteId = _currentSystem.SiteId,
                    Status = SfcStatusEnum.Complete,
                    CreatedBy = _currentSystem.Name,
                    UpdatedBy = _currentSystem.Name,
                });

                manuSfcInfoEntities.Add( new ManuSfcInfoEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSystem.SiteId,
                    SfcId = manuSfcId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    ProductId = planWorkOrderEntity.ProductId,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                    IsUsed = false,
                    CreatedBy = _currentSystem.Name,
                    UpdatedBy = _currentSystem.Name
                });
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("MaterialError"), validationFailures);
            }

            //领料，对应仓库对应物料增加数量
            var manuRequistionOrder = new ManuRequistionOrderEntity()
            {
                Id = requistionOrderId,
                ReqOrderCode = erpRequisitionOrder,
                WorkOrderId = planWorkOrderEntity.Id,
                Status = SysDataStatusEnum.Enable,
                Type = ManuRequistionTypeEnum.ERP,
                Remark = "",
                CreatedBy = _currentSystem.Name,
                UpdatedBy = _currentSystem.Name,
                SiteId = _currentSystem.SiteId
            };
            #endregion

            #region 入库

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _requistionOrderRepository.InsertAsync(manuRequistionOrder);
                if (rows == 0)
                {
                    //并发报错
                    return;
                }
                if (manuRequistionOrderDetails.Any())
                {
                    rows += await _requistionOrderDetailRepository.InsertsAsync(manuRequistionOrderDetails);
                }

                if (addInventoryEntities.Any())
                {
                    rows += await _whMaterialInventoryRepository.InsertsAsync(addInventoryEntities);
                }

                //if (updateQuantitieCommands.Any())
                //{
                //    rows += await _whMaterialInventoryRepository.UpdatesIncreaseQuantityResidueAsync(updateQuantitieCommands);
                //}

                if (standingbookEntities.Any())
                {
                    rows += await _whMaterialStandingbookRepository.InsertsAsync(standingbookEntities);
                }

                if (manuSfcEntities.Any())
                {
                    await _manuSfcRepository.InsertRangeAsync(manuSfcEntities);
                }

                if (manuSfcInfoEntities.Any())
                {
                    await _manuSfcInfoRepository.InsertsAsync(manuSfcInfoEntities);
                }
                trans.Complete();
            }
            #endregion
        }

        /// <summary>
        /// 获取物料列表
        /// </summary>
        /// <param name="productionPickDto"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private async Task<IEnumerable<ProcMaterialEntity>> GetMaterialEntitiesAsync(ProductionPickDto productionPickDto, long siteId)
        {
            //查询物料信息
            var materialCodeList = new List<string> { };
            if (productionPickDto.ReceiveMaterials.Any())
            {
                var codes = productionPickDto.ReceiveMaterials.Select(x => x.MaterialCode).ToArray();
                materialCodeList.AddRange(codes);
            }
            var materialCodes = materialCodeList.Distinct().ToArray();
            var procMaterials = await _procMaterialRepository.GetProcMaterialEntitiesAsync(new ProcMaterialQuery
            {
                MaterialCodes = materialCodes,
                SiteId = siteId
            });
            return procMaterials;
        }
    }
}
