using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcInfo.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.IQualityRepository;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Transactions;

namespace Hymson.MES.Services.Services.Report
{
    /// <summary>
    /// 不良记录报表 服务
    /// </summary>
    public class WorkshopJobControlReportService : IWorkshopJobControlReportService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 表 仓储
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IManuSfcStepRepository _manuSfcStepRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;

        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IProcProcessRouteRepository _procProcessRouteRepository;
        private readonly IProcBomRepository _procBomRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        public WorkshopJobControlReportService(ICurrentUser currentUser, ICurrentSite currentSite, IManuSfcInfoRepository manuSfcInfoRepository, IManuSfcStepRepository manuSfcStepRepository, IProcProcedureRepository procProcedureRepository, IPlanWorkOrderRepository planWorkOrderRepository, IProcMaterialRepository procMaterialRepository, IProcProcessRouteRepository procProcessRouteRepository, IProcBomRepository procBomRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;

            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _procProcedureRepository = procProcedureRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procMaterialRepository = procMaterialRepository;
            _procProcessRouteRepository = procProcessRouteRepository;
            _procBomRepository = procBomRepository;
        }

        /// <summary>
        /// 根据查询条件获取车间作业控制报表分页数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WorkshopJobControlReportViewDto>> GetWorkshopJobControlPageListAsync(WorkshopJobControlReportPagedQueryDto param)
        {
            var pagedQuery = param.ToQuery<WorkshopJobControlReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _manuSfcInfoRepository.GetPagedInfoWorkshopJobControlReportAsync(pagedQuery);

            List<WorkshopJobControlReportViewDto> listDto = new List<WorkshopJobControlReportViewDto>();
            foreach (var item in pagedInfo.Data)
            {
                listDto.Add(item.ToModel<WorkshopJobControlReportViewDto>());
            }

            return new PagedInfo<WorkshopJobControlReportViewDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 获取SFC的车间作业控制步骤
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<WorkshopJobControlStepReportDto> GetSfcInOutInfo(string sfc) 
        {
            var workshopJobControlStepReportDto = new WorkshopJobControlStepReportDto() { SFC=sfc};

            var sfcSteps= await _manuSfcStepRepository.GetSFCInOutStepAsync(sfc);

            if (sfcSteps==null|| !sfcSteps.Any()) 
            {
                throw new BusinessException(nameof(ErrorCode.MES18101)).WithData("sfc", sfc);
            }
            #region 查询基础数据
            var oneSfcStep = sfcSteps.FirstOrDefault();
            //查询工单信息
            var workOrder= await _planWorkOrderRepository.GetByIdAsync(oneSfcStep.WorkOrderId);
            if (workOrder == null) 
            {
                throw new BusinessException(nameof(ErrorCode.MES18102)).WithData("sfc", sfc);
            }
            workshopJobControlStepReportDto.OrderCode=workOrder.OrderCode;

            //查询物料信息
            var material = await _procMaterialRepository.GetByIdAsync(oneSfcStep.ProductId);
            if (material == null)
            {
                throw new BusinessException(nameof(ErrorCode.MES18103)).WithData("sfc", sfc);
            }
            workshopJobControlStepReportDto.MaterialCodrNameVersion = material.MaterialCode + "/" + material.MaterialName + "/" + material.Version;

            //查询工艺路线
            var processRoute = await _procProcessRouteRepository.GetByIdAsync(workOrder.ProcessRouteId);
            if (processRoute == null) 
            {
                throw new BusinessException(nameof(ErrorCode.MES18104)).WithData("sfc", sfc);
            }
            workshopJobControlStepReportDto.ProcessRouteCodeNameVersion = processRoute.Code + "/" + processRoute.Name + "/" + processRoute.Version;

            //查询Bom
            var bom = await _procBomRepository.GetByIdAsync(workOrder.ProductBOMId);
            if (bom == null)
            {
                throw new BusinessException(nameof(ErrorCode.MES18105)).WithData("sfc", sfc);
            }
            workshopJobControlStepReportDto.ProcBomCodeNameVersion = bom.BomCode + "/" + bom.BomName + "/" + bom.Version;

            #endregion


            //入站
            var inSfcSteps = sfcSteps.Where(x => x.Operatetype == Core.Enums.Manufacture.ManuSfcStepTypeEnum.InStock).OrderBy(x=>x.CreatedOn).ToList();

            //出站
            var outSfcSteps = sfcSteps.Where(x => x.Operatetype == Core.Enums.Manufacture.ManuSfcStepTypeEnum.OutStock).ToList();

            
            if (inSfcSteps != null && inSfcSteps.Count > 0)
            {
                //查找工序
                var procedureIds = inSfcSteps.Where(x=>x.ProcedureId!=null).Select(x => x.ProcedureId.Value).Distinct().ToArray();
                var procedures = await _procProcedureRepository.GetByIdsAsync(procedureIds);

                for (int i = 0; i < inSfcSteps.Count; i++)
                {
                    //查找当前出站时间
                    var currentStep = inSfcSteps[i];
                    ManuSfcStepEntity? nextStep = null;
                    ManuSfcStepEntity? outStep = null;

                    var viewDto = new WorkshopJobControlStepReportDto();

                    //是否有下一个进站
                    if (i + 1 < inSfcSteps.Count)
                    {
                        nextStep = inSfcSteps[i + 1];
                        //查找出站时间
                        outStep = outSfcSteps.FirstOrDefault(x => currentStep.CreatedOn < x.CreatedOn && x.CreatedOn < nextStep.CreatedOn);
                    }
                    else 
                    {
                        //查找出站时间
                        outStep = outSfcSteps.FirstOrDefault(x => currentStep.CreatedOn < x.CreatedOn);
                    }

                    workshopJobControlStepReportDto.WorkshopJobControlInOutSteptDtos.Add(new WorkshopJobControlInOutSteptDto() 
                    {
                        ProcedureCode = procedures.FirstOrDefault(x => x.Id == currentStep.ProcedureId)?.Code ?? string.Empty,
                        Status = nextStep != null || outStep!=null ? "完成" : "活动",
                        InDateTime = currentStep.CreatedOn,
                        OutDatetTime = outStep?.CreatedOn
                    });
                }
            }

            return workshopJobControlStepReportDto;
        }

        /// <summary>
        /// 根据SFC分页获取条码步骤信息
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcStepBySFCViewDto>> GetSFCStepsBySFCPageList(ManuSfcStepBySFCPagedQueryDto queryParam)
        {
            var pagedQuery = queryParam.ToQuery<ManuSfcStepBySFCPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId.Value;

            if (string.IsNullOrEmpty(pagedQuery.SFC)) 
            {
                throw new BusinessException(nameof(ErrorCode.MES18110));
            }

            var pagedInfo = await _manuSfcStepRepository.GetPagedInfoBySFCAsync(pagedQuery);

            List<ManuSfcStepBySFCViewDto> listDto = new List<ManuSfcStepBySFCViewDto>();

            if (pagedInfo.Data == null || !pagedInfo.Data.Any()) 
            {
                return new PagedInfo<ManuSfcStepBySFCViewDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
            }

            var materialIds = pagedInfo.Data.Select(x => x.ProductId).Distinct().ToArray();
            var materials = await _procMaterialRepository.GetByIdsAsync(materialIds);

            var procedureIds= pagedInfo.Data.Select(x => x.ProcedureId.Value).Distinct().ToArray();
            var procedures = procedureIds!=null&&procedureIds.Any()? await _procProcedureRepository.GetByIdsAsync(procedureIds):null;

            var workOrderIds= pagedInfo.Data.Select(x => x.WorkOrderId).Distinct().ToArray();
            var workOrders= await _planWorkOrderRepository.GetByIdsAsync(workOrderIds);

            foreach (var item in pagedInfo.Data)
            {
                var material = materials != null && materials.Any() ? materials.Where(x => x.Id == item.ProductId).FirstOrDefault() : null;
                var procedure = procedures != null && procedures.Any() ? procedures.Where(x => x.Id == item.ProcedureId).FirstOrDefault() : null;
                var workOrder=workOrders!=null&&workOrders.Any() ? workOrders.Where(x => x.Id == item.WorkOrderId).FirstOrDefault() : null;


                listDto.Add(new ManuSfcStepBySFCViewDto()
                {
                    Id = item.Id,
                    SFC = item.SFC,
                    Operatetype = item.Operatetype,
                    CreatedOn = item.CreatedOn,
                    MaterialCodeVersion = material != null ? material.MaterialCode + "/" + material.Version : "",
                    MaterialName = material != null ? material.MaterialName : "",
                    ProcedureCode = procedure != null ? procedure.Code : "",
                    ProcedureName = procedure != null ? procedure.Name : "",
                    OrderCode= workOrder != null ? workOrder.OrderCode : ""
                });
            }

            return new PagedInfo<ManuSfcStepBySFCViewDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }
    }
}