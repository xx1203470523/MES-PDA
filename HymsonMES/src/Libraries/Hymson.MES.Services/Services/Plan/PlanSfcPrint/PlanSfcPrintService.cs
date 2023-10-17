using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.EventBus.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuCreateBarcode;
using Hymson.MES.CoreServices.Events.ManufactureEvents.ManuSfcStepEvents;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Manufacture.ManuCreateBarcode;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.HttpClients;
using Hymson.MES.HttpClients.Requests.Print;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuCreateBarcodeDto;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Services.Plan
{
    /// <summary>
    /// 条码打印 服务
    /// </summary>
    public class PlanSfcPrintService : IPlanSfcPrintService
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
        /// 验证器
        /// </summary>
        private readonly AbstractValidator<PlanSfcPrintCreateDto> _validationCreateRules;
        private readonly AbstractValidator<PlanSfcPrintCreatePrintDto> _validationCreatePrintRules;

        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 仓储（条码）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 仓储（条码信息）
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

        /// <summary>
        /// 仓储（条码在制品）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储（工单）
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        private readonly IManuCreateBarcodeService _manuCreateBarcodeService;

        /// <summary>
        /// 打印机配置
        /// </summary>
        private readonly IProcPrintConfigRepository _procPrintConfigRepository;
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IProcLabelTemplateRepository _procLabelTemplateRepository;
        private readonly ILabelPrintRequest _labelPrintRequest;
        private readonly ILocalizationService _localizationService;
        /// <summary>
        /// 工序-物料-打印模板
        /// </summary>
        private readonly IProcProcedurePrintRelationRepository _procProcedurePrintRelationRepository;

        /// <summary>
        /// 事件总线
        /// </summary>
        private readonly IEventBus<EventBusInstance1> _eventBus;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationCreatePrintRules"></param>
        /// <param name="manuCommonService"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="procPrintConfigRepository"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="procProcedurePrintRelationRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procLabelTemplateRepository"></param>
        /// <param name="labelPrintRequest"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="manuCreateBarcodeService"></param>
        /// <param name="localizationService"></param>
        /// <param name="eventBus"></param>
        public PlanSfcPrintService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<PlanSfcPrintCreateDto> validationCreateRules,
            AbstractValidator<PlanSfcPrintCreatePrintDto> validationCreatePrintRules,
            IManuCommonService manuCommonService,
            IManuSfcRepository manuSfcRepository,
            IProcPrintConfigRepository procPrintConfigRepository,
            IProcResourceRepository procResourceRepository,
            IProcProcedureRepository procProcedureRepository,
            IProcProcedurePrintRelationRepository procProcedurePrintRelationRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcLabelTemplateRepository procLabelTemplateRepository,
            ILabelPrintRequest labelPrintRequest,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuCreateBarcodeService manuCreateBarcodeService,
            ILocalizationService localizationService, 
            IEventBus<EventBusInstance1> eventBus)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationCreateRules = validationCreateRules;
            _validationCreatePrintRules = validationCreatePrintRules;
            _manuCommonService = manuCommonService;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procPrintConfigRepository = procPrintConfigRepository;
            _procResourceRepository = procResourceRepository;
            _procProcedureRepository = procProcedureRepository;
            _procProcedurePrintRelationRepository = procProcedurePrintRelationRepository;
            _procMaterialRepository = procMaterialRepository;
            _procLabelTemplateRepository = procLabelTemplateRepository;
            _labelPrintRequest = labelPrintRequest;
            _manuCreateBarcodeService = manuCreateBarcodeService;
            _localizationService = localizationService;
            _eventBus = eventBus;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task CreateAsync(PlanSfcPrintCreateDto createDto)
        {
            // 验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(createDto);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task CreatePrintAsync(PlanSfcPrintCreatePrintDto createDto)
        {
            // 验证DTO
            await _validationCreatePrintRules.ValidateAndThrowAsync(createDto);

            var resourceEntity = await _procResourceRepository.GetResByIdAsync(createDto.ResourceId);
            var procedureEntity = await _procProcedureRepository.GetByIdAsync(createDto.ProcedureId);
            // 对工序资源类型和资源的资源类型校验
            if (resourceEntity != null && procedureEntity != null && procedureEntity.ResourceTypeId.HasValue && resourceEntity.ResTypeId != procedureEntity.ResourceTypeId.Value)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16507));
            }

            var print = await _procPrintConfigRepository.GetByIdAsync(createDto.PrintId);
            if (print == null)
                throw new CustomerValidationException(nameof(ErrorCode.MES17002));
            PlanWorkOrderEntity work;
            if (createDto.WorkOrderId == 0)
            {
                work = await _planWorkOrderRepository.GetByCodeAsync(new Data.Repositories.Plan.PlanWorkOrder.Query.PlanWorkOrderQuery()
                {
                    OrderCode = createDto.OrderCode,
                    SiteId = _currentSite.SiteId ?? 0
                });
            }
            else
            {
                work = await _planWorkOrderRepository.GetByIdAsync(createDto.WorkOrderId);
            }
            var material = await _procMaterialRepository.GetByIdAsync(work.ProductId);
            var ppr = await _procProcedurePrintRelationRepository.GetProcProcedurePrintReleationEntitiesAsync(new ProcProcedurePrintReleationQuery()
            {
                MaterialId = material.Id,
                ProcedureId = createDto.ProcedureId,
                Version = material?.Version ?? "",
                SiteId = _currentSite.SiteId ?? 0

            });
            PrintRequest printEntity = new PrintRequest();

            foreach (var pprp in ppr)
            {
                var tl = await _procLabelTemplateRepository.GetByIdAsync(pprp.TemplateId);
                if (tl != null)
                {
                    var body = new PrintBody()
                    {
                        TemplatePath = tl.Path,
                        PrinterName = print.PrintName,
                        PrintCount = pprp.Copy ?? 1,
                        Params = new List<PrintBody.ParamEntity>()
                        {
                            new PrintBody.ParamEntity()
                            {
                                ParamName = "SFC",
                                ParamValue = createDto.SFC !
                            },
                            new PrintBody.ParamEntity()
                            {
                                ParamName = "SiteId",
                                ParamValue = (_currentSite.SiteId??123456).ToString()
                            }
                        }

                    };
                    printEntity.Bodies.Add(body);
                }
                else
                    throw new CustomerValidationException(nameof(ErrorCode.MES17001));
            }
            var result = await _labelPrintRequest.PrintAsync(printEntity);
            if (!result.result)
                throw new CustomerValidationException(nameof(ErrorCode.MES17003)).WithData("msg", result.msg);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            var sfcEntities = await _manuSfcRepository.GetByIdsAsync(idsArr);
            if (sfcEntities.Any(it => it.IsUsed == YesOrNoEnum.Yes)) throw new CustomerValidationException(nameof(ErrorCode.MES16116));
            if (sfcEntities.Any(it => it.Status == SfcStatusEnum.Scrapping)) throw new CustomerValidationException(nameof(ErrorCode.MES16130));

            // 对锁定状态进行验证
            await _manuCommonService.VerifySfcsLockAsync(new ManuProcedureBo
            {
                SiteId = 0,
                SFCs = sfcEntities.Select(s => s.SFC)
            });

            // 条码集合
            var sfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsAsync(sfcEntities.Select(s => s.Id));

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _manuSfcRepository.DeletesAsync(new DeleteCommand
                {
                    Ids = idsArr,
                    UserId = _currentUser.UserName,
                    DeleteOn = HymsonClock.Now()
                });
                rows += await _manuSfcProduceRepository.DeletePhysicalRangeAsync(new Data.Repositories.Manufacture.ManuSfcProduce.Command.DeletePhysicalBySfcsCommand()
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    Sfcs = sfcEntities.Select(s => s.SFC).ToArray()
                });
                //rows += await _manuSfcStepRepository.InsertRangeAsync(sfcEntities.Select(s => new ManuSfcStepEntity
                //{
                //    Id = IdGenProvider.Instance.CreateId(),
                //    SiteId = _currentSite.SiteId ?? 0,
                //    SFC = s.SFC,
                //    Qty = s.Qty,
                //    ProductId = sfcInfoEntities.FirstOrDefault(f => f.SfcId == s.Id)!.ProductId,
                //    WorkOrderId = sfcInfoEntities.FirstOrDefault(f => f.SfcId == s.Id)!.WorkOrderId,
                //    Operatetype = ManuSfcStepTypeEnum.Delete,
                //    CurrentStatus = SfcStatusEnum.Complete,
                //    CreatedBy = _currentUser.UserName,
                //    UpdatedBy = _currentUser.UserName
                //}));
                _eventBus.Publish(new ManuSfcStepsEvent { manuSfcStepEntities = sfcEntities.Select(s => new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SFC = s.SFC,
                    Qty = s.Qty,
                    ProductId = sfcInfoEntities.FirstOrDefault(f => f.SfcId == s.Id)!.ProductId,
                    WorkOrderId = sfcInfoEntities.FirstOrDefault(f => f.SfcId == s.Id)!.WorkOrderId,
                    Operatetype = ManuSfcStepTypeEnum.Delete,
                    CurrentStatus = SfcStatusEnum.Complete,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName
                })
                });
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 分页查询列表（条码打印）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanSfcPrintDto>> GetPagedListAsync(PlanSfcPrintPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ManuSfcPassDownPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _manuSfcRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => new PlanSfcPrintDto
            {
                Id = s.Id,
                SFC = s.SFC,
                IsUsed = s.IsUsed,
                UpdatedOn = s.UpdatedOn,
                OrderCode = s.OrderCode,
                MaterialCode = s.MaterialCode,
                MaterialName = s.MaterialName,
                BuyType = s.BuyType
            });
            return new PagedInfo<PlanSfcPrintDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<List<ManuSfcEntity>> CreateBarcodeByWorkOrderIdAsync(CreateBarcodeByWorkOrderDto parm)
        {
            return await _manuCreateBarcodeService.CreateBarcodeByWorkOrderIdAsync(new CreateBarcodeByWorkOrderBo
            {
                SiteId = _currentSite.SiteId ?? 0,
                UserName = _currentUser.UserName,
                WorkOrderId = parm.WorkOrderId,
                Qty = parm.Qty
            }, _localizationService);
        }

        /// <summary>
        /// 工单下达及打印
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task CreateBarcodeByWorkOrderIdAndPrintAsync(CreateBarcodeByWorkOrderAndPrintDto parm)
        {
            var list = await _manuCreateBarcodeService.CreateBarcodeByWorkOrderIdAsync(new CreateBarcodeByWorkOrderBo
            {
                SiteId = _currentSite.SiteId ?? 0,
                UserName = _currentUser.UserName,
                WorkOrderId = parm.WorkOrderId,
                Qty = parm.Qty
            }, _localizationService);

            foreach (var item in list)
            {
                await CreatePrintAsync(new Dtos.Plan.PlanSfcPrintCreatePrintDto()
                {
                    PrintId = parm.PrintId,
                    ProcedureId = parm.ProcedureId,
                    SFC = item.SFC,
                    WorkOrderId = parm.WorkOrderId
                });
            }
        }
    }
}
