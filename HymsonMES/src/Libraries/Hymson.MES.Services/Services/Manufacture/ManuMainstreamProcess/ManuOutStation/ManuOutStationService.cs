﻿using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Services.Bos.Manufacture;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.OutStation;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuOutStation
{
    /// <summary>
    /// 出站
    /// </summary>
    public class ManuOutStationService : IManuOutStationService
    {
        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 仓储接口（条码信息）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储接口（上料信息）
        /// </summary>
        private readonly IManuFeedingRepository _manuFeedingRepository;

        /// <summary>
        /// 仓储接口（条码流转）
        /// </summary>
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;

        /// <summary>
        /// 仓储接口（BOM明细）
        /// </summary>
        private readonly IProcBomDetailRepository _procBomDetailRepository;

        /// <summary>
        /// 仓储接口（BOM替代料明细）
        /// </summary>
        private readonly IProcBomDetailReplaceMaterialRepository _procBomDetailReplaceMaterialRepository;

        /// <summary>
        /// 仓储接口（物料维护）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 仓储接口（物料替代料）
        /// </summary>
        private readonly IProcReplaceMaterialRepository _procReplaceMaterialRepository;

        /// <summary>
        /// 仓储接口（物料库存）
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        /// 仓储接口（物料台账）
        /// </summary>
        private readonly IWhMaterialStandingbookRepository _whMaterialStandingbookRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuCommonService"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuFeedingRepository"></param>
        /// <param name="manuSfcCirculationRepository"></param>
        /// <param name="procBomDetailRepository"></param>
        /// <param name="procBomDetailReplaceMaterialRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procReplaceMaterialRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="whMaterialStandingbookRepository"></param>
        public ManuOutStationService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuCommonService manuCommonService,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuFeedingRepository manuFeedingRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IProcBomDetailRepository procBomDetailRepository,
            IProcBomDetailReplaceMaterialRepository procBomDetailReplaceMaterialRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcReplaceMaterialRepository procReplaceMaterialRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuCommonService = manuCommonService;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuFeedingRepository = manuFeedingRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _procBomDetailReplaceMaterialRepository = procBomDetailReplaceMaterialRepository;
            _procMaterialRepository = procMaterialRepository;
            _procReplaceMaterialRepository = procReplaceMaterialRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _whMaterialStandingbookRepository = whMaterialStandingbookRepository;
        }


        /// <summary>
        /// 出站
        /// </summary>
        /// <param name="sfcProduceEntity"></param>
        /// <returns></returns>
        public async Task<int> OutStationAsync(ManuSfcProduceEntity sfcProduceEntity)
        {
            var rows = 0;

            // 获取生产工单
            _ = await _manuCommonService.GetProduceWorkOrderByIdAsync(sfcProduceEntity.WorkOrderId);

            // 更新时间
            sfcProduceEntity.UpdatedBy = _currentUser.UserName;
            sfcProduceEntity.UpdatedOn = HymsonClock.Now();

            // 初始化步骤
            var sfcStep = new ManuSfcStepEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentSite.SiteId ?? 0,
                SFC = sfcProduceEntity.SFC,
                ProductId = sfcProduceEntity.ProductId,
                WorkOrderId = sfcProduceEntity.WorkOrderId,
                WorkCenterId = sfcProduceEntity.WorkCenterId,
                ProductBOMId = sfcProduceEntity.ProductBOMId,
                Qty = sfcProduceEntity.Qty,
                EquipmentId = sfcProduceEntity.EquipmentId,
                ResourceId = sfcProduceEntity.ResourceId,
                CreatedBy = sfcProduceEntity.UpdatedBy,
                CreatedOn = sfcProduceEntity.UpdatedOn.Value,
                UpdatedBy = sfcProduceEntity.UpdatedBy,
                UpdatedOn = sfcProduceEntity.UpdatedOn.Value,
            };

            // 合格品出站
            // 获取下一个工序（如果没有了，就表示完工）
            var nextProcedure = await _manuCommonService.GetNextProcedureAsync(sfcProduceEntity);

            // 扣料
            //await func(sfcProduceEntity.ProductBOMId, sfcProduceEntity.ProcedureId);
            var initialMaterials = await GetInitialMaterialsAsync(sfcProduceEntity);

            // 过滤扣料集合
            foreach (var material in initialMaterials)
            {
                var materialEntity = await _procMaterialRepository.GetByIdAsync(material.MaterialId);
                if (materialEntity == null) continue;

                // 如有设置消耗系数
                if (materialEntity.ConsumeRatio.HasValue == true) material.ConsumeRatio = materialEntity.ConsumeRatio.Value;

                // 收集方式是批次
                if (material.DataCollectionWay == MaterialSerialNumberEnum.Batch)
                {
                    // 进行扣料
                    await UpdateDeductQty(sfcProduceEntity, material);
                    continue;
                }

                // 2.确认主物料的收集方式，不是"批次"就结束（不对该物料进行扣料）
                if (materialEntity.SerialNumber != MaterialSerialNumberEnum.Batch) continue;

                // 进行扣料
                await UpdateDeductQty(sfcProduceEntity, material);
            }

            // 完工
            if (nextProcedure == null)
            {
                // 删除 manu_sfc_produce
                rows += await _manuSfcProduceRepository.DeletePhysicalAsync(sfcProduceEntity.SFC);

                // 插入 manu_sfc_step 状态为 完成
                sfcStep.Operatetype = ManuSfcStepTypeEnum.OutStock;    // TODO 这里的状态？？
                sfcStep.CurrentStatus = SfcProduceStatusEnum.Complete;  // TODO 这里的状态？？
                rows += await _manuSfcStepRepository.InsertAsync(sfcStep);

                // manu_sfc_info 修改为完成 且入库
                // 条码信息
                var sfcInfo = await _manuSfcRepository.GetBySFCAsync(sfcProduceEntity.SFC);

                // 删除 manu_sfc_produce_business
                await _manuSfcProduceRepository.DeleteSfcProduceBusinessBySfcInfoIdAsync(new DeleteSfcProduceBusinesssBySfcInfoIdCommand
                {
                    SiteId = sfcProduceEntity.SiteId,
                    SfcInfoId = sfcInfo.Id
                });

                // 更新状态
                sfcInfo.Status = SfcStatusEnum.Complete;
                sfcInfo.UpdatedBy = sfcProduceEntity.UpdatedBy;
                sfcInfo.UpdatedOn = sfcProduceEntity.UpdatedOn;
                rows += await _manuSfcRepository.UpdateAsync(sfcInfo);

                // 入库
                rows += await SaveToWarehouse(sfcProduceEntity);
            }
            // 未完工
            else
            {
                // 修改 manu_sfc_produce 为排队, 工序修改为下一工序的id
                sfcProduceEntity.Status = SfcProduceStatusEnum.lineUp;
                sfcProduceEntity.ProcedureId = nextProcedure.Id;
                rows += await _manuSfcProduceRepository.UpdateAsync(sfcProduceEntity);

                // 插入 manu_sfc_step 状态为 进站
                sfcStep.Operatetype = ManuSfcStepTypeEnum.OutStock;
                rows += await _manuSfcStepRepository.InsertAsync(sfcStep);
            }

            return rows;
        }

        /// <summary>
        /// 出站（批量）
        /// </summary>
        /// <param name="bos"></param>
        /// <returns></returns>
        public async Task<int> OutStationAsync(IEnumerable<ManufactureBo> bos)
        {
            var rows = 0;

            // TODO
            await Task.CompletedTask;
            return rows;
        }

        /// <summary>
        /// 出站(在制维修)
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public async Task<int> OutStationRepiarAsync(ManufactureRepairBo bo)
        {
            // 获取生产条码信息
            var (sfcProduceEntity, _) = await _manuCommonService.GetProduceSFCAsync(bo.SFC);

            // 合法性校验
            sfcProduceEntity.VerifySFCStatus(SfcProduceStatusEnum.Activity)
                            .VerifyProcedure(bo.ProcedureId);

            // 出站
            return await OutStationAsync(sfcProduceEntity);
        }

        /// <summary>
        /// 获取即将扣料的物料数据
        /// </summary>
        /// <param name="sfcProduceEntity"></param>
        /// <returns></returns>
        private async Task<List<MaterialDeductBo>> GetInitialMaterialsAsync(ManuSfcProduceEntity sfcProduceEntity)
        {
            // 获取BOM绑定的物料
            var mainMaterials = await _procBomDetailRepository.GetByBomIdAsync(sfcProduceEntity.ProductBOMId);

            // 未设置物料
            if (mainMaterials == null || mainMaterials.Any() == false) throw new CustomerValidationException(nameof(ErrorCode.MES10612));

            // 取得特定工序的物料
            mainMaterials = mainMaterials.Where(w => w.ProcedureId == sfcProduceEntity.ProcedureId);

            // 检查是否有BOM替代料
            var replaceMaterialsForBOM = await _procBomDetailReplaceMaterialRepository.GetByBomIdAsync(sfcProduceEntity.ProductBOMId);
            var replaceMaterialsDic = replaceMaterialsForBOM.ToLookup(w => w.BomDetailId).ToDictionary(d => d.Key, d => d);

            // 获取初始扣料数据
            var initialMaterials = new List<MaterialDeductBo> { };
            foreach (var item in mainMaterials)
            {
                var deduct = new MaterialDeductBo
                {
                    MaterialId = item.MaterialId,
                    Usages = item.Usages,
                    Loss = item.Loss,
                    DataCollectionWay = item.DataCollectionWay
                };

                // 填充BOM替代料
                if (item.IsEnableReplace == false)
                {
                    if (replaceMaterialsDic.TryGetValue(item.Id, out var replaces) == true)
                    {
                        deduct.ReplaceMaterials = replaces.Select(s => new MaterialDeductItemBo
                        {
                            MaterialId = s.ReplaceMaterialId,
                            Usages = s.Usages,
                            Loss = s.Loss,
                        });
                    }
                }
                // 填充物料替代料
                else
                {
                    var replaceMaterialsForMain = await _procReplaceMaterialRepository.GetProcReplaceMaterialViewsAsync(new ProcReplaceMaterialQuery
                    {
                        SiteId = item.SiteId,
                        MaterialId = item.MaterialId,
                    });

                    // 启用的替代物料
                    deduct.ReplaceMaterials = replaceMaterialsForMain.Where(w => w.IsEnabled == true).Select(s => new MaterialDeductItemBo
                    {
                        MaterialId = s.MaterialId,
                        Usages = item.Usages,
                        Loss = item.Loss
                    });
                }

                // 添加到初始扣料集合
                initialMaterials.Add(deduct);
            }

            return initialMaterials;
        }

        /// <summary>
        /// 进行扣料（单一物料）
        /// </summary>
        /// <param name="sfcProduceEntity"></param>
        /// <param name="material"></param>
        /// <returns></returns>
        private async Task<int> UpdateDeductQty(ManuSfcProduceEntity sfcProduceEntity, MaterialDeductBo material)
        {
            // 取得当前物料的库存
            var feedingEntities = await _manuFeedingRepository.GetByResourceIdAndMaterialIdAsync(new GetByResourceIdAndMaterialIdQuery
            {
                ResourceId = sfcProduceEntity.ResourceId,
                MaterialId = material.MaterialId
            });

            // 待执行
            List<UpdateQtyByProductIdCommand> updateQtyByProductIdCommands = new();
            List<ManuSfcCirculationEntity> manuSfcCirculationEntities = new();

            // 需扣减数量 = 用量 * 损耗 * 消耗系数
            decimal qty = material.Usages * material.Loss * material.ConsumeRatio;

            // 剩余需扣减的数量
            var residue = qty;
            foreach (var item in feedingEntities)
            {
                // 数量足够
                if (residue < item.Qty)
                {
                    residue -= item.Qty;
                    item.Qty -= qty;

                    // 添加到扣减物料库存
                    updateQtyByProductIdCommands.Add(new UpdateQtyByProductIdCommand
                    {
                        UpdatedBy = sfcProduceEntity.UpdatedBy ?? _currentUser.UserName,
                        UpdatedOn = sfcProduceEntity.UpdatedOn,
                        ResourceId = sfcProduceEntity.ResourceId,
                        ProductId = material.MaterialId,
                        Qty = item.Qty
                    });

                    // 条码流转
                    manuSfcCirculationEntities.Add(new ManuSfcCirculationEntity
                    {

                    });
                    break;
                }
                // 数量不够
                else
                {
                    residue -= item.Qty;

                    // 继续下一个
                    item.Qty = 0;
                }
            }

            // 物料库存不够，启用替代料
            if (residue > 0)
            {
                // TODO
            }

            // 扣减物料库存
            _ = await _manuFeedingRepository.UpdateQtyByProductIdAsync(updateQtyByProductIdCommands);

            return 0;
        }

        /// <summary>
        /// 入库
        /// </summary>
        /// <param name="manuSfcProduceEntity"></param>
        /// <returns></returns>
        private async Task<int> SaveToWarehouse(ManuSfcProduceEntity manuSfcProduceEntity)
        {
            var rows = 0;

            var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(manuSfcProduceEntity.ProductId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES15101));

            // 新增 wh_material_inventory
            rows += await _whMaterialInventoryRepository.InsertAsync(new WhMaterialInventoryEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SupplierId = 0,//自制品 没有
                MaterialId = manuSfcProduceEntity.ProductId,
                MaterialBarCode = manuSfcProduceEntity.SFC,
                Batch = "",//自制品 没有
                QuantityResidue = procMaterialEntity.Batch,
                Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                Source = WhMaterialInventorySourceEnum.ManuComplete,
                SiteId = _currentSite.SiteId ?? 0,
                CreatedBy = _currentUser.UserName,
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = manuSfcProduceEntity.UpdatedBy,
                UpdatedOn = manuSfcProduceEntity.UpdatedOn
            });

            // 新增 wh_material_standingbook
            rows += await _whMaterialStandingbookRepository.InsertAsync(new WhMaterialStandingbookEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                MaterialCode = procMaterialEntity.MaterialCode,
                MaterialName = procMaterialEntity.MaterialName,
                MaterialVersion = procMaterialEntity.Version ?? "",
                MaterialBarCode = manuSfcProduceEntity.SFC,
                Batch = "",//自制品 没有
                Quantity = procMaterialEntity.Batch,
                Unit = procMaterialEntity.Unit ?? "",
                Type = WhMaterialInventoryTypeEnum.ManuComplete,
                Source = WhMaterialInventorySourceEnum.ManuComplete,
                SiteId = _currentSite.SiteId ?? 0,
                CreatedBy = _currentUser.UserName,
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = manuSfcProduceEntity.UpdatedBy,
                UpdatedOn = manuSfcProduceEntity.UpdatedOn
            });

            return rows;
        }

    }
}
