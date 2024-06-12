using Dapper;
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Process;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcInfo.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.ProcessRoute.Query;
using Hymson.MES.Services.Dtos.Report;
using Minio.DataModel;

namespace Hymson.MES.Services.Services.Report
{
    /// <summary>
    /// 工单报告 服务
    /// </summary>
    public class WorkOrderStepControlService : IWorkOrderStepControlService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 表 仓储
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IManuSfcStepRepository _manuSfcStepRepository;
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
        private readonly IManuSfcSummaryRepository _manuSfcSummaryRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteDetailNodeRepository;

        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IProcProcessRouteRepository _procProcessRouteRepository;
        private readonly IProcBomRepository _procBomRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuSfcSummaryRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procProcessRouteRepository"></param>
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        /// <param name="procBomRepository"></param>

        public WorkOrderStepControlService(ICurrentUser currentUser, ICurrentSite currentSite, IManuSfcInfoRepository manuSfcInfoRepository, IManuSfcStepRepository manuSfcStepRepository, IProcProcedureRepository procProcedureRepository, IPlanWorkOrderRepository planWorkOrderRepository, IProcMaterialRepository procMaterialRepository, IProcProcessRouteRepository procProcessRouteRepository, IProcBomRepository procBomRepository, IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository, IManuSfcProduceRepository manuSfcProduceRepository, IManuSfcSummaryRepository manuSfcSummaryRepository)
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
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcSummaryRepository = manuSfcSummaryRepository;
        }

        /// <summary>
        /// 根据查询条件获取工单步骤控制报表分页数据
        /// 优化: 不模糊查询，且通过关联ID查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WorkOrderStepControlViewDto>> GetWorkOrderStepControlPageListAsync(WorkOrderStepControlOptimizePagedQueryDto param)
        {
            var pagedQuery = param.ToQuery<PlanWorkOrderPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            // 判断是否有获取到站点码 
            if (param.OrderCode == null)
            {
                return null;
            }

            var pagedInfo = await _planWorkOrderRepository.GetPagedInfoAsync(pagedQuery);

            List<WorkOrderStepControlViewDto> listDto = new();
            if (pagedInfo.Data.Any())
            {
                // 查询物料
                var materialsTask = _procMaterialRepository.GetByIdsAsync(pagedInfo.Data.Select(x => x.ProductId));
                // 查询工序节点明细
                var procProcessRouteDetailNodeTask = _procProcessRouteDetailNodeRepository.GetProcessRouteDetailNodesByProcessRouteIdAsync(pagedInfo.Data.First().ProcessRouteId ?? 0);
                //查询工序
                var procProceduresTask = _procProcedureRepository.GetByIdsAsync(procProcessRouteDetailNodeTask.Result.Select(x => x.ProcedureId));

                var materials = await materialsTask;
                var procProcessRouteDetailNode = await procProcessRouteDetailNodeTask;
                var procProcedures = await procProceduresTask;

                foreach (var item in procProcessRouteDetailNode.ToList())
                {
                    var material = materials.FirstOrDefault(x => x.Id == pagedInfo.Data.First().ProductId);
                    var PassDownQuantityquery = new ManuSfcProduceVehiclePagedQuery()
                    {
                        WorkOrderId = pagedInfo.Data.First().Id,
                        ProductId = pagedInfo.Data.First().ProductId,
                        ProcessRouteId = pagedInfo.Data.First().ProcessRouteId,
                        ProcedureId = item.ProcedureId,
                        Status = (SfcStatusEnum)1
                    };
                    var PassDownQuantity = _manuSfcProduceRepository.GetStepPageListAsync(PassDownQuantityquery).Result.Data.Count();

                    var ProcessDownQuantityquery = new ManuSfcProduceVehiclePagedQuery()
                    {
                        WorkOrderId = pagedInfo.Data.First().Id,
                        ProductId = pagedInfo.Data.First().ProductId,
                        ProcessRouteId = pagedInfo.Data.First().ProcessRouteId,
                        ProcedureId = item.ProcedureId,
                        Status = (SfcStatusEnum)2
                    };
                    var ProcessDownQuantity = _manuSfcProduceRepository.GetStepPageListAsync(PassDownQuantityquery).Result.Data.Count();

                    var FinishProductQuantityquery = new ManuSfcProduceVehiclePagedQuery()
                    {
                        WorkOrderId = pagedInfo.Data.First().Id,
                        ProductId = pagedInfo.Data.First().ProductId,
                        ProcedureId = item.ProcedureId,
                        SiteId = _currentSite.SiteId
                    };
                    var summaryResult = await _manuSfcSummaryRepository.GetWorkOrderAsync(FinishProductQuantityquery);
                    var FinishProductQuantity = summaryResult.FirstOrDefault()?.OutputQty;

                    listDto.Add(new WorkOrderStepControlViewDto
                    {
                        Serialno = item.SerialNo,
                        ProcedureCode = procProcedures.FirstOrDefault(x => x.Id == item.ProcedureId)?.Code,
                        MaterialCode = material != null ? material.MaterialCode + "/" + material.Version : "",
                        ProcessRout = pagedInfo.Data.First().ProcessRouteCode + "/" + pagedInfo.Data.First().ProcessRouteVersion,
                        OrderCode = pagedInfo.Data.First()?.OrderCode ?? "",
                        PassDownQuantity = PassDownQuantity,
                        ProcessDownQuantity = PassDownQuantity,
                        FinishProductQuantity = FinishProductQuantity ?? 0,
                    });
                }
            }
            return new PagedInfo<WorkOrderStepControlViewDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }
    }
}