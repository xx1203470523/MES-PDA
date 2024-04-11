using AutoMapper.Execution;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Qual;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Services;
using Hymson.MES.Services.Dtos.Qual;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using OfficeOpenXml;
using System.Reflection;
using System.Security.Policy;
using System.Text;

namespace Hymson.MES.Services.Services.Quality
{
    /// <summary>
    /// 服务（OQC检验参数组） 
    /// </summary>
    public class QualFqcParameterGroupService : IQualFqcParameterGroupService
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
        private readonly AbstractValidator<QualFqcParameterGroupDto> _validationSaveRules;

        /// <summary>
        /// 更新验证器
        /// </summary>
        private readonly AbstractValidator<QualFqcParameterGroupUpdateDto> _validationUpdateRules;

        /// <summary>
        /// 删除验证器
        /// </summary>
        private readonly AbstractValidator<QualFqcParameterGroupDeleteDto> _validationDeleteRules;

        /// <summary>
        /// 仓储接口（OQC检验参数组）
        /// </summary>
        private readonly IQualFqcParameterGroupRepository _qualFqcParameterGroupRepository;
        private readonly IQualFqcParameterGroupDetailRepository _qualFqcParameterGroupDetailRepository;


        private readonly IProcMaterialRepository _procMaterialRepository;

        private readonly IInteCustomRepository _inteCustomRepository;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="validationUpdateRules"></param>
        /// <param name="validationDeleteRules"></param>
        /// <param name="qualFqcParameterGroupRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="inteCustomRepository"></param>
        /// <param name="qualFqcParameterGroupDetailRepository"></param>
        public QualFqcParameterGroupService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<QualFqcParameterGroupDto> validationSaveRules,
            AbstractValidator<QualFqcParameterGroupUpdateDto> validationUpdateRules,
            AbstractValidator<QualFqcParameterGroupDeleteDto> validationDeleteRules,
            IQualFqcParameterGroupRepository qualFqcParameterGroupRepository,
            IProcMaterialRepository procMaterialRepository, IInteCustomRepository inteCustomRepository,
            IQualFqcParameterGroupDetailRepository qualFqcParameterGroupDetailRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _validationUpdateRules = validationUpdateRules;
            _validationDeleteRules = validationDeleteRules;
            _qualFqcParameterGroupRepository = qualFqcParameterGroupRepository;
            _procMaterialRepository = procMaterialRepository;
            _inteCustomRepository = inteCustomRepository;
            _qualFqcParameterGroupDetailRepository = qualFqcParameterGroupDetailRepository;
        }

        ///// <summary>
        ///// 获取单个
        ///// </summary>
        ///// <param name="queryDto"></param>
        ///// <returns></returns>
        ///// <exception cref="CustomerValidationException"></exception>
        //public async Task<QualFqcParameterGroupOutputDto> GetOneAsync(QualFqcParameterGroupQueryDto queryDto)
        //{
        //    var query = queryDto.ToQuery<QualFqcParameterGroupToQuery>();
        //    query.SiteId = _currentSite.SiteId;

        //    var qualIqcInspectionItemEntity = await _qualFqcParameterGroupRepository.GetOneAsync(query);
        //    if (qualIqcInspectionItemEntity == null)
        //    {
        //        throw new CustomerValidationException(nameof(ErrorCode.MES10104));
        //    }

        //    var result = qualIqcInspectionItemEntity.ToModel<QualOqcParameterGroupOutputDto>();
        //    if (result == null)
        //    {
        //        throw new CustomerValidationException(nameof(ErrorCode.MES10104));
        //    }

        //    var materialEntity = await _procMaterialRepository.GetByIdAsync(result.MaterialId.GetValueOrDefault());
        //    if (materialEntity != null)
        //    {
        //        result.MaterialName = materialEntity.MaterialName;
        //        result.MaterialCode = materialEntity.MaterialCode;
        //        result.MaterialUnit = materialEntity.Unit;
        //        result.MaterialVersion = materialEntity.Version;
        //    }

        //    var customerEntity = await _inteCustomRepository.GetByIdAsync(result.CustomerId.GetValueOrDefault());
        //    if (customerEntity != null)
        //    {
        //        result.CustomerCode = customerEntity.Code;
        //        result.CustomerName = customerEntity.Name;
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// 创建
        ///// </summary>
        ///// <param name="createDto"></param>
        ///// <returns></returns>
        //public async Task CreateAsync(QualFqcParameterGroupDto createDto)
        //{
        //    await _validationSaveRules.ValidateAndThrowAsync(createDto);

        //    var command = createDto.ToCommand<QualFqcParameterGroupEntity>();
        //    command.Init();
        //    command.CreatedBy = _currentUser.UserName;
        //    command.UpdatedBy = _currentUser.UserName;
        //    command.SiteId = _currentSite.SiteId;

        //    if (command.Code != null || command.Name != null)
        //    {
        //        var projectCode = new QualFqcParameterGroupQuery
        //        {
        //            SiteId = command.SiteId,
        //            MaterialId = command.MaterialId,
        //        };
        //        //校验项目编码
        //        var qualIqcInspectionItemEntity = await _qualFqcParameterGroupRepository.GetEntitiesAsync(projectCode);
        //        if (qualIqcInspectionItemEntity != null)
        //        {
        //            throw new CustomerValidationException(nameof(ErrorCode.MES19955));
        //        }
        //    }

        //    if (command.MaterialId != null && command.CustomerId != null && command.Version != null)
        //    {
        //        var projectCode = new QualFqcParameterGroupQuery
        //        {
        //            MaterialId = command.MaterialId,
        //        };
        //        //相同的客户 + 物料 + 检验项目版本
        //        var Entity = await _qualFqcParameterGroupRepository.GetEntitiesAsync(projectCode);
        //        if (Entity != null)
        //        {
        //            var materialEntity = await _procMaterialRepository.GetByIdAsync(Entity.MaterialId);
        //            var customerEntity = await _inteCustomRepository.GetByIdAsync(Entity.CustomerId);
        //            throw new CustomerValidationException(nameof(ErrorCode.MES19956)).WithData("materialCode", materialEntity.MaterialCode).WithData("customCode", customerEntity.Code).WithData("version", command.Version);
        //        }
        //    }

        //    var detailCommands = Enumerable.Empty<QualOqcParameterGroupDetailCreateCommand>();
        //    if (createDto.qualFqcParameterGroupDetailDtos != null && createDto.qualFqcParameterGroupDetailDtos.Any())
        //    {
        //        detailCommands = createDto.qualFqcParameterGroupDetailDtos.Select(m =>
        //        {
        //            var detailCommand = m.ToCommand<QualfqcParameterGroupDetailCreateCommand>();
        //            detailCommand.Init();
        //            detailCommand.ParameterGroupId = command.Id;
        //            detailCommand.CreatedBy = command.CreatedBy;
        //            detailCommand.UpdatedBy = command.UpdatedBy;
        //            detailCommand.SiteId = command.SiteId;

        //            return detailCommand;
        //        });
        //    }


        //    using var scope = TransactionHelper.GetTransactionScope();

        //    var affectedRow = await _qualFqcParameterGroupRepository.InsertAsync(command);
        //    if (affectedRow == 0)
        //    {
        //        throw new CustomerValidationException(nameof(ErrorCode.MES19950));
        //    }

        //    await _qualFqcParameterGroupDetailRepository.InsertAsync(detailCommands);

        //    scope.Complete();


        //}

        ///// <summary>
        ///// 修改
        ///// </summary>
        ///// <param name="updateDto"></param>
        ///// <returns></returns>
        //public async Task ModifyAsync(QualFqcParameterGroupUpdateDto updateDto)
        //{
        //    await _validationUpdateRules.ValidateAndThrowAsync(updateDto);

        //    var entity = await _qualFqcParameterGroupRepository.GetEntitiesAsync(new QualFqcParameterGroupToQuery { Id = updateDto.Id });
        //    if (entity == null)
        //    {
        //        throw new CustomerValidationException(nameof(ErrorCode.MES19954));
        //    }

        //    var command = updateDto.ToCommand<QualOqcParameterGroupUpdateCommand>();
        //    command.Init();
        //    command.SiteId = _currentSite.SiteId;
        //    command.UpdatedBy = _currentUser.UserName;

        //    var detailCommands = Enumerable.Empty<QualOqcParameterGroupDetailCreateCommand>();
        //    if (updateDto.qualFqcParameterGroupDetailDtos != null && updateDto.qualFqcParameterGroupDetailDtos.Any())
        //    {
        //        detailCommands = updateDto.qualFqcParameterGroupDetailDtos.Select(m =>
        //        {
        //            var detailCommand = m.ToCommand<QualOqcParameterGroupDetailCreateCommand>();
        //            detailCommand.Init();
        //            detailCommand.ParameterGroupId = command.Id;
        //            detailCommand.CreatedOn = entity.CreatedOn;
        //            detailCommand.CreatedBy = entity.CreatedBy;
        //            detailCommand.UpdatedBy = command.UpdatedBy;
        //            detailCommand.UpdatedOn = command.UpdatedOn;
        //            detailCommand.SiteId = command.SiteId;

        //            return detailCommand;
        //        });
        //    }

        //    using var scope = TransactionHelper.GetTransactionScope();

        //    await _qualFqcParameterGroupDetailRepository.DeletesAsync(entity.Id);

        //    await _qualFqcParameterGroupRepository.UpdateAsync(command);

        //    await _qualFqcParameterGroupDetailRepository.InsertAsync(detailCommands);

        //    scope.Complete();

        //}

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _qualFqcParameterGroupRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _qualFqcParameterGroupRepository.DeletesAsync(new DeleteCommand
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
        public async Task<QualFqcParameterGroupDto?> QueryByIdAsync(long id)
        {
            var qualOqcParameterGroupEntity = await _qualFqcParameterGroupRepository.GetByIdAsync(id);
            if (qualOqcParameterGroupEntity == null) return null;

            return qualOqcParameterGroupEntity.ToModel<QualFqcParameterGroupDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualFqcParameterGroupDto>> GetPagedListAsync(QualFqcParameterGroupPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<QualFqcParameterGroupPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _qualFqcParameterGroupRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<QualFqcParameterGroupDto>());
            return new PagedInfo<QualFqcParameterGroupDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 分页查询-包含参数明细
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualFqcParameterGroupOutputDto>> GetPagedAsync(QualFqcParameterGroupPagedQueryDto queryDto)
        {
            var _siteId = _currentSite.SiteId;
            List<long> emptyDefault = new List<long> { 0 };

            var materialIds = Enumerable.Empty<long>();
            if (!string.IsNullOrWhiteSpace(queryDto.MaterialCode))
            {
                var materialEntities = await _procMaterialRepository.GetProcMaterialEntitiesAsync(new ProcMaterialQuery { SiteId = _siteId, MaterialCode = queryDto.MaterialCode });
                materialIds = materialEntities.Select(m => m.Id);
                if (!materialIds.Any())
                {
                    materialIds = emptyDefault;
                }
            }
            if (!string.IsNullOrWhiteSpace(queryDto.MaterialName))
            {
                var materialEntities = await _procMaterialRepository.GetProcMaterialEntitiesAsync(new ProcMaterialQuery { SiteId = _siteId, MaterialName = queryDto.MaterialName });
                materialIds = materialIds.Concat(materialEntities.Select(m => m.Id));
                if (!materialIds.Any())
                {
                    materialIds = emptyDefault;
                }
            }
            materialIds = materialIds.Distinct();

            var query = new QualFqcParameterGroupPagedQuery
            {
                PageIndex = queryDto.PageIndex,
                PageSize = queryDto.PageSize,
                SiteId = _siteId
            };
            query.Sorting = "Id Desc";

            var result = new PagedInfo<QualFqcParameterGroupOutputDto>(Enumerable.Empty<QualFqcParameterGroupOutputDto>(), query.PageIndex, query.PageSize);

            var pageResult = await _qualFqcParameterGroupRepository.GetPagedListAsync(query);
            if (pageResult.Data != null && pageResult.Data.Any())
            {
                result.Data = pageResult.Data.Select(m => m.ToModel<QualFqcParameterGroupOutputDto>());
                result.TotalCount = pageResult.TotalCount;

                var resultMaterialIds = result.Data.Select(m => m.MaterialId);

                try
                {
                    var materialEntities = await _procMaterialRepository.GetByIdsAsync(resultMaterialIds);
                    result.Data = result.Data.Select(m =>
                    {
                        var materialEntity = materialEntities.FirstOrDefault(e => e.Id == m.MaterialId);
                        if (materialEntity != default)
                        {
                            m.MaterialCode = materialEntity.MaterialCode;
                            m.MaterialName = materialEntity.MaterialName;
                            m.MaterialUnit = materialEntity.Unit;
                            m.MaterialVersion = materialEntity.Version;
                        }
                        return m;
                    });

                }
                catch (Exception ex) { }
            }

            return result;
        }


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="deleteDto"></param>
        /// <returns></returns>
        public async Task DeleteAsync(QualFqcParameterGroupDeleteDto deleteDto)
        {
            if (!deleteDto.Ids.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10213));

            var entities = await _qualFqcParameterGroupRepository.GetByIdsAsync(deleteDto.Ids);

            using var scope = TransactionHelper.GetTransactionScope();
            var command = new DeleteCommand
            {
                Ids = deleteDto.Ids,
                DeleteOn = HymsonClock.Now(),
                UserId = _currentUser.UserName
            };
            await _qualFqcParameterGroupRepository.DeletesAsync(command);

            scope.Complete();
        }

        public async Task<QualFqcParameterGroupOutputDto> GetOneAsync(QualFqcParameterGroupQueryDto queryDto)
        {
            var query = _qualFqcParameterGroupRepository.GetByIdAsync(queryDto.Id);
            var queryentity = new QualFqcParameterGroupQuery()
            {
                SiteId = query.Result.SiteId,
                MaterialId =query.Result.MaterialId,
                Status = query.Result.Status,
            };
            var qualFqcInspectionItemEntity = await _qualFqcParameterGroupRepository.GetEntityAsync(queryentity);
            if (qualFqcInspectionItemEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }

            var result = qualFqcInspectionItemEntity.ToModel<QualFqcParameterGroupOutputDto>();
            if (result == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }

            var materialEntity = await _procMaterialRepository.GetByIdAsync((long)result.MaterialId);
            if (materialEntity != null)
            {
                result.MaterialName = materialEntity.MaterialName;
                result.MaterialCode = materialEntity.MaterialCode;
                result.MaterialUnit = materialEntity.Unit;
                result.MaterialVersion = materialEntity.Version;
            }

            return result;
        }

        public async Task CreateAsync(QualFqcParameterGroupDto createDto)
        {
            await _validationSaveRules.ValidateAndThrowAsync(createDto);

            var command = new QualFqcParameterGroupEntity()
            {
                Id = IdGenProvider.Instance.CreateId(),
                CreatedOn = HymsonClock.Now(),
                UpdatedOn = HymsonClock.Now(),
                CreatedBy = _currentUser.UserName,
                UpdatedBy = _currentUser.UserName,
                IsDeleted = 0,
                Code = createDto.MaterialId + "_" + createDto.Version,
                Name = createDto.MaterialId + "_" + createDto.Version,
                MaterialId = createDto.MaterialId,
                SiteId = _currentSite.SiteId,
                SampleQty = createDto.SampleQty,
                IsSameWorkCenter = createDto.IsSameWorkCenter,
                IsSameWorkOrder = createDto.IsSameWorkOrder,
                Version = createDto.Version,
                LotSize = createDto.LotSize,
                LotUnit = createDto.LotUnit,
                Status = createDto.Status,
            };

            if (command.Code != null || command.Name != null)
            {
                var projectCode = new QualFqcParameterGroupQuery
                {
                    SiteId = command.SiteId,
                    MaterialId = command.MaterialId,
                    Status = command.Status,
                };
                //校验项目编码
                var qualFqcInspectionItemEntity = await _qualFqcParameterGroupRepository.GetEntityAsync(projectCode);
                if (qualFqcInspectionItemEntity != null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES19955));
                }
            }

            if (command.MaterialId != null && command.Version != null)
            {
                var projectCode = new QualFqcParameterGroupQuery
                {
                    MaterialId = command.MaterialId,
                };
                //相同的客户 + 物料 + 检验项目版本
                var Entity = await _qualFqcParameterGroupRepository.GetEntityAsync(projectCode);
                if (Entity != null&&Entity.Version==command.Version)
                {
                    var materialEntity = await _procMaterialRepository.GetByIdAsync(Entity.MaterialId);
                    throw new CustomerValidationException(nameof(ErrorCode.MES19982)).WithData("materialCode", materialEntity.MaterialCode).WithData("version", command.Version);
                }
            }

            var detailCommands = Enumerable.Empty<QualFqcParameterGroupDetailEntity>();
            if (createDto.qualFqcParameterGroupDetailDtos != null && createDto.qualFqcParameterGroupDetailDtos.Any())
            {
                detailCommands = createDto.qualFqcParameterGroupDetailDtos.Select(m =>
                {
                    var detailCommand = new QualFqcParameterGroupDetailEntity()
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        IsDeleted = 0,
                        ParameterGroupId = command.Id,
                        ParameterId = m.ParameterId,
                        UpperLimit = m.UpperLimit,
                        CenterValue = m.CenterValue,
                        LowerLimit = m.LowerLimit,
                        ReferenceValue = m.ReferenceValue,
                        EnterNumber = 1,//这个不知道从哪里来
                        DisplayOrder = 1,
                        Remark = m.Remark,
                        CreatedBy = command.CreatedBy,
                        UpdatedBy = command.UpdatedBy,
                        SiteId = command.SiteId,       
                    };
                    return detailCommand;
                });
            }


            using var scope = TransactionHelper.GetTransactionScope();

            var affectedRow = await _qualFqcParameterGroupRepository.InsertAsync(command);
            if (affectedRow == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19950));
            }

            foreach (var item in detailCommands)
            {
                await _qualFqcParameterGroupDetailRepository.InsertAsync(item);
            }

            scope.Complete();
        }

        public async Task ModifyAsync(QualFqcParameterGroupUpdateDto updateDto)
        {
            await _validationUpdateRules.ValidateAndThrowAsync(updateDto);
            var query = new QualFqcParameterGroupQuery()
            {
                SiteId = updateDto.SiteId,
                MaterialId = updateDto.MaterialId,
                Status = updateDto.Status,
            };
            var entity = await _qualFqcParameterGroupRepository.GetEntityAsync(query);
            if (entity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19954));
            }

            //var command = updateDto.ToCommand<QualFqcParameterGroupUpdateCommand>();
            //command.Init();
            //command.SiteId = _currentSite.SiteId;
            //command.UpdatedBy = _currentUser.UserName;

            //QualFqcParameterGroupEntity commandInstance = new QualFqcParameterGroupEntity();

            //foreach (var item in typeof(QualFqcParameterGroupEntity).GetProperties())
            //{
            //    PropertyInfo Property = typeof(QualFqcParameterGroupUpdateCommand).GetProperty(item.Name);
            //    if (Property != null && Property.PropertyType == Property.PropertyType)
            //    {
            //        Property.SetValue(commandInstance, Property.GetValue(command));
            //    }
            //}
            var command = new QualFqcParameterGroupEntity()
            {
                Id = (long)updateDto.Id,
                CreatedOn = HymsonClock.Now(),
                UpdatedOn = HymsonClock.Now(),
                CreatedBy = _currentUser.UserName,
                UpdatedBy = _currentUser.UserName,
                IsDeleted = 0,
                Code = updateDto.MaterialId+"_"+updateDto.Version,
                Name = updateDto.MaterialId + "_" + updateDto.Version,
                MaterialId = updateDto.MaterialId,
                SiteId = _currentSite.SiteId,
                SampleQty = updateDto.SampleQty,
                IsSameWorkCenter = updateDto.IsSameWorkCenter,
                IsSameWorkOrder = updateDto.IsSameWorkOrder,
                Version = updateDto.Version,
                LotSize = updateDto.LotSize,
                LotUnit = updateDto.LotUnit,
                Status = updateDto.Status,
            };

            //var detailCommands = Enumerable.Empty<QualFqcParameterGroupDetailCreateCommand>();
            //if (updateDto.qualFqcParameterGroupDetailDtos != null && updateDto.qualFqcParameterGroupDetailDtos.Any())
            //{
            //    detailCommands = updateDto.qualFqcParameterGroupDetailDtos.Select(m =>
            //    {
            //        var detailCommand = m.ToCommand<QualFqcParameterGroupDetailCreateCommand>();
            //        detailCommand.Init();
            //        detailCommand.ParameterGroupId = command.Id;
            //        detailCommand.CreatedOn = entity.CreatedOn;
            //        detailCommand.CreatedBy = entity.CreatedBy;
            //        detailCommand.UpdatedBy = command.UpdatedBy;
            //        detailCommand.UpdatedOn = (DateTime)command.UpdatedOn;
            //        detailCommand.SiteId = command.SiteId;

            //        return detailCommand;
            //    });
            //}
            var detailCommands = Enumerable.Empty<QualFqcParameterGroupDetailEntity>();
            if (updateDto.qualFqcParameterGroupDetailDtos != null && updateDto.qualFqcParameterGroupDetailDtos.Any())
            {
                detailCommands = updateDto.qualFqcParameterGroupDetailDtos.Select(m =>
                {
                    var detailCommand = new QualFqcParameterGroupDetailEntity()
                    {
                        Id = command.Id,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        IsDeleted = 0,
                        IsDeviceCollect = m.IsDeviceCollect,
                        ParameterGroupId = command.Id,
                        ParameterId = m.ParameterId,
                        UpperLimit = m.UpperLimit,
                        CenterValue = m.CenterValue,
                        LowerLimit = m.LowerLimit,
                        ReferenceValue = m.ReferenceValue,
                        EnterNumber = 1,//这个不知道从哪里来
                        DisplayOrder = m.DisplayOrder,
                        Remark = m.Remark,
                        CreatedBy = command.CreatedBy,
                        UpdatedBy = command.UpdatedBy,
                        SiteId = command.SiteId,
                    };
                    return detailCommand;
                });
            }
            using var scope = TransactionHelper.GetTransactionScope();

            await _qualFqcParameterGroupRepository.UpdateAsync(command);

            //foreach (var details in detailCommands)
            //{
            //    QualFqcParameterGroupDetailEntity detailInstance = new QualFqcParameterGroupDetailEntity();

            //    foreach (var item in typeof(QualFqcParameterGroupDetailEntity).GetProperties())
            //    {
            //        PropertyInfo Property = typeof(QualFqcParameterGroupDetailCreateCommand).GetProperty(item.Name);
            //        if (Property != null && Property.PropertyType == Property.PropertyType)
            //        {
            //            Property.SetValue(detailInstance, Property.GetValue(details));
            //        }
            //    }
            //    await _qualFqcParameterGroupDetailRepository.InsertAsync(detailInstance);
            //}
            foreach (var item in detailCommands)
            {
                await _qualFqcParameterGroupDetailRepository.UpdateAsync(item);
            }
            scope.Complete();

        }
    }
}
