/*
 *creator: Karl
 *
 *describe: 工单激活    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-29 10:23:51
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Integrated.InteWorkCenter;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Transactions;

namespace Hymson.MES.Services.Services.Plan
{
    /// <summary>
    /// 工单激活 服务
    /// </summary>
    public class PlanWorkOrderActivationService : IPlanWorkOrderActivationService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 工单激活 仓储
        /// </summary>
        private readonly IPlanWorkOrderActivationRepository _planWorkOrderActivationRepository;
        private readonly AbstractValidator<PlanWorkOrderActivationCreateDto> _validationCreateRules;
        private readonly AbstractValidator<PlanWorkOrderActivationModifyDto> _validationModifyRules;

        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IPlanWorkOrderActivationRecordRepository _planWorkOrderActivationRecordRepository;
        private readonly IPlanWorkOrderStatusRecordRepository _planWorkOrderStatusRecordRepository;

        public PlanWorkOrderActivationService(ICurrentUser currentUser, ICurrentSite currentSite, IPlanWorkOrderActivationRepository planWorkOrderActivationRepository, AbstractValidator<PlanWorkOrderActivationCreateDto> validationCreateRules, AbstractValidator<PlanWorkOrderActivationModifyDto> validationModifyRules, IInteWorkCenterRepository inteWorkCenterRepository, IPlanWorkOrderRepository planWorkOrderRepository, IPlanWorkOrderActivationRecordRepository planWorkOrderActivationRecordRepository, IPlanWorkOrderStatusRecordRepository planWorkOrderStatusRecordRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _planWorkOrderActivationRepository = planWorkOrderActivationRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;

            _inteWorkCenterRepository = inteWorkCenterRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _planWorkOrderActivationRecordRepository = planWorkOrderActivationRecordRepository;
            _planWorkOrderStatusRecordRepository = planWorkOrderStatusRecordRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="planWorkOrderActivationCreateDto"></param>
        /// <returns></returns>
        public async Task CreatePlanWorkOrderActivationAsync(PlanWorkOrderActivationCreateDto planWorkOrderActivationCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(planWorkOrderActivationCreateDto);

            //DTO转换实体
            var planWorkOrderActivationEntity = planWorkOrderActivationCreateDto.ToEntity<PlanWorkOrderActivationEntity>();
            planWorkOrderActivationEntity.Id = IdGenProvider.Instance.CreateId();
            planWorkOrderActivationEntity.CreatedBy = _currentUser.UserName;
            planWorkOrderActivationEntity.UpdatedBy = _currentUser.UserName;
            planWorkOrderActivationEntity.CreatedOn = HymsonClock.Now();
            planWorkOrderActivationEntity.UpdatedOn = HymsonClock.Now();
            planWorkOrderActivationEntity.SiteId = _currentSite.SiteId ?? 0;

            //入库
            await _planWorkOrderActivationRepository.InsertAsync(planWorkOrderActivationEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeletePlanWorkOrderActivationAsync(long id)
        {
            await _planWorkOrderActivationRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesPlanWorkOrderActivationAsync(long[] idsArr)
        {
            return await _planWorkOrderActivationRepository.DeletesAsync(new DeleteCommand { Ids = idsArr, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="planWorkOrderActivationPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanWorkOrderActivationListDetailViewDto>> GetPageListAsync(PlanWorkOrderActivationPagedQueryDto planWorkOrderActivationPagedQueryDto)
        {
            if (!planWorkOrderActivationPagedQueryDto.LineId.HasValue)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16401));
            }

            //查询当前线体
            var line = await _inteWorkCenterRepository.GetByIdAsync(planWorkOrderActivationPagedQueryDto.LineId.Value);
            if (line == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16402));
            }
            if (line.Type != WorkCenterTypeEnum.Line)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16403));
            }

            //查询线体上级车间
            var workCenter = await _inteWorkCenterRepository.GetHigherInteWorkCenterAsync(planWorkOrderActivationPagedQueryDto.LineId ?? 0);

            var planWorkOrderActivationPagedQuery = planWorkOrderActivationPagedQueryDto.ToQuery<PlanWorkOrderActivationPagedQuery>();
            planWorkOrderActivationPagedQuery.SiteId = _currentSite.SiteId!.Value;

            //将对应的工作中心ID放置查询条件中
            planWorkOrderActivationPagedQuery.WorkCenterIds.Add(planWorkOrderActivationPagedQueryDto.LineId ?? 0);
            if (workCenter != null && workCenter.Id > 0)
            {
                planWorkOrderActivationPagedQuery.WorkCenterIds.Add(workCenter.Id);
            }

            var pagedInfo = await _planWorkOrderActivationRepository.GetPagedInfoAsync(planWorkOrderActivationPagedQuery);

            //实体到DTO转换 装载数据
            List<PlanWorkOrderActivationListDetailViewDto> planWorkOrderActivationDtos = PreparePlanWorkOrderActivationDtos(pagedInfo);
            return new PagedInfo<PlanWorkOrderActivationListDetailViewDto>(planWorkOrderActivationDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据查询条件获取分页数据--(根据资源先找到线体)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanWorkOrderActivationListDetailViewDto>> GetPageListAboutResAsync(PlanWorkOrderActivationAboutResPagedQueryDto param)
        {
            if (!param.ResourceId.HasValue)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16412));
            }

            //检查当前资源对应的线体
            var workCenterEntity = await _inteWorkCenterRepository.GetByResourceIdAsync(param.ResourceId.Value);
            if (workCenterEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16413));
            }

            if (workCenterEntity.Type != WorkCenterTypeEnum.Line)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16414));
            }

            var planWorkOrderActivationPagedQuery = param.ToQuery<PlanWorkOrderActivationPagedQuery>();
            planWorkOrderActivationPagedQuery.SiteId = _currentSite.SiteId!.Value;

            //将对应的工作中心ID放置查询条件中
            planWorkOrderActivationPagedQuery.WorkCenterIds.Add(workCenterEntity.Id);

            var pagedInfo = await _planWorkOrderActivationRepository.GetPagedInfoAsync(planWorkOrderActivationPagedQuery);

            //实体到DTO转换 装载数据
            List<PlanWorkOrderActivationListDetailViewDto> planWorkOrderActivationDtos = PreparePlanWorkOrderActivationDtos(pagedInfo);
            return new PagedInfo<PlanWorkOrderActivationListDetailViewDto>(planWorkOrderActivationDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<PlanWorkOrderActivationListDetailViewDto> PreparePlanWorkOrderActivationDtos(PagedInfo<PlanWorkOrderActivationListDetailView> pagedInfo)
        {
            var planWorkOrderActivationDtos = new List<PlanWorkOrderActivationListDetailViewDto>();
            foreach (var planWorkOrderActivation in pagedInfo.Data)
            {
                var planWorkOrderActivationDto = planWorkOrderActivation.ToModel<PlanWorkOrderActivationListDetailViewDto>();
                planWorkOrderActivationDtos.Add(planWorkOrderActivationDto);
            }

            return planWorkOrderActivationDtos;
        }

        /// <summary>
        /// 激活/取消激活 工单
        /// </summary>
        /// <param name="activationWorkOrderDto"></param>
        /// <returns></returns>
        public async Task ActivationWorkOrderAsync(ActivationWorkOrderDto activationWorkOrderDto)
        {
            //查询当前线体
            var line = await _inteWorkCenterRepository.GetByIdAsync(activationWorkOrderDto.LineId);
            if (line == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16402));
            }
            if (line.Type != WorkCenterTypeEnum.Line)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16403));
            }

            //查询当前工单信息
            var workOrder = await _planWorkOrderRepository.GetByIdAsync(activationWorkOrderDto.Id);
            if (workOrder == null || workOrder.Id <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16404));
            }

            //查询是否被暂停
            if (workOrder.Status == PlanWorkOrderStatusEnum.Pending) 
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16415)).WithData("orderCode", workOrder.OrderCode);
            }

            //查询当前工单是否已经被激活
            var workOrderActivation = (await _planWorkOrderActivationRepository.GetPlanWorkOrderActivationEntitiesAsync(new PlanWorkOrderActivationQuery()
            {
                WorkOrderId = workOrder.Id,
                SiteId = _currentSite.SiteId ?? 0
            })).FirstOrDefault();

            var isActivationed = workOrderActivation != null;//是否已经激活
            if (isActivationed && isActivationed == activationWorkOrderDto.IsNeedActivation)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16406)).WithData("orderCode", workOrder.OrderCode);
            }
            else if (!isActivationed && isActivationed == activationWorkOrderDto.IsNeedActivation)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16407)).WithData("orderCode", workOrder.OrderCode);
            }

            //取消激活
            if (!activationWorkOrderDto.IsNeedActivation)
            {
                using (TransactionScope ts = TransactionHelper.GetTransactionScope())
                {
                    await _planWorkOrderActivationRepository.DeleteTrueAsync(workOrderActivation!.Id);//真删除

                    //记录下激活状态变化
                    await _planWorkOrderActivationRecordRepository.InsertAsync(new PlanWorkOrderActivationRecordEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = _currentSite.SiteId ?? 0,

                        WorkOrderId = activationWorkOrderDto.Id,
                        LineId = activationWorkOrderDto.LineId,

                        ActivateType = PlanWorkOrderActivateTypeEnum.CancelActivate
                    });

                    ts.Complete();
                }
                return;
            }

            if (line.IsMixLine!.Value)
            {//混线
                await DoActivationWorkOrderAsync(workOrder, activationWorkOrderDto);
            }
            else
            {//不混线
                //判断当前线体是否有无激活的工单
                var hasActivation = (await _planWorkOrderActivationRepository.GetPlanWorkOrderActivationEntitiesAsync(new PlanWorkOrderActivationQuery { LineId = activationWorkOrderDto.LineId, SiteId = _currentSite.SiteId ?? 0 })).FirstOrDefault();
                if (hasActivation != null)
                {
                    var activationWorkOrder = await _planWorkOrderRepository.GetByIdAsync(hasActivation.WorkOrderId);
                    throw new CustomerValidationException(nameof(ErrorCode.MES16409)).WithData("orderCode", activationWorkOrder.OrderCode);
                }

                await DoActivationWorkOrderAsync(workOrder, activationWorkOrderDto);
            }
        }

        /// <summary>
        /// 激活工单
        /// </summary>
        /// <param name="workOrder"></param>
        /// <param name="activationWorkOrderDto"></param>
        /// <returns></returns>
        private async Task DoActivationWorkOrderAsync(PlanWorkOrderEntity workOrder, ActivationWorkOrderDto activationWorkOrderDto)
        {
            if (workOrder.Status == PlanWorkOrderStatusEnum.Pending)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16408)).WithData("orderCode", workOrder.OrderCode);
            }

            var planWorkOrderActivationEntity = new PlanWorkOrderActivationEntity()
            {
                Id = IdGenProvider.Instance.CreateId(),
                CreatedBy = _currentUser.UserName,
                UpdatedBy = _currentUser.UserName,
                CreatedOn = HymsonClock.Now(),
                UpdatedOn = HymsonClock.Now(),
                SiteId = _currentSite.SiteId ?? 0,

                WorkOrderId = activationWorkOrderDto.Id,
                LineId = activationWorkOrderDto.LineId
            };

            //组装工单状态变化记录
            var record = AutoMapperConfiguration.Mapper.Map<PlanWorkOrderStatusRecordEntity>(workOrder);
            record.Id = IdGenProvider.Instance.CreateId();
            record.CreatedBy = _currentUser.UserName;
            record.UpdatedBy = _currentUser.UserName;
            record.CreatedOn = HymsonClock.Now();
            record.UpdatedOn = HymsonClock.Now();
            record.SiteId = _currentSite.SiteId ?? 0;
            record.IsDeleted = 0;

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                switch (workOrder.Status)
                {
                    case Core.Enums.PlanWorkOrderStatusEnum.NotStarted:
                        throw new CustomerValidationException(nameof(ErrorCode.MES16405)).WithData("orderCode", workOrder.OrderCode);
                    case Core.Enums.PlanWorkOrderStatusEnum.SendDown:
                        await _planWorkOrderActivationRepository.InsertAsync(planWorkOrderActivationEntity);

                        //记录下激活状态变化
                        await _planWorkOrderActivationRecordRepository.InsertAsync(new PlanWorkOrderActivationRecordEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            CreatedBy = _currentUser.UserName,
                            UpdatedBy = _currentUser.UserName,
                            CreatedOn = HymsonClock.Now(),
                            UpdatedOn = HymsonClock.Now(),
                            SiteId = _currentSite.SiteId ?? 0,

                            WorkOrderId = activationWorkOrderDto.Id,
                            LineId = activationWorkOrderDto.LineId,

                            ActivateType = PlanWorkOrderActivateTypeEnum.Activate
                        });


                        //修改工单状态为生产中
                        List<UpdateStatusCommand> updateStatusCommands = new List<UpdateStatusCommand>();
                        updateStatusCommands.Add(new UpdateStatusCommand()
                        {
                            Id = activationWorkOrderDto.Id,
                            Status = Core.Enums.PlanWorkOrderStatusEnum.InProduction,
                            BeforeStatus= workOrder.Status,
                            UpdatedBy = _currentUser.UserName,
                            UpdatedOn = HymsonClock.Now()
                        });
                        await _planWorkOrderRepository.ModifyWorkOrderStatusAsync(updateStatusCommands);

                        //TODO  修改工单状态还需要在 工单记录表中记录
                        await _planWorkOrderStatusRecordRepository.InsertAsync(record);

                        break;
                    case Core.Enums.PlanWorkOrderStatusEnum.InProduction:
                    case Core.Enums.PlanWorkOrderStatusEnum.Finish:
                        await _planWorkOrderActivationRepository.InsertAsync(planWorkOrderActivationEntity);

                        //记录下激活状态变化
                        await _planWorkOrderActivationRecordRepository.InsertAsync(new PlanWorkOrderActivationRecordEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            CreatedBy = _currentUser.UserName,
                            UpdatedBy = _currentUser.UserName,
                            CreatedOn = HymsonClock.Now(),
                            UpdatedOn = HymsonClock.Now(),
                            SiteId = _currentSite.SiteId ?? 0,

                            WorkOrderId = activationWorkOrderDto.Id,
                            LineId = activationWorkOrderDto.LineId,

                            ActivateType = PlanWorkOrderActivateTypeEnum.Activate
                        });

                        break;

                    default:
                        break;
                }

                ts.Complete();
            }


        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="planWorkOrderActivationModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyPlanWorkOrderActivationAsync(PlanWorkOrderActivationModifyDto planWorkOrderActivationModifyDto)
        {
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(planWorkOrderActivationModifyDto);

            //DTO转换实体
            var planWorkOrderActivationEntity = planWorkOrderActivationModifyDto.ToEntity<PlanWorkOrderActivationEntity>();
            planWorkOrderActivationEntity.UpdatedBy = _currentUser.UserName;
            planWorkOrderActivationEntity.UpdatedOn = HymsonClock.Now();

            await _planWorkOrderActivationRepository.UpdateAsync(planWorkOrderActivationEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PlanWorkOrderActivationDto> QueryPlanWorkOrderActivationByIdAsync(long id)
        {
            var planWorkOrderActivationEntity = await _planWorkOrderActivationRepository.GetByIdAsync(id);
            if (planWorkOrderActivationEntity != null)
            {
                return planWorkOrderActivationEntity.ToModel<PlanWorkOrderActivationDto>();
            }
            return new PlanWorkOrderActivationDto();
        }
    }
}
