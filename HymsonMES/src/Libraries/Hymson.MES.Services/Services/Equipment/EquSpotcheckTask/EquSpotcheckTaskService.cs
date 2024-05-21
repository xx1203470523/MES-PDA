using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Linq;
using System.Net.Mail;
using Hymson.MES.CoreServices.Bos.Quality;
using Hymson.MES.Core.Enums.Equipment;
using Hymson.MES.CoreServices.Bos.Equment;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 服务（点检任务） 
    /// </summary>
    public class EquSpotcheckTaskService : IEquSpotcheckTaskService
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        private readonly ICurrentUser _currentUser;
        /// <summary>
        /// 当前站点
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 参数验证器
        /// </summary>
        private readonly AbstractValidator<EquSpotcheckTaskSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（点检任务）
        /// </summary>
        private readonly IEquSpotcheckTaskRepository _equSpotcheckTaskRepository;

        /// <summary>
        /// 仓储（设备注册）
        /// </summary>
        private readonly IEquEquipmentRepository _equEquipmentRepository;
        /// <summary>
        /// 设备点检任务项目
        /// </summary>
        private readonly IEquSpotcheckTaskItemRepository _equSpotcheckTaskItemRepository;
        /// <summary>
        /// 设备点检快照任务项目
        /// </summary>
        private readonly IEquSpotcheckTaskSnapshotItemRepository _equSpotcheckTaskSnapshotItemRepository;

        /// <summary>
        /// 单位
        /// </summary>
        private readonly IInteUnitRepository _inteUnitRepository;

        /// <summary>
        /// 仓储接口（附件维护）
        /// </summary>
        private readonly IInteAttachmentRepository _inteAttachmentRepository;

        /// <summary>
        /// 设备点检任务项目附件
        /// </summary>
        private readonly IEquSpotcheckTaskItemAttachmentRepository _equSpotcheckTaskItemAttachmentRepository;

        /// <summary>
        /// 设备点检任务操作
        /// </summary>
        private readonly IEquSpotcheckTaskOperationRepository _equSpotcheckTaskOperationRepository;

        /// <summary>
        /// 设备点检任务结果处理
        /// </summary>
        private readonly IEquSpotcheckTaskProcessedRepository _equSpotcheckTaskProcessedRepository;




        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="equSpotcheckTaskRepository"></param>
        public EquSpotcheckTaskService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<EquSpotcheckTaskSaveDto> validationSaveRules,
            IEquSpotcheckTaskRepository equSpotcheckTaskRepository,
            IEquEquipmentRepository equEquipmentRepository,
            IEquSpotcheckTaskItemRepository equSpotcheckTaskItemRepository,
            IEquSpotcheckTaskSnapshotItemRepository equSpotcheckTaskSnapshotItemRepository,
            IInteUnitRepository inteUnitRepository, IInteAttachmentRepository inteAttachmentRepository, 
            IEquSpotcheckTaskItemAttachmentRepository equSpotcheckTaskItemAttachmentRepository, 
            IEquSpotcheckTaskOperationRepository equSpotcheckTaskOperationRepository,
            IEquSpotcheckTaskProcessedRepository equSpotcheckTaskProcessedRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _equSpotcheckTaskRepository = equSpotcheckTaskRepository;
            _equEquipmentRepository = equEquipmentRepository;
            _equSpotcheckTaskItemRepository = equSpotcheckTaskItemRepository;
            _equSpotcheckTaskSnapshotItemRepository = equSpotcheckTaskSnapshotItemRepository;
            _inteUnitRepository = inteUnitRepository;
            _inteAttachmentRepository = inteAttachmentRepository;
            _equSpotcheckTaskItemAttachmentRepository = equSpotcheckTaskItemAttachmentRepository;
            _equSpotcheckTaskOperationRepository= equSpotcheckTaskOperationRepository;
            _equSpotcheckTaskProcessedRepository = equSpotcheckTaskProcessedRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(EquSpotcheckTaskSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<EquSpotcheckTaskEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _equSpotcheckTaskRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(EquSpotcheckTaskSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<EquSpotcheckTaskEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _equSpotcheckTaskRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _equSpotcheckTaskRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _equSpotcheckTaskRepository.DeletesAsync(new DeleteCommand
            {
                Ids = ids,
                DeleteOn = HymsonClock.Now(),
                UserId = _currentUser.UserName
            });
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquSpotcheckTaskDto?> QueryByIdAsync(long id)
        {
            var equSpotcheckTaskEntity = await _equSpotcheckTaskRepository.GeUnionByIdAsync(id);
            if (equSpotcheckTaskEntity == null) return null;

            var result = equSpotcheckTaskEntity.ToModel<EquSpotcheckTaskDto>();

            var equipmenEntity = await _equEquipmentRepository.GetByIdAsync(result.EquipmentId.GetValueOrDefault());

            result.StatusText = result.Status.GetDescription();
            result.IsQualifiedText = result.IsQualified.GetDescription();
            result.EquipmentCode = equipmenEntity.EquipmentCode;
            result.Location = equipmenEntity.Location;

            return result;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSpotcheckTaskDto>> GetPagedListAsync(EquSpotcheckTaskPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquSpotcheckTaskPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;

            // 转换设备编码变为taskid
            if (!string.IsNullOrWhiteSpace(pagedQuery.EquipmentCode))
            {
                var equipmenEntities = await _equEquipmentRepository.GetByCodeAsync(new Data.Repositories.Common.Query.EntityByCodeQuery
                {
                    Site = pagedQuery.SiteId,
                    Code = pagedQuery.EquipmentCode,
                });
                if (equipmenEntities != null) pagedQuery.EquipmentId = equipmenEntities.Id;
                else pagedQuery.EquipmentId = default;
            }

            // 将不合格处理方式转换为点检单ID
            //if (pagedQueryDto.HandMethod.HasValue)
            //{
            //    var unqualifiedHandEntities = await _qualFqcOrderUnqualifiedHandleRepository.GetEntitiesAsync(new QualFqcOrderUnqualifiedHandleQuery
            //    {
            //        SiteId = pagedQuery.SiteId,
            //        HandMethod = pagedQueryDto.HandMethod
            //    });
            //    if (unqualifiedHandEntities != null && unqualifiedHandEntities.Any()) pagedQuery.FQCOrderIds = unqualifiedHandEntities.Select(s => s.FQCOrderId);
            //    else pagedQuery.FQCOrderIds = Array.Empty<long>();
            //}

            var result = new PagedInfo<EquSpotcheckTaskDto>(Enumerable.Empty<EquSpotcheckTaskDto>(), pagedQuery.PageIndex, pagedQuery.PageSize);

            var pagedInfo = await _equSpotcheckTaskRepository.GetPagedListAsync(pagedQuery);

            if (pagedInfo.Data != null && pagedInfo.Data.Any())
            {
                result.Data = pagedInfo.Data.Select(s => s.ToModel<EquSpotcheckTaskDto>());
                result.TotalCount = pagedInfo.TotalCount;

                var resultEquipmentIds = result.Data.Select(m => m.EquipmentId.GetValueOrDefault());
                try
                {
                    var equipmenEntities = await _equEquipmentRepository.GetByIdAsync(resultEquipmentIds);

                    result.Data = result.Data.Select(m =>
                    {
                        var equipmentEntity = equipmenEntities.FirstOrDefault(e => e.Id == m.EquipmentId);
                        if (equipmentEntity != null)
                        {
                            m.EquipmentCode = equipmentEntity.EquipmentCode;
                            m.EquipmentName = equipmentEntity.EquipmentName;
                            m.Location = equipmentEntity.Location;
                        }
                        return m;
                    });
                }
                catch (Exception ex) { }


            }

            return result;
        }

        /// <summary>
        /// 查询点检单明细项数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TaskItemUnionSnapshotView>> querySnapshotItemAsync(SpotcheckTaskSnapshotItemQueryDto requestDto)
        {
            var taskitem = await _equSpotcheckTaskItemRepository.GetEntitiesAsync(new EquSpotcheckTaskItemQuery { SpotCheckTaskId = requestDto.SpotCheckTaskId });
            var spotCheckItemSnapshotIds = taskitem.Select(e => e.SpotCheckItemSnapshotId);
            if (!spotCheckItemSnapshotIds.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }
            var taskitemSnap = await _equSpotcheckTaskSnapshotItemRepository.GetEntitiesAsync(new EquSpotcheckTaskSnapshotItemQuery { Id = spotCheckItemSnapshotIds });
            //单位
            var unitIds = taskitemSnap.Where(x => x.UnitId.HasValue).Select(x => x.UnitId!.Value);
            var inteUnitEntitys = await _inteUnitRepository.GetByIdsAsync(unitIds);
            var unitDic = inteUnitEntitys.ToDictionary(x => x.Id, x => x.Code);

            List<TaskItemUnionSnapshotView> outViews = new();

            //var result = from a in taskitem
            //             join b in taskitemSnap on a.SpotCheckItemSnapshotId equals b.Id
            //             select new TaskItemUnionSnapshotView
            //             {
            //                 Id = a.Id,
            //                 SpotCheckTaskId = a.SpotCheckTaskId,
            //                 SpotCheckItemSnapshotId = b.Id,

            //                 // 映射其他属性
            //                 InspectionValue = a.InspectionValue,
            //                 IsQualified = a.IsQualified,
            //                 Remark = a.Remark,
            //                 SiteId = a.SiteId,

            //                 //snapshot
            //                 Code = b.Code,
            //                 Name = b.Name,
            //                 Status = b.Status,
            //                 DataType = b.DataType,
            //                 CheckType = b.CheckType,
            //                 CheckMethod = b.CheckMethod,
            //                 UnitId = b.UnitId,
            //                 Unit = "",
            //                 OperationContent = b.OperationContent,
            //                 Components = b.Components,
            //                 LowerLimit = b.LowerLimit,
            //                 ReferenceValue = b.ReferenceValue,
            //                 UpperLimit = b.UpperLimit

            //             };

            try
            {
                foreach (var item in taskitem)
                {
                    var snap = taskitemSnap.Where(x => x.Id == item.SpotCheckItemSnapshotId).FirstOrDefault();
                    if (snap != null)
                    {
                        var oneView = snap.ToModel<TaskItemUnionSnapshotView>();
                        oneView = item.ToCombineMap(oneView);
                        //处理单位
                        if (snap.UnitId.HasValue)
                        {
                            unitDic.TryGetValue(snap.UnitId ?? 0, out var unitCode);
                            if (unitCode != null)
                            {
                                oneView.Unit = unitCode;
                            }

                        }
                        outViews.Add(oneView);
                    }

                }
            }
            catch (Exception ex) { }

            return outViews;
        }


        /// <summary>
        /// 保存点检
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> SaveAndUpdateTaskItemAsync(SpotcheckTaskItemSaveDto requestDto)
        {
            var taskItemids = requestDto.Details.Select(x => x.Id);

            var entitys = await _equSpotcheckTaskItemRepository.GetByIdsAsync(taskItemids.ToArray());
            if (!entitys.Any()) return 0;

            var site = entitys.FirstOrDefault()?.SiteId ?? 0;

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // 样本附件
            List<InteAttachmentEntity> attachmentEntities = new();
            List<EquSpotcheckTaskItemAttachmentEntity> sampleDetailAttachmentEntities = new();

            foreach (var entity in entitys)
            {
                var transItem = requestDto.Details.Where(x => x.Id == entity.Id).FirstOrDefault();

                if (transItem == null) continue;

                entity.IsQualified = transItem.IsQualified;
                entity.InspectionValue = transItem.InspectionValue ?? "";
                entity.Remark = transItem.Remark;
                entity.UpdatedBy = updatedBy;
                entity.UpdatedOn = updatedOn;

                var oneDetail = requestDto.Details.Where(x => x.Id == entity.Id).FirstOrDefault();
                if (oneDetail == null) continue;

                var requestAttachments = oneDetail.Attachments;


                if (requestAttachments != null && requestAttachments.Any())
                {
                    foreach (var attachment in requestAttachments)
                    {
                        // 附件
                        var attachmentId = IdGenProvider.Instance.CreateId();
                        attachmentEntities.Add(new InteAttachmentEntity
                        {
                            Id = attachmentId,
                            Name = attachment.Name,
                            Path = attachment.Path,
                            CreatedBy = updatedBy,
                            CreatedOn = updatedOn,
                            UpdatedBy = updatedBy,
                            UpdatedOn = updatedOn,
                            SiteId = entity.SiteId,
                        });

                        // 样本附件
                        sampleDetailAttachmentEntities.Add(new EquSpotcheckTaskItemAttachmentEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = entity.SiteId,
                            SpotCheckTaskId = requestDto.SpotCheckTaskId,
                            SpotCheckTaskItemId = entity.Id,
                            AttachmentId = attachmentId,
                            CreatedBy = updatedBy,
                            CreatedOn = updatedOn,
                            UpdatedBy = updatedBy,
                            UpdatedOn = updatedOn
                        });
                    }
                }
            }


            // 之前的附件
            var beforeAttachments = await _equSpotcheckTaskItemAttachmentRepository.GetEntitiesAsync(new EquSpotcheckTaskItemAttachmentQuery
            {
                SiteId = site,
                SpotCheckTaskId = requestDto.SpotCheckTaskId,
            });

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _equSpotcheckTaskItemRepository.UpdateRangeAsync(entitys);

            // 先删除再添加
            if (beforeAttachments != null && beforeAttachments.Any())
            {
                rows += await _equSpotcheckTaskItemAttachmentRepository.DeletesAsync(new DeleteCommand
                {
                    UserId = updatedBy,
                    DeleteOn = updatedOn,
                    Ids = beforeAttachments.Select(s => s.Id)
                });

                rows += await _inteAttachmentRepository.DeletesAsync(new DeleteCommand
                {
                    UserId = updatedBy,
                    DeleteOn = updatedOn,
                    Ids = beforeAttachments.Select(s => s.AttachmentId)
                });
            }

            if (attachmentEntities.Any())
            {
                rows += await _inteAttachmentRepository.InsertRangeAsync(attachmentEntities);
                rows += await _equSpotcheckTaskItemAttachmentRepository.InsertRangeAsync(sampleDetailAttachmentEntities);
            }
            trans.Complete();
            return rows;
        }


        public async Task<int> CompleteOrderAsync(SpotcheckTaskCompleteDto requestDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // FQC检验单
            var entity = await _equSpotcheckTaskRepository.GetByIdAsync(requestDto.Id)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

            // 只有"检验中"的状态才允许点击"完成"
            if (entity.Status != EquSpotcheckTaskStautusEnum.Inspecting)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11912))
                    .WithData("Before", EquSpotcheckTaskStautusEnum.Inspecting.GetDescription())
                    .WithData("After", EquSpotcheckTaskStautusEnum.Completed.GetDescription());
            }
            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // 检查每种类型是否已经录入足够
            //var sampleEntities = await _equSpotcheckTaskItemRepository.GetEntitiesAsync(new QualFqcOrderSampleQuery
            //{
            //    SiteId = entity.SiteId,
            //    FQCOrderId = entity.Id
            //});

            ////校验已检数量

            //if (sampleEntities.Count() < entity.SampleQty)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES11716)).WithData("CheckedQty", sampleEntities.Count()).WithData("SampleQty", entity.SampleQty);
            //}

            // 读取所有明细参数
            var sampleDetailEntities = await _equSpotcheckTaskItemRepository.GetEntitiesAsync(new EquSpotcheckTaskItemQuery
            {
                SiteId = entity.SiteId,
                SpotCheckTaskId = entity.Id
            });

            var operationType = EquSpotcheckOperationTypeEnum.Complete;

            //检验值是否为空
            if (sampleDetailEntities.Any(x => string.IsNullOrEmpty(x.InspectionValue)))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15902));
            }

            //有任一不合格，完成
            if (sampleDetailEntities.Any(X => X.IsQualified == TrueOrFalseEnum.No))
            {
                entity.Status = EquSpotcheckTaskStautusEnum.Completed;
                entity.IsQualified = TrueOrFalseEnum.No;
                operationType = EquSpotcheckOperationTypeEnum.Complete;
            }
            else
            {
                // 默认是关闭
                entity.Status = EquSpotcheckTaskStautusEnum.Closed;
                operationType = EquSpotcheckOperationTypeEnum.Close;
            }

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await CommonOperationAsync(entity, operationType);
            trans.Complete();
            return rows;
        }

        /// <summary>
        /// 关闭检验单
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> CloseOrderAsync(SpotcheckTaskCloseDto requestDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            //点检单
            var entity = await _equSpotcheckTaskRepository.GetByIdAsync(requestDto.SpotCheckTaskId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

            // 只有"已检验"的状态才允许"关闭"
            if (entity.Status != EquSpotcheckTaskStautusEnum.Completed)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11912))
                    .WithData("Before", InspectionStatusEnum.Completed.GetDescription())
                    .WithData("After", InspectionStatusEnum.Closed.GetDescription());
            }

            // 不合格处理完成之后直接关闭（无需变为合格）
            entity.Status = EquSpotcheckTaskStautusEnum.Closed;

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await CommonOperationAsync(entity, EquSpotcheckOperationTypeEnum.Close, new SpotTaskHandleBo { HandMethod = requestDto.HandMethod, Remark = requestDto.Remark });
            trans.Complete();
            return rows;
        }

        /// <summary>
        /// 通用操作（未加事务）
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="operationType"></param>
        /// <param name="handleBo"></param>
        /// <returns></returns>
        private async Task<int> CommonOperationAsync(EquSpotcheckTaskEntity entity, EquSpotcheckOperationTypeEnum operationType, SpotTaskHandleBo? handleBo = null)
        {
            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // 更新
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;

            var rows = 0;
            rows += await _equSpotcheckTaskRepository.UpdateAsync(entity);
            rows += await _equSpotcheckTaskOperationRepository.InsertAsync(new EquSpotcheckTaskOperationEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = entity.SiteId,
                SpotCheckTaskId = entity.Id,
                OperationType = operationType,
                OperationBy = updatedBy,
                OperationOn = updatedOn,
                CreatedBy = updatedBy,
                CreatedOn = updatedOn,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn
            });

            if (handleBo != null) rows += await _equSpotcheckTaskProcessedRepository.InsertAsync(new EquSpotcheckTaskProcessedEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = entity.SiteId,
                SpotCheckTaskId = entity.Id,
                HandMethod = handleBo.HandMethod,
                Remark = handleBo.Remark ?? "",
                ProcessedBy = updatedBy,
                ProcessedOn = updatedOn,
                CreatedBy = updatedBy,
                CreatedOn = updatedOn,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn
            });
            return rows;
        }

    }
}
