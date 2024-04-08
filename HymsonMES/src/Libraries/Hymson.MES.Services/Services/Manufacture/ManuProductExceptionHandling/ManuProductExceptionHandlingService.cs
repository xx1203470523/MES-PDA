using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.Warehouse;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务（生产异常处理）
    /// </summary>
    public class ManuProductExceptionHandlingService : IManuProductExceptionHandlingService
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger<ManuProductExceptionHandlingService> _logger;

        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 服务接口（Excel）
        /// </summary>
        private readonly IExcelService _excelService;

        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        /// <summary>
        ///  仓储接口（条码）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        ///  仓储接口（条码信息）
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

        /// <summary>
        ///  仓储接口（条码在制）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 仓储接口（产品不良录入）
        /// </summary>
        private readonly IManuProductBadRecordRepository _manuProductBadRecordRepository;

        /// <summary>
        /// 仓储接口（产品NG记录）
        /// </summary>
        private readonly IManuProductNgRecordRepository _manuProductNgRecordRepository;

        /// <summary>
        ///  仓储接口（物料）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 仓储接口（工序）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        ///  仓储接口（工单）
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 仓储接口（不合格代码）
        /// </summary>
        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;

        /// <summary>
        ///  仓储接口（物料库存）
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        ///  仓储接口（物料台账）
        /// </summary>
        private readonly IWhMaterialStandingbookRepository _whMaterialStandingbookRepository;

        /// <summary>
        /// 构造函数（生产异常处理）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="excelService"></param>
        /// <param name="manuCommonService"></param>
        /// <param name="masterDataService"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuProductBadRecordRepository"></param>
        /// <param name="manuProductNgRecordRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="qualUnqualifiedCodeRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="whMaterialStandingbookRepository"></param>
        public ManuProductExceptionHandlingService(ILogger<ManuProductExceptionHandlingService> logger,
            ICurrentUser currentUser, ICurrentSite currentSite, IExcelService excelService,
            IManuCommonService manuCommonService,
            IMasterDataService masterDataService,
            IManuSfcRepository manuSfcRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuProductBadRecordRepository manuProductBadRecordRepository,
            IManuProductNgRecordRepository manuProductNgRecordRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcProcedureRepository procProcedureRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository)
        {
            _logger = logger;
            _currentUser = currentUser;
            _currentSite = currentSite;
            _excelService = excelService;
            _manuCommonService = manuCommonService;
            _masterDataService = masterDataService;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuProductBadRecordRepository = manuProductBadRecordRepository;
            _manuProductNgRecordRepository = manuProductNgRecordRepository;
            _procMaterialRepository = procMaterialRepository;
            _procProcedureRepository = procProcedureRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _whMaterialStandingbookRepository = whMaterialStandingbookRepository;
        }


        #region 设备误判
        /// <summary>
        /// 根据条码查询信息（设备误判）
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuMisjudgmentBarCodeDto>> GetMisjudgmentByBarCodeAsync(string barCode)
        {
            if (string.IsNullOrEmpty(barCode)) throw new CustomerValidationException(nameof(ErrorCode.MES15445));

            // 查询条码
            var sfcEntity = await _manuSfcRepository.GetSingleAsync(new ManuSfcQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                SFC = barCode,
                Type = SfcTypeEnum.Produce
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES15446)).WithData("barCode", barCode);

            // 状态校验
            if (ManuSfcStatus.ForbidSfcStatuss.Contains(sfcEntity.Status)) throw new CustomerValidationException(nameof(ErrorCode.MES15447))
                    .WithData("barCode", barCode)
                    .WithData("status", sfcEntity.Status.GetDescription());

            /*
            // 非生产条码
            if (sfcEntity.Type == SfcTypeEnum.NoProduce) throw new CustomerValidationException(nameof(ErrorCode.MES15456))
                    .WithData("barCode", barCode)
                    .WithData("type", sfcEntity.Type.GetDescription());
            */

            // 查询不合格记录
            var badRecordEntities = await _manuProductBadRecordRepository.GetManuProductBadRecordEntitiesBySFCAsync(new ManuProductBadRecordBySfcQuery
            {
                SiteId = sfcEntity.SiteId,
                Status = ProductBadRecordStatusEnum.Open,
                SFC = barCode
            });
            if (badRecordEntities == null || !badRecordEntities.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15451)).WithData("barCode", barCode);

            // 查询条码信息
            var sfcInfoEntity = await _manuSfcInfoRepository.GetBySFCIdAsync(sfcEntity.Id)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES15446)).WithData("barCode", barCode);

            // 查询产品
            var productEntity = await _procMaterialRepository.GetByIdAsync(sfcInfoEntity.ProductId);

            // 查询在制品
            var sfcProduceEntity = await _manuSfcProduceRepository.GetBySFCIdAsync(sfcEntity.Id);

            // 查询工序
            List<long> procedureIds = new();
            if (sfcProduceEntity != null) procedureIds.Add(sfcProduceEntity.ProcedureId);
            procedureIds.AddRange(badRecordEntities.Select(s => s.FoundBadOperationId));
            var procedureEntities = await _procProcedureRepository.GetByIdsAsync(procedureIds);

            // 遍历不良记录
            List<ManuMisjudgmentBarCodeDto> dtos = new();
            foreach (var item in badRecordEntities)
            {
                var dto = new ManuMisjudgmentBarCodeDto
                {
                    Id = item.Id,
                    BarCode = sfcEntity.SFC,
                    Qty = sfcEntity.Qty
                };

                // 填充工单
                if (sfcInfoEntity.WorkOrderId.HasValue)
                {
                    var workOrderEntity = await _planWorkOrderRepository.GetByIdAsync(sfcInfoEntity.WorkOrderId.Value);
                    if (workOrderEntity != null) dto.WorkOrderCode = workOrderEntity.OrderCode;
                }

                // 填充产品
                if (productEntity != null)
                {
                    dto.ProductCode = productEntity.MaterialCode;
                    dto.ProductName = productEntity.MaterialName;
                }

                // 填充发现工序
                var foundProcedureEntity = procedureEntities.FirstOrDefault(f => f.Id == item.FoundBadOperationId);
                if (foundProcedureEntity != null) dto.FoundProcedure = foundProcedureEntity.Name;

                // 填充工序
                if (sfcProduceEntity != null)
                {
                    var procedureEntity = procedureEntities.FirstOrDefault(f => f.Id == sfcProduceEntity.ProcedureId);
                    if (procedureEntity != null)
                    {
                        dto.ProcedureCode = procedureEntity.Code;
                        dto.ProcedureName = procedureEntity.Name;
                    }
                }

                dtos.Add(dto);
            }

            return dtos;
        }

        /// <summary>
        /// 提交（设备误判）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> SubmitMisjudgmentAsync(ManuMisjudgmentDto requestDto)
        {
            if (requestDto == null) return 0;
            if (requestDto.BarCodes == null || !requestDto.BarCodes.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15445));

            // 更新时间
            var siteId = _currentSite.SiteId ?? 0;
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // 查询条码
            var sfcEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                SiteId = siteId,
                SFCs = requestDto.BarCodes
            });
            if (sfcEntities == null || !sfcEntities.Any()) return 0;

            // 查询条码信息
            var sfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsAsync(sfcEntities.Select(s => s.Id));

            // 读取在制品
            var sfcProduceEntities = await _manuSfcProduceRepository.GetBySFCIdsAsync(sfcEntities.Where(w => w.Type == SfcTypeEnum.Produce).Select(s => s.Id));

            // 读取产品
            var productEntities = await _procMaterialRepository.GetByIdsAsync(sfcInfoEntities.Select(s => s.ProductId));

            // 查询不合格记录
            var allBadRecordEntities = await _manuProductBadRecordRepository.GetManuProductBadRecordEntitiesBySFCAsync(new ManuProductBadRecordBySfcQuery
            {
                SiteId = siteId,
                Status = ProductBadRecordStatusEnum.Open,
                SFCs = requestDto.BarCodes
            });
            if (allBadRecordEntities == null || !allBadRecordEntities.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15451)).WithData("barCode", string.Join(',', requestDto.BarCodes));
            var allBadRecordEntitiesDict = allBadRecordEntities.ToLookup(x => x.SFC).ToDictionary(d => d.Key, d => d);

            // 遍历所有条码
            List<ManuSfcStepEntity> sfcStepEntities = new();
            List<ManuProductBadRecordUpdateCommand> badRecordUpdateCommands = new();
            List<WhMaterialInventoryEntity> materialInventoryEntities = new();
            List<WhMaterialStandingbookEntity> materialStandingbookEntities = new();
            List<PhysicalDeleteSFCProduceByIdsCommand> physicalDeleteSFCProduceByIdsCommands = new();
            PhysicalDeleteSFCProduceByIdsCommand physicalDeleteSFCProduceByIdsCommand = new() { SiteId = siteId };

            List<long> sfcProduceIds = new();
            foreach (var sfcEntity in sfcEntities)
            {
                // 每个条码的不合格记录
                if (!allBadRecordEntitiesDict.TryGetValue(sfcEntity.SFC, out var badRecordEntities)) continue;

                // 关闭不合格
                badRecordUpdateCommands.AddRange(badRecordEntities.Select(s => new ManuProductBadRecordUpdateCommand
                {
                    Id = s.Id,
                    Status = ProductBadRecordStatusEnum.Close,
                    DisposalResult = ProductBadDisposalResultEnum.Misjudgment,
                    UpdatedOn = updatedOn,
                    UserId = updatedBy,
                    Remark = requestDto.Remark ?? ""
                }));

                if (sfcEntity.Type == SfcTypeEnum.Produce)
                {
                    // 在制品
                    var sfcProduceEntity = sfcProduceEntities.FirstOrDefault(f => f.SFCId == sfcEntity.Id);
                    if (sfcProduceEntity == null) continue;

                    // 初始化步骤数据
                    var stepEntity = new ManuSfcStepEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        Operatetype = ManuSfcStepTypeEnum.Misjudgment,
                        CurrentStatus = sfcProduceEntity.Status,
                        SFC = sfcProduceEntity.SFC,
                        ProductId = sfcProduceEntity.ProductId,
                        WorkOrderId = sfcProduceEntity.WorkOrderId,
                        WorkCenterId = sfcProduceEntity.WorkCenterId,
                        ProductBOMId = sfcProduceEntity.ProductBOMId,
                        SFCInfoId = sfcProduceEntity.BarCodeInfoId,
                        Qty = sfcProduceEntity.Qty,
                        VehicleCode = "", // 这里要赋值？
                        ProcedureId = sfcProduceEntity.ProcedureId,
                        ResourceId = sfcProduceEntity.ResourceId,
                        EquipmentId = sfcProduceEntity.EquipmentId,
                        Remark = requestDto.Remark,
                        SiteId = sfcEntity.SiteId,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn
                    };
                    sfcStepEntities.Add(stepEntity);

                    // 如果是在制完成，改为完成（默认就处于合格工艺路线里面）
                    if (sfcProduceEntity.Status == SfcStatusEnum.InProductionComplete)
                    {
                        // 产品信息
                        var productEntity = productEntities.FirstOrDefault(f => f.Id == sfcProduceEntity.ProductId);
                        if (productEntity == null) continue;

                        stepEntity.Id = IdGenProvider.Instance.CreateId();
                        stepEntity.Operatetype = ManuSfcStepTypeEnum.OutStock;
                        stepEntity.AfterOperationStatus = SfcStatusEnum.Complete;

                        // 再次添加步骤
                        sfcStepEntities.Add(stepEntity);

                        // 删除在制
                        sfcProduceIds.Add(sfcProduceEntity.Id);

                        // 新增 wh_material_inventory
                        materialInventoryEntities.Add(new WhMaterialInventoryEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SupplierId = 0,//自制品 没有
                            MaterialId = sfcProduceEntity.ProductId,
                            MaterialBarCode = sfcProduceEntity.SFC,
                            Batch = "",//自制品 没有
                            MaterialType = MaterialInventoryMaterialTypeEnum.SelfMadeParts,
                            QuantityResidue = sfcProduceEntity.Qty,
                            Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                            Source = MaterialInventorySourceEnum.ManuComplete,
                            SiteId = sfcEntity.SiteId,
                            CreatedBy = updatedBy,
                            CreatedOn = updatedOn,
                            UpdatedBy = updatedBy,
                            UpdatedOn = updatedOn
                        });

                        // 新增 wh_material_standingbook
                        materialStandingbookEntities.Add(new WhMaterialStandingbookEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            MaterialCode = productEntity.MaterialCode,
                            MaterialName = productEntity.MaterialName,
                            MaterialVersion = productEntity.Version ?? "",
                            MaterialBarCode = sfcProduceEntity.SFC,
                            Batch = "",//自制品 没有
                            Quantity = sfcProduceEntity.Qty,
                            Unit = productEntity.Unit ?? "",
                            Type = WhMaterialInventoryTypeEnum.ManuComplete,
                            Source = MaterialInventorySourceEnum.ManuComplete,
                            SiteId = sfcEntity.SiteId,
                            CreatedBy = updatedBy,
                            CreatedOn = updatedOn,
                            UpdatedBy = updatedBy,
                            UpdatedOn = updatedOn
                        });
                    }
                }
                else if (sfcEntity.Type == SfcTypeEnum.NoProduce)
                {
                    // 条码信息
                    var sfcInfoEntity = sfcInfoEntities.FirstOrDefault(f => f.SfcId == sfcEntity.Id);
                    if (sfcInfoEntity == null) continue;

                    // 产品信息
                    var productEntity = productEntities.FirstOrDefault(f => f.Id == sfcInfoEntity.ProductId);
                    if (productEntity == null) continue;

                    // 添加步骤
                    sfcStepEntities.AddRange(badRecordEntities.Select(s => new ManuSfcStepEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        Operatetype = ManuSfcStepTypeEnum.Misjudgment,
                        CurrentStatus = SfcStatusEnum.Complete,
                        SFC = sfcEntity.SFC,
                        ProductId = sfcInfoEntity.ProductId,
                        WorkOrderId = sfcInfoEntity.WorkOrderId ?? 0,
                        SFCInfoId = sfcInfoEntity.Id,
                        Qty = sfcEntity.Qty,
                        Remark = requestDto.Remark,
                        SiteId = sfcEntity.SiteId,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn
                    }));
                }
                else continue;
            }

            // 需要删除的条码ID
            physicalDeleteSFCProduceByIdsCommand.Ids = sfcProduceIds;

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();

            // 关闭不合格记录
            rows += await _manuProductBadRecordRepository.UpdateStatusByIdRangeAsync(badRecordUpdateCommands);

            // 插入 manu_sfc_step
            rows += await _manuSfcStepRepository.InsertRangeAsync(sfcStepEntities);

            // 删除在制
            rows += await _manuSfcProduceRepository.DeletePhysicalRangeByIdsAsync(physicalDeleteSFCProduceByIdsCommand);

            trans.Complete();
            return rows;
        }
        #endregion


        #region 返工
        /// <summary>
        /// 根据条码查询信息（返工）
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuReworkBarCodeDto>> GetReworkByBarCodeAsync(string barCode)
        {
            if (string.IsNullOrEmpty(barCode)) throw new CustomerValidationException(nameof(ErrorCode.MES15445));

            // 站点
            var siteId = _currentSite.SiteId ?? 0;

            return await GetReworkByBarCodesAsync(siteId, new List<string> { barCode });
        }

        /// <summary>
        /// 根据托盘码条码查询信息（返工）
        /// </summary>
        /// <param name="palletCode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuReworkBarCodeDto>> GetReworkByPalletCodeAsync(string palletCode)
        {
            if (string.IsNullOrEmpty(palletCode)) throw new CustomerValidationException(nameof(ErrorCode.MES19111));

            // 站点
            var siteId = _currentSite.SiteId ?? 0;

            // 根据载具代码获取载具里面的条码
            var vehicleSFCs = await _manuCommonService.GetSFCsByVehicleCodesAsync(new VehicleSFCRequestBo
            {
                SiteId = siteId,
                VehicleCodes = new List<string> { palletCode }
            });
            if (vehicleSFCs == null || !vehicleSFCs.Any()) return Array.Empty<ManuReworkBarCodeDto>();

            return await GetReworkByBarCodesAsync(siteId, vehicleSFCs.Select(s => s.SFC));
        }

        /// <summary>
        /// 提交（返工）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> SubmitReworkAsync(ManuReworkDto requestDto)
        {
            if (requestDto == null) return 0;
            if (requestDto.BarCodes == null || !requestDto.BarCodes.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15445));

            // 初始化请求参数
            var dataBo = new ReworkRequestBo
            {
                SiteId = _currentSite.SiteId ?? 0,
                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now()
            };

            // 查询条码
            dataBo.SFCEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                SiteId = dataBo.SiteId,
                SFCs = requestDto.BarCodes
            });
            if (dataBo.SFCEntities == null || !dataBo.SFCEntities.Any()) return 0;

            // 状态校验
            var validationFailures = new List<ValidationFailure>();
            var noMatchSFCEntities = dataBo.SFCEntities.Where(w => ManuSfcStatus.ForbidSfcStatuss.Contains(w.Status));
            if (noMatchSFCEntities.Any())
            {
                foreach (var sfcEntity in noMatchSFCEntities)
                {
                    var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                    validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfcEntity.SFC);
                    validationFailure.FormattedMessagePlaceholderValues.Add("barCode", sfcEntity.SFC);
                    validationFailure.FormattedMessagePlaceholderValues.Add("status", sfcEntity.Status.GetDescription());
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15447);
                    validationFailures.Add(validationFailure);
                }

                if (validationFailures.Any()) throw new ValidationException("", validationFailures);
            }

            // 查询条码信息
            dataBo.SFCInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsAsync(dataBo.SFCEntities.Select(s => s.Id));

            // 查询工单信息
            dataBo.WorkOrderEntities = await _planWorkOrderRepository.GetByIdsAsync(dataBo.SFCInfoEntities.Where(w => w.WorkOrderId.HasValue).Select(s => s.WorkOrderId!.Value));

            // 读取在制品
            dataBo.SFCProduceEntities = await _manuSfcProduceRepository.GetBySFCIdsAsync(dataBo.SFCEntities.Where(w => w.Type == SfcTypeEnum.Produce).Select(s => s.Id));

            // 读取产品
            dataBo.ProductEntities = await _procMaterialRepository.GetByIdsAsync(dataBo.SFCInfoEntities.Select(s => s.ProductId));

            // 查询不合格记录（开启的状态）
            dataBo.BadRecordEntities = await _manuProductBadRecordRepository.GetManuProductBadRecordEntitiesBySFCAsync(new ManuProductBadRecordBySfcQuery
            {
                SiteId = dataBo.SiteId,
                Status = ProductBadRecordStatusEnum.Open,
                SFCs = requestDto.BarCodes
            });

            // 根据返工类型处理
            ReworkResponseBo? responseBo = null;
            switch (requestDto.Type)
            {
                case ManuReworkTypeEnum.OriginalOrder:
                    responseBo = await ReWorkForOriginalOrderAsync(requestDto, dataBo);
                    break;
                case ManuReworkTypeEnum.NewOrder:
                    responseBo = await ReWorkForNewOrderAsync(requestDto, dataBo);
                    break;
                case ManuReworkTypeEnum.NewOrderCell:
                    responseBo = await ReWorkForNewOrderCellAsync(requestDto, dataBo);
                    break;
                default:
                    break;
            }

            var rows = 0;
            if (responseBo == null) return rows;

            using var trans = TransactionHelper.GetTransactionScope();
            List<Task<int>> tasks = new()
            {
                // 关闭不合格记录
               _manuProductBadRecordRepository.UpdateStatusByIdRangeAsync(responseBo.BadRecordUpdateCommands),

                // 插入 manu_sfc_produce
                _manuSfcProduceRepository.InsertRangeAsync(responseBo.InsertSFCProduceEntities),

                // 更新 manu_sfc_produce
                _manuSfcProduceRepository.UpdateRangeAsync(responseBo.UpdateSFCProduceEntities),

                // 插入 manu_sfc_step
                _manuSfcStepRepository.InsertRangeAsync(responseBo.SFCStepEntities),

                // 插入不良记录
                _manuProductBadRecordRepository.InsertRangeAsync(responseBo.ProductBadRecordEntities),

                // 插入NG记录
                _manuProductNgRecordRepository.InsertRangeAsync(responseBo.ProductNgRecordEntities)
            };

            var rowArray = await Task.WhenAll(tasks);
            trans.Complete();
            return rowArray.Sum();
        }

        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task<string> DownloadImportTemplateAsync(Stream stream)
        {
            var worksheetName = "产品异常处理-返工";
            await _excelService.ExportAsync(Array.Empty<ManuReworkExcelDto>(), stream, worksheetName);
            return worksheetName;
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <returns></returns>
        public async Task ImportAsync(IFormFile formFile)
        {
            using MemoryStream memoryStream = new();
            await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);
            var dtos = _excelService.Import<ManuReworkExcelDto>(memoryStream);
            if (dtos == null || !dtos.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10133));

            /*
            // 备份用户上传的文件，可选
            var stream = formFile.OpenReadStream();
            var uploadResult = await _minioService.PutObjectAsync(formFile.FileName, stream, formFile.ContentType);
            */

            // 分组标准
            var standardDict = dtos.Select(s => s.BarCode).DistinctBy(d => d);
            if (standardDict == null || !standardDict.Any()) return;

            // 站点
            var siteId = _currentSite.SiteId ?? 0;

            // 获取条码结果
            var barCodeDtos = await GetReworkByBarCodesAsync(siteId, standardDict, false);

            // TODO
        }
        #endregion


        #region 离脱
        /// <summary>
        /// 根据条码查询信息（离脱）
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public async Task<ManuDetachmentBarCodeDto> GetDetachmentByBarCodeAsync(string barCode)
        {
            if (string.IsNullOrEmpty(barCode)) throw new CustomerValidationException(nameof(ErrorCode.MES15445));

            // 查询条码
            var sfcEntity = await _manuSfcRepository.GetSingleAsync(new ManuSfcQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                SFC = barCode
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES15446)).WithData("barCode", barCode);

            // 状态校验
            if (ManuSfcStatus.ForbidSfcStatuss.Contains(sfcEntity.Status)) throw new CustomerValidationException(nameof(ErrorCode.MES15447))
                    .WithData("barCode", barCode)
                    .WithData("status", sfcEntity.Status.GetDescription());

            // 初始化返回值
            var dto = new ManuDetachmentBarCodeDto
            {
                BarCode = sfcEntity.SFC,
                Qty = sfcEntity.Qty,
                Type = sfcEntity.Type
            };

            // 查询条码信息
            var sfcInfoEntity = await _manuSfcInfoRepository.GetBySFCIdAsync(sfcEntity.Id)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES15446)).WithData("barCode", barCode);

            // 填充工单
            if (sfcInfoEntity.WorkOrderId.HasValue)
            {
                var workOrderEntity = await _planWorkOrderRepository.GetByIdAsync(sfcInfoEntity.WorkOrderId.Value);
                if (workOrderEntity != null) dto.WorkOrderCode = workOrderEntity.OrderCode;
            }

            // 填充产品
            var productEntity = await _procMaterialRepository.GetByIdAsync(sfcInfoEntity.ProductId);
            if (productEntity != null)
            {
                dto.ProductCode = productEntity.MaterialCode;
                dto.ProductName = productEntity.MaterialName;
            }

            // 填充工序
            if (sfcEntity.Type == SfcTypeEnum.Produce)
            {
                var sfcProduceEntity = await _manuSfcProduceRepository.GetBySFCIdAsync(sfcEntity.Id);
                if (sfcProduceEntity != null)
                {
                    var procedureEntity = await _procProcedureRepository.GetByIdAsync(sfcProduceEntity.ProcedureId);
                    if (procedureEntity != null)
                    {
                        dto.ProcedureCode = procedureEntity.Code;
                        dto.ProcedureName = procedureEntity.Name;
                    }
                }
            }

            return dto;
        }

        /// <summary>
        /// 提交（离脱）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> SubmitDetachmentAsync(ManuDetachmentDto requestDto)
        {
            if (requestDto == null) return 0;
            if (requestDto.BarCodes == null || !requestDto.BarCodes.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15445));

            // 更新时间
            var siteId = _currentSite.SiteId ?? 0;
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // 查询条码
            var sfcEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                SiteId = siteId,
                SFCs = requestDto.BarCodes
            });
            if (sfcEntities == null || !sfcEntities.Any()) return 0;

            // 状态校验
            var validationFailures = new List<ValidationFailure>();
            var noMatchSFCEntities = sfcEntities.Where(w => ManuSfcStatus.ForbidSfcStatuss.Contains(w.Status));
            if (noMatchSFCEntities.Any())
            {
                foreach (var sfcEntity in noMatchSFCEntities)
                {
                    var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                    validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfcEntity.SFC);
                    validationFailure.FormattedMessagePlaceholderValues.Add("barCode", sfcEntity.SFC);
                    validationFailure.FormattedMessagePlaceholderValues.Add("status", sfcEntity.Status.GetDescription());
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15447);
                    validationFailures.Add(validationFailure);
                }

                if (validationFailures.Any()) throw new ValidationException("", validationFailures);
            }

            // 查询条码信息
            var sfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsAsync(sfcEntities.Select(s => s.Id));

            // 读取在制品
            var sfcProduceEntities = await _manuSfcProduceRepository.GetBySFCIdsAsync(sfcEntities.Where(w => w.Type == SfcTypeEnum.Produce).Select(s => s.Id));

            // 读取产品
            var productEntities = await _procMaterialRepository.GetByIdsAsync(sfcInfoEntities.Select(s => s.ProductId));

            // 遍历所有条码
            List<ManuSfcEntity> updateManuSfcEntities = new();
            List<DeletePhysicalBySfcCommand> deletePhysicalBySfcCommands = new();
            List<ManuSfcStepEntity> sfcStepEntities = new();
            List<WhMaterialStandingbookEntity> materialStandingbookEntities = new();

            PhysicalDeleteSFCProduceByIdsCommand physicalDeleteSFCProduceByIdsCommand = new() { SiteId = siteId };
            UpdateWhMaterialInventoryEmptyCommand updateWhMaterialInventoryEmptyCommand = new()
            {
                SiteId = siteId,
                UserName = updatedBy,
                UpdateTime = updatedOn
            };

            List<long> sfcProduceIds = new();
            List<string> sfcInventorys = new();
            foreach (var sfcEntity in sfcEntities)
            {
                if (sfcEntity.Type == SfcTypeEnum.Produce)
                {
                    // 在制品
                    var sfcProduceEntity = sfcProduceEntities.FirstOrDefault(f => f.SFCId == sfcEntity.Id);
                    if (sfcProduceEntity == null) continue;

                    // 删除在制；
                    sfcProduceIds.Add(sfcProduceEntity.Id);

                    // 添加步骤
                    sfcStepEntities.Add(new ManuSfcStepEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        Operatetype = ManuSfcStepTypeEnum.Detachment,
                        CurrentStatus = SfcStatusEnum.Detachment,
                        SFC = sfcProduceEntity.SFC,
                        ProductId = sfcProduceEntity.ProductId,
                        WorkOrderId = sfcProduceEntity.WorkOrderId,
                        WorkCenterId = sfcProduceEntity.WorkCenterId,
                        ProductBOMId = sfcProduceEntity.ProductBOMId,
                        SFCInfoId = sfcProduceEntity.BarCodeInfoId,
                        Qty = sfcProduceEntity.Qty,
                        VehicleCode = "", // 这里要赋值？
                        ProcedureId = sfcProduceEntity.ProcedureId,
                        ResourceId = sfcProduceEntity.ResourceId,
                        EquipmentId = sfcProduceEntity.EquipmentId,
                        Remark = requestDto.Remark,
                        SiteId = sfcEntity.SiteId,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn
                    });
                }
                else if (sfcEntity.Type == SfcTypeEnum.NoProduce)
                {
                    // 条码信息
                    var sfcInfoEntity = sfcInfoEntities.FirstOrDefault(f => f.SfcId == sfcEntity.Id);
                    if (sfcInfoEntity == null) continue;

                    // 产品信息
                    var productEntity = productEntities.FirstOrDefault(f => f.Id == sfcInfoEntity.ProductId);
                    if (productEntity == null) continue;

                    // 添加步骤
                    sfcStepEntities.Add(new ManuSfcStepEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        Operatetype = ManuSfcStepTypeEnum.Detachment,
                        CurrentStatus = SfcStatusEnum.Detachment,
                        SFC = sfcEntity.SFC,
                        ProductId = sfcInfoEntity.ProductId,
                        WorkOrderId = sfcInfoEntity.WorkOrderId ?? 0,
                        SFCInfoId = sfcInfoEntity.Id,
                        Qty = 0,
                        Remark = requestDto.Remark,
                        SiteId = sfcEntity.SiteId,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn
                    });

                    // 清零库存；
                    sfcInventorys.Add(sfcEntity.SFC);

                    // 记录台账；
                    materialStandingbookEntities.Add(new WhMaterialStandingbookEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        MaterialCode = productEntity.MaterialCode,
                        MaterialName = productEntity.MaterialName,
                        MaterialVersion = productEntity.Version ?? "",
                        MaterialBarCode = sfcEntity.SFC,
                        Batch = "",//自制品 没有
                        Quantity = 0,
                        Unit = productEntity.Unit ?? "",
                        Type = WhMaterialInventoryTypeEnum.Detachment,
                        Source = MaterialInventorySourceEnum.Detachment,
                        SiteId = sfcEntity.SiteId,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn
                    });
                }
                else continue;

                // 条码表改状态；
                sfcEntity.Status = SfcStatusEnum.Detachment;
                sfcEntity.UpdatedBy = updatedBy;
                sfcEntity.UpdatedOn = updatedOn;

                updateManuSfcEntities.Add(sfcEntity);
            }

            // 需要删除的条码ID
            physicalDeleteSFCProduceByIdsCommand.Ids = sfcProduceIds;
            updateWhMaterialInventoryEmptyCommand.BarCodeList = sfcInventorys;

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();

            // 删除在制
            rows += await _manuSfcProduceRepository.DeletePhysicalRangeByIdsAsync(physicalDeleteSFCProduceByIdsCommand);

            // 插入 manu_sfc_step
            rows += await _manuSfcStepRepository.InsertRangeAsync(sfcStepEntities);

            // 清零库存
            rows += await _whMaterialInventoryRepository.UpdateWhMaterialInventoryEmptyByBarCodeAync(updateWhMaterialInventoryEmptyCommand);

            // 台账
            rows += await _whMaterialStandingbookRepository.InsertsAsync(materialStandingbookEntities);

            // manu_sfc 更新状态
            rows += await _manuSfcRepository.UpdateRangeWithStatusCheckAsync(updateManuSfcEntities);

            trans.Complete();
            return rows;
        }
        #endregion



        #region 内部方法
        /// <summary>
        /// 根据条码集合查询信息（返工）
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="barCodes"></param>
        /// <param name="isFillInfo"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuReworkBarCodeDto>> GetReworkByBarCodesAsync(long siteId, IEnumerable<string> barCodes, bool isFillInfo = true)
        {
            if (barCodes == null || !barCodes.Any()) return Array.Empty<ManuReworkBarCodeDto>();

            // 初始化返回值
            List<ManuReworkBarCodeDto> dtos = new();

            // 查询条码
            var sfcEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                SiteId = siteId,
                SFCs = barCodes
            });
            if (sfcEntities == null || !sfcEntities.Any()) return dtos;

            // 状态校验
            var validationFailures = new List<ValidationFailure>();
            var noMatchSFCEntities = sfcEntities.Where(w => ManuSfcStatus.ForbidSfcStatuss.Contains(w.Status));
            if (noMatchSFCEntities.Any())
            {
                foreach (var sfcEntity in noMatchSFCEntities)
                {
                    var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                    validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfcEntity.SFC);
                    validationFailure.FormattedMessagePlaceholderValues.Add("barCode", sfcEntity.SFC);
                    validationFailure.FormattedMessagePlaceholderValues.Add("status", sfcEntity.Status.GetDescription());
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15447);
                    validationFailures.Add(validationFailure);
                }

                if (validationFailures.Any()) throw new ValidationException("", validationFailures);
            }

            // 查询条码信息
            var sfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsAsync(sfcEntities.Select(s => s.Id));

            // 读取在制品
            var sfcProduceEntities = await _manuSfcProduceRepository.GetBySFCIdsAsync(sfcEntities.Where(w => w.Type == SfcTypeEnum.Produce).Select(s => s.Id));

            // 读取产品
            var productEntities = await _procMaterialRepository.GetByIdsAsync(sfcInfoEntities.Select(s => s.ProductId));

            // 遍历所有条码
            foreach (var sfcEntity in sfcEntities)
            {
                var dto = new ManuReworkBarCodeDto
                {
                    Id = sfcEntity.Id,
                    BarCode = sfcEntity.SFC,
                    Qty = sfcEntity.Qty
                };

                // 是否填充额外信息 
                if (!isFillInfo)
                {
                    dtos.Add(dto);
                    continue;
                }

                // 查询条码信息
                var sfcInfoEntity = await _manuSfcInfoRepository.GetBySFCIdAsync(sfcEntity.Id)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES15446)).WithData("barCode", sfcEntity.SFC);

                // 填充工单
                if (sfcInfoEntity.WorkOrderId.HasValue)
                {
                    var workOrderEntity = await _planWorkOrderRepository.GetByIdAsync(sfcInfoEntity.WorkOrderId.Value);
                    if (workOrderEntity != null) dto.WorkOrderCode = workOrderEntity.OrderCode;
                }

                // 填充产品
                var productEntity = await _procMaterialRepository.GetByIdAsync(sfcInfoEntity.ProductId);
                if (productEntity != null)
                {
                    dto.ProductCode = productEntity.MaterialCode;
                    dto.ProductName = productEntity.MaterialName;
                }

                dtos.Add(dto);
            }

            return dtos;
        }

        /// <summary>
        /// 原工单返工
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="dataBo"></param>
        /// <returns></returns>
        public async Task<ReworkResponseBo> ReWorkForOriginalOrderAsync(ManuReworkDto requestDto, ReworkRequestBo dataBo)
        {
            var responseBo = new ReworkResponseBo();

            if (!requestDto.UnqualifiedCode.HasValue) throw new CustomerValidationException(nameof(ErrorCode.MES19702));
            if (!requestDto.FoundProcedure.HasValue) throw new CustomerValidationException(nameof(ErrorCode.MES15433));
            if (!requestDto.OutProcedure.HasValue) throw new CustomerValidationException(nameof(ErrorCode.MES15457));
            if (!requestDto.ReworkProcedure.HasValue) throw new CustomerValidationException(nameof(ErrorCode.MES15458));

            // 查询传入的不合格代码
            var unqualifiedCodeEntity = await _qualUnqualifiedCodeRepository.GetByIdAsync(requestDto.UnqualifiedCode.Value);

            // 不良记录分组
            var badRecordEntitiesDict = dataBo.BadRecordEntities.ToLookup(x => x.SFC).ToDictionary(d => d.Key, d => d);

            // 遍历所有条码
            foreach (var sfcEntity in dataBo.SFCEntities)
            {
                // 条码信息
                var sfcInfoEntity = dataBo.SFCInfoEntities.FirstOrDefault(f => f.SfcId == sfcEntity.Id);
                if (sfcInfoEntity == null) continue;

                // 关闭不合格记录（如果有的话）
                if (badRecordEntitiesDict.TryGetValue(sfcEntity.SFC, out var badRecordEntities))
                {
                    responseBo.BadRecordUpdateCommands.AddRange(badRecordEntities.Select(s => new ManuProductBadRecordUpdateCommand
                    {
                        Id = s.Id,
                        Status = ProductBadRecordStatusEnum.Close,
                        DisposalResult = ProductBadDisposalResultEnum.Rework,
                        UpdatedOn = dataBo.UpdatedOn,
                        UserId = dataBo.UpdatedBy,
                        Remark = requestDto.Remark ?? ""
                    }));
                }

                // 初始化步骤数据
                var sfcStepEntity = new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    Operatetype = ManuSfcStepTypeEnum.Rework,
                    SFC = sfcEntity.SFC,
                    ProductId = sfcInfoEntity.ProductId,
                    SFCInfoId = sfcInfoEntity.Id,
                    Qty = sfcEntity.Qty,
                    VehicleCode = "", // 这里要赋值？
                    Remark = requestDto.Remark,
                    SiteId = sfcEntity.SiteId,
                    CreatedBy = dataBo.UpdatedBy,
                    CreatedOn = dataBo.UpdatedOn,
                    UpdatedBy = dataBo.UpdatedBy,
                    UpdatedOn = dataBo.UpdatedOn
                };

                ManuSfcProduceEntity? sfcProduceEntity = null;
                if (sfcEntity.Type == SfcTypeEnum.Produce)
                {
                    // 在制品
                    sfcProduceEntity = dataBo.SFCProduceEntities.FirstOrDefault(f => f.SFCId == sfcEntity.Id);
                    if (sfcProduceEntity == null) continue;

                    // 修改在制信息
                    sfcProduceEntity.ProcedureId = requestDto.ReworkProcedure ?? 0;
                    sfcProduceEntity.ResourceId = null;
                    sfcProduceEntity.EquipmentId = null;
                    sfcProduceEntity.Status = SfcStatusEnum.lineUp;
                    responseBo.UpdateSFCProduceEntities.Add(sfcProduceEntity);
                }
                else if (sfcEntity.Type == SfcTypeEnum.NoProduce)
                {
                    // 产品信息
                    var productEntity = dataBo.ProductEntities.FirstOrDefault(f => f.Id == sfcInfoEntity.ProductId);
                    if (productEntity == null) continue;

                    // 工单信息
                    var workOrderEntity = dataBo.WorkOrderEntities.FirstOrDefault(f => f.Id == sfcInfoEntity.WorkOrderId);
                    if (workOrderEntity == null) continue;

                    // 新增在制品
                    sfcProduceEntity = new ManuSfcProduceEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SFC = sfcEntity.SFC,
                        SFCId = sfcEntity.Id,
                        ProductId = sfcInfoEntity.ProductId,
                        WorkOrderId = sfcInfoEntity.WorkOrderId ?? 0,
                        BarCodeInfoId = sfcInfoEntity.Id,
                        ProcessRouteId = workOrderEntity.ProcessRouteId,
                        WorkCenterId = workOrderEntity.WorkCenterId ?? 0,
                        ProductBOMId = workOrderEntity.ProductBOMId,
                        Qty = productEntity.Batch,
                        ProcedureId = requestDto.ReworkProcedure ?? 0,
                        Status = SfcStatusEnum.lineUp,
                        RepeatedCount = 0,
                        IsScrap = TrueOrFalseEnum.No,
                        SiteId = sfcEntity.SiteId,
                        CreatedBy = dataBo.UpdatedBy,
                        CreatedOn = dataBo.UpdatedOn,
                        UpdatedBy = dataBo.UpdatedBy,
                        UpdatedOn = dataBo.UpdatedOn
                    };
                    responseBo.InsertSFCProduceEntities.Add(sfcProduceEntity);
                }
                else continue;

                // 添加步骤
                sfcStepEntity.WorkOrderId = sfcProduceEntity.WorkOrderId;
                sfcStepEntity.WorkCenterId = sfcProduceEntity.WorkCenterId;
                sfcStepEntity.ProductBOMId = sfcProduceEntity.ProductBOMId;
                sfcStepEntity.ProcedureId = sfcProduceEntity.ProcedureId;
                sfcStepEntity.ResourceId = sfcProduceEntity.ResourceId;
                sfcStepEntity.EquipmentId = sfcProduceEntity.EquipmentId;
                sfcStepEntity.CurrentStatus = sfcProduceEntity.Status;
                responseBo.SFCStepEntities.Add(sfcStepEntity);

                // 添加不良记录
                var badRecordId = IdGenProvider.Instance.CreateId();
                responseBo.ProductBadRecordEntities.Add(new ManuProductBadRecordEntity
                {
                    Id = badRecordId,
                    SiteId = sfcEntity.SiteId,
                    FoundBadOperationId = requestDto.FoundProcedure.Value,
                    FoundBadResourceId = null,
                    OutflowOperationId = requestDto.OutProcedure.Value,
                    UnqualifiedId = unqualifiedCodeEntity.Id,
                    SfcStepId = sfcStepEntity.Id,
                    SFC = sfcEntity.SFC,
                    SfcInfoId = sfcInfoEntity.Id,
                    Qty = sfcEntity.Qty,
                    Status = ProductBadRecordStatusEnum.Close,
                    Source = ProductBadRecordSourceEnum.BadManualEntry,
                    Remark = "",
                    DisposalResult = ProductBadDisposalResultEnum.Rework,
                    CreatedBy = dataBo.UpdatedBy,
                    CreatedOn = dataBo.UpdatedOn,
                    UpdatedBy = dataBo.UpdatedBy,
                    UpdatedOn = dataBo.UpdatedOn
                });

                // 添加NG记录
                responseBo.ProductNgRecordEntities.Add(new ManuProductNgRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = sfcEntity.SiteId,
                    BadRecordId = badRecordId,
                    UnqualifiedId = unqualifiedCodeEntity.Id,
                    NGCode = unqualifiedCodeEntity.UnqualifiedCode,
                    Remark = "",
                    CreatedBy = dataBo.UpdatedBy,
                    CreatedOn = dataBo.UpdatedOn,
                    UpdatedBy = dataBo.UpdatedBy,
                    UpdatedOn = dataBo.UpdatedOn
                });
            }

            return responseBo;
        }

        /// <summary>
        /// 新工单返工
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="dataBo"></param>
        /// <returns></returns>
        public async Task<ReworkResponseBo> ReWorkForNewOrderAsync(ManuReworkDto requestDto, ReworkRequestBo dataBo)
        {
            var responseBo = new ReworkResponseBo();

            if (!requestDto.UnqualifiedCode.HasValue) throw new CustomerValidationException(nameof(ErrorCode.MES19702));
            if (!requestDto.FoundProcedure.HasValue) throw new CustomerValidationException(nameof(ErrorCode.MES15433));
            if (!requestDto.OutProcedure.HasValue) throw new CustomerValidationException(nameof(ErrorCode.MES15457));
            if (!requestDto.ReworkWorkOrder.HasValue) throw new CustomerValidationException(nameof(ErrorCode.MES15456));

            // 查询条码（库存中）
            var inventorySFCEntities = dataBo.SFCEntities.Where(w => w.Type == SfcTypeEnum.NoProduce);
            if (inventorySFCEntities != null && inventorySFCEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15453)).WithData("barCode", inventorySFCEntities.Select(s => s.SFC));
            }

            // 查询传入的不合格代码
            var unqualifiedCodeEntity = await _qualUnqualifiedCodeRepository.GetByIdAsync(requestDto.UnqualifiedCode.Value);

            // 查询传入的工单
            var workOrderEntity = await _planWorkOrderRepository.GetByIdAsync(requestDto.ReworkWorkOrder.Value)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16301));

            // 检验工单
            await CheckWorkOrderAsync(dataBo, workOrderEntity);

            // 读取工单的工艺路线的首工序
            var routeProcedureDto = await _masterDataService.GetFirstProcedureAsync(workOrderEntity.ProcessRouteId);

            // 不良记录分组
            var badRecordEntitiesDict = dataBo.BadRecordEntities.ToLookup(x => x.SFC).ToDictionary(d => d.Key, d => d);

            // 遍历所有条码
            foreach (var sfcEntity in dataBo.SFCEntities)
            {
                // 条码信息
                var sfcInfoEntity = dataBo.SFCInfoEntities.FirstOrDefault(f => f.SfcId == sfcEntity.Id);
                if (sfcInfoEntity == null) continue;

                // 关闭不合格记录（如果有的话）
                if (badRecordEntitiesDict.TryGetValue(sfcEntity.SFC, out var badRecordEntities))
                {
                    responseBo.BadRecordUpdateCommands.AddRange(badRecordEntities.Select(s => new ManuProductBadRecordUpdateCommand
                    {
                        Id = s.Id,
                        Status = ProductBadRecordStatusEnum.Close,
                        DisposalResult = ProductBadDisposalResultEnum.Rework,
                        UpdatedOn = dataBo.UpdatedOn,
                        UserId = dataBo.UpdatedBy,
                        Remark = requestDto.Remark ?? ""
                    }));
                }

                // 在制品
                var sfcProduceEntity = dataBo.SFCProduceEntities.FirstOrDefault(f => f.SFCId == sfcEntity.Id);
                if (sfcProduceEntity == null) continue;

                // 修改在制信息
                sfcProduceEntity.WorkOrderId = workOrderEntity.Id;
                sfcProduceEntity.WorkCenterId = workOrderEntity.WorkCenterId ?? 0;
                sfcProduceEntity.ProcessRouteId = workOrderEntity.ProcessRouteId;
                sfcProduceEntity.ProductBOMId = workOrderEntity.ProductBOMId;
                sfcProduceEntity.ProcedureId = routeProcedureDto.ProcedureId;
                sfcProduceEntity.ResourceId = null;
                sfcProduceEntity.EquipmentId = null;
                sfcProduceEntity.Status = SfcStatusEnum.lineUp;
                responseBo.UpdateSFCProduceEntities.Add(sfcProduceEntity);

                // 添加步骤
                var sfcStepId = IdGenProvider.Instance.CreateId();
                responseBo.SFCStepEntities.Add(new ManuSfcStepEntity
                {
                    Id = sfcStepId,
                    Operatetype = ManuSfcStepTypeEnum.Rework,
                    SFC = sfcEntity.SFC,
                    ProductId = sfcInfoEntity.ProductId,
                    SFCInfoId = sfcInfoEntity.Id,
                    WorkOrderId = sfcProduceEntity.WorkOrderId,
                    WorkCenterId = sfcProduceEntity.WorkCenterId,
                    ProductBOMId = sfcProduceEntity.ProductBOMId,
                    ProcedureId = sfcProduceEntity.ProcedureId,
                    ResourceId = sfcProduceEntity.ResourceId,
                    EquipmentId = sfcProduceEntity.EquipmentId,
                    CurrentStatus = sfcProduceEntity.Status,
                    Qty = sfcEntity.Qty,
                    VehicleCode = "", // 这里要赋值？
                    Remark = requestDto.Remark,
                    SiteId = sfcEntity.SiteId,
                    CreatedBy = dataBo.UpdatedBy,
                    CreatedOn = dataBo.UpdatedOn,
                    UpdatedBy = dataBo.UpdatedBy,
                    UpdatedOn = dataBo.UpdatedOn
                });

                // 添加不良记录
                var badRecordId = IdGenProvider.Instance.CreateId();
                responseBo.ProductBadRecordEntities.Add(new ManuProductBadRecordEntity
                {
                    Id = badRecordId,
                    SiteId = sfcEntity.SiteId,
                    FoundBadOperationId = requestDto.FoundProcedure.Value,
                    FoundBadResourceId = null,
                    OutflowOperationId = requestDto.OutProcedure.Value,
                    UnqualifiedId = unqualifiedCodeEntity.Id,
                    SfcStepId = sfcStepId,
                    SFC = sfcEntity.SFC,
                    SfcInfoId = sfcInfoEntity.Id,
                    Qty = sfcEntity.Qty,
                    Status = ProductBadRecordStatusEnum.Close,
                    Source = ProductBadRecordSourceEnum.BadManualEntry,
                    Remark = "",
                    DisposalResult = ProductBadDisposalResultEnum.Rework,
                    CreatedBy = dataBo.UpdatedBy,
                    CreatedOn = dataBo.UpdatedOn,
                    UpdatedBy = dataBo.UpdatedBy,
                    UpdatedOn = dataBo.UpdatedOn
                });

                // 添加NG记录
                responseBo.ProductNgRecordEntities.Add(new ManuProductNgRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = sfcEntity.SiteId,
                    BadRecordId = badRecordId,
                    UnqualifiedId = unqualifiedCodeEntity.Id,
                    NGCode = unqualifiedCodeEntity.UnqualifiedCode,
                    Remark = "",
                    CreatedBy = dataBo.UpdatedBy,
                    CreatedOn = dataBo.UpdatedOn,
                    UpdatedBy = dataBo.UpdatedBy,
                    UpdatedOn = dataBo.UpdatedOn
                });
            }

            return responseBo;
        }

        /// <summary>
        /// 新工单返工（成品电芯）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="dataBo"></param>
        /// <returns></returns>
        public async Task<ReworkResponseBo> ReWorkForNewOrderCellAsync(ManuReworkDto requestDto, ReworkRequestBo dataBo)
        {
            var responseBo = new ReworkResponseBo();

            if (!requestDto.UnqualifiedCode.HasValue) throw new CustomerValidationException(nameof(ErrorCode.MES19702));
            if (!requestDto.ReworkWorkOrder.HasValue) throw new CustomerValidationException(nameof(ErrorCode.MES15456));

            // 查询在制条码
            var produceSFCEntities = dataBo.SFCEntities.Where(w => w.Type == SfcTypeEnum.Produce);
            if (produceSFCEntities != null && produceSFCEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15454)).WithData("barCode", produceSFCEntities.Select(s => s.SFC));
            }

            // 查询传入的不合格代码
            var unqualifiedCodeEntity = await _qualUnqualifiedCodeRepository.GetByIdAsync(requestDto.UnqualifiedCode.Value);

            // 查询传入的工单
            var workOrderEntity = await _planWorkOrderRepository.GetByIdAsync(requestDto.ReworkWorkOrder.Value)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16301));

            // 检验工单
            await CheckWorkOrderAsync(dataBo, workOrderEntity);

            // 读取工单的工艺路线的首工序
            var routeProcedureDto = await _masterDataService.GetFirstProcedureAsync(workOrderEntity.ProcessRouteId);

            // 不良记录分组
            var badRecordEntitiesDict = dataBo.BadRecordEntities.ToLookup(x => x.SFC).ToDictionary(d => d.Key, d => d);

            // 遍历所有条码
            foreach (var sfcEntity in dataBo.SFCEntities)
            {
                // 条码信息
                var sfcInfoEntity = dataBo.SFCInfoEntities.FirstOrDefault(f => f.SfcId == sfcEntity.Id);
                if (sfcInfoEntity == null) continue;

                // 关闭不合格记录（如果有的话）
                if (badRecordEntitiesDict.TryGetValue(sfcEntity.SFC, out var badRecordEntities))
                {
                    responseBo.BadRecordUpdateCommands.AddRange(badRecordEntities.Select(s => new ManuProductBadRecordUpdateCommand
                    {
                        Id = s.Id,
                        Status = ProductBadRecordStatusEnum.Close,
                        DisposalResult = ProductBadDisposalResultEnum.Rework,
                        UpdatedOn = dataBo.UpdatedOn,
                        UserId = dataBo.UpdatedBy,
                        Remark = requestDto.Remark ?? ""
                    }));
                }

                // 产品信息
                var productEntity = dataBo.ProductEntities.FirstOrDefault(f => f.Id == sfcInfoEntity.ProductId);
                if (productEntity == null) continue;

                // 新增在制品
                var sfcProduceEntity = new ManuSfcProduceEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = sfcEntity.SFC,
                    SFCId = sfcEntity.Id,
                    ProductId = sfcInfoEntity.ProductId,
                    BarCodeInfoId = sfcInfoEntity.Id,
                    WorkOrderId = workOrderEntity.Id,
                    WorkCenterId = workOrderEntity.WorkCenterId ?? 0,
                    ProcessRouteId = workOrderEntity.ProcessRouteId,
                    ProductBOMId = workOrderEntity.ProductBOMId,
                    Qty = productEntity.Batch,
                    ProcedureId = routeProcedureDto.ProcedureId,
                    Status = SfcStatusEnum.lineUp,
                    RepeatedCount = 0,
                    IsScrap = TrueOrFalseEnum.No,
                    SiteId = sfcEntity.SiteId,
                    CreatedBy = dataBo.UpdatedBy,
                    CreatedOn = dataBo.UpdatedOn,
                    UpdatedBy = dataBo.UpdatedBy,
                    UpdatedOn = dataBo.UpdatedOn
                };
                responseBo.InsertSFCProduceEntities.Add(sfcProduceEntity);

                // 添加步骤
                var sfcStepId = IdGenProvider.Instance.CreateId();
                responseBo.SFCStepEntities.Add(new ManuSfcStepEntity
                {
                    Id = sfcStepId,
                    Operatetype = ManuSfcStepTypeEnum.Rework,
                    SFC = sfcEntity.SFC,
                    ProductId = sfcInfoEntity.ProductId,
                    SFCInfoId = sfcInfoEntity.Id,
                    WorkOrderId = sfcProduceEntity.WorkOrderId,
                    WorkCenterId = sfcProduceEntity.WorkCenterId,
                    ProductBOMId = sfcProduceEntity.ProductBOMId,
                    ProcedureId = sfcProduceEntity.ProcedureId,
                    ResourceId = sfcProduceEntity.ResourceId,
                    EquipmentId = sfcProduceEntity.EquipmentId,
                    CurrentStatus = sfcProduceEntity.Status,
                    Qty = sfcEntity.Qty,
                    VehicleCode = "", // 这里要赋值？
                    Remark = requestDto.Remark,
                    SiteId = sfcEntity.SiteId,
                    CreatedBy = dataBo.UpdatedBy,
                    CreatedOn = dataBo.UpdatedOn,
                    UpdatedBy = dataBo.UpdatedBy,
                    UpdatedOn = dataBo.UpdatedOn
                });

                // 添加不良记录
                var badRecordId = IdGenProvider.Instance.CreateId();
                responseBo.ProductBadRecordEntities.Add(new ManuProductBadRecordEntity
                {
                    Id = badRecordId,
                    SiteId = sfcEntity.SiteId,
                    FoundBadOperationId = 0,
                    FoundBadResourceId = null,
                    OutflowOperationId = 0,
                    UnqualifiedId = unqualifiedCodeEntity.Id,
                    SfcStepId = sfcStepId,
                    SFC = sfcEntity.SFC,
                    SfcInfoId = sfcInfoEntity.Id,
                    Qty = sfcEntity.Qty,
                    Status = ProductBadRecordStatusEnum.Close,
                    Source = ProductBadRecordSourceEnum.BadManualEntry,
                    Remark = "",
                    DisposalResult = ProductBadDisposalResultEnum.Rework,
                    CreatedBy = dataBo.UpdatedBy,
                    CreatedOn = dataBo.UpdatedOn,
                    UpdatedBy = dataBo.UpdatedBy,
                    UpdatedOn = dataBo.UpdatedOn
                });

                // 添加NG记录
                responseBo.ProductNgRecordEntities.Add(new ManuProductNgRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = sfcEntity.SiteId,
                    BadRecordId = badRecordId,
                    UnqualifiedId = unqualifiedCodeEntity.Id,
                    NGCode = unqualifiedCodeEntity.UnqualifiedCode,
                    Remark = "",
                    CreatedBy = dataBo.UpdatedBy,
                    CreatedOn = dataBo.UpdatedOn,
                    UpdatedBy = dataBo.UpdatedBy,
                    UpdatedOn = dataBo.UpdatedOn
                });
            }

            return responseBo;
        }

        /// <summary>
        /// 检验工单
        /// </summary>
        /// <param name="dataBo"></param>
        /// <param name="workOrderEntity"></param>
        /// <returns></returns>
        private async Task CheckWorkOrderAsync(ReworkRequestBo dataBo, PlanWorkOrderEntity workOrderEntity)
        {
            // 校验条码对应的产品与工单产品是否一致
            var noSameProductSFCInfoEntities = dataBo.SFCInfoEntities.Where(w => w.ProductId != workOrderEntity.ProductId);
            if (noSameProductSFCInfoEntities == null || !noSameProductSFCInfoEntities.Any()) return;

            // 读取工单BOM
            var productBOMEntities = await _masterDataService.GetBomDetailEntitiesByBomIdAsync(workOrderEntity.ProductBOMId);
            if (productBOMEntities != null && productBOMEntities.Any())
            {
                // 如果选择的是新工单返工/新工单返工（成品电芯），校验条码对应的产品与工单产品是否一致，产品是否包含在新工单BOM中，若二者条件都不满足
                var ids = noSameProductSFCInfoEntities.Select(w => w.ProductId).Where(w => !productBOMEntities.Select(s => s.MaterialId).Contains(w));
                if (ids != null && ids.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15455));

                /*
                var ids = noSameProductSFCInfoEntities.Select(w => w.ProductId).Intersect(productBOMEntities.Select(s => s.MaterialId));
                if (ids == null || !ids.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15455));
                */
            }
        }

        #endregion

    }
}
