using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Qual;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.MES.Services.Dtos.Qual;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Quality
{
    /// <summary>
    /// 服务（OQC检验参数组明细） 
    /// </summary>
    public class QualOqcParameterGroupDetailService : IQualOqcParameterGroupDetailService
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
        private readonly AbstractValidator<QualOqcParameterGroupDetailSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（OQC检验参数组明细）
        /// </summary>
        private readonly IQualOqcParameterGroupDetailRepository _qualOqcParameterGroupDetailRepository;

        /// <summary>
        /// 标准参数
        /// </summary>
        private readonly IProcParameterRepository _procParameterRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="qualOqcParameterGroupDetailRepository"></param>
        public QualOqcParameterGroupDetailService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<QualOqcParameterGroupDetailSaveDto> validationSaveRules, 
            IQualOqcParameterGroupDetailRepository qualOqcParameterGroupDetailRepository, IProcParameterRepository procParameterRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _qualOqcParameterGroupDetailRepository = qualOqcParameterGroupDetailRepository;
            _procParameterRepository = procParameterRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(QualOqcParameterGroupDetailSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<QualOqcParameterGroupDetailEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _qualOqcParameterGroupDetailRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(QualOqcParameterGroupDetailSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

             // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<QualOqcParameterGroupDetailEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _qualOqcParameterGroupDetailRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _qualOqcParameterGroupDetailRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _qualOqcParameterGroupDetailRepository.DeletesAsync(new DeleteCommand
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
        public async Task<QualOqcParameterGroupDetailDto?> QueryByIdAsync(long id) 
        {
           var qualOqcParameterGroupDetailEntity = await _qualOqcParameterGroupDetailRepository.GetByIdAsync(id);
           if (qualOqcParameterGroupDetailEntity == null) return null;
           
           return qualOqcParameterGroupDetailEntity.ToModel<QualOqcParameterGroupDetailDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualOqcParameterGroupDetailDto>> GetPagedListAsync(QualOqcParameterGroupDetailPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<QualOqcParameterGroupDetailPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _qualOqcParameterGroupDetailRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<QualOqcParameterGroupDetailDto>());
            return new PagedInfo<QualOqcParameterGroupDetailDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 获取数据集
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualOqcParameterGroupDetailOutputDto>> GetListAsync(QualOqcParameterGroupDetailQueryDto queryDto)
        {
            var query = queryDto.ToQuery<QualOqcParameterGroupDetailQuery>();
            query.SiteId = _currentSite.SiteId;

            var qualIqcInspectionItemDetailEntities = await _qualOqcParameterGroupDetailRepository.GetEntitiesAsync(query);
            if (qualIqcInspectionItemDetailEntities == null || !qualIqcInspectionItemDetailEntities.Any())
            {
                return Enumerable.Empty<QualOqcParameterGroupDetailOutputDto>();
            }

            var parameterIds = qualIqcInspectionItemDetailEntities.Select(m => m.ParameterId);
            var parameterEntities = await _procParameterRepository.GetByIdsAsync(parameterIds);

            var result = qualIqcInspectionItemDetailEntities.Select(m =>
            {
                var item = m.ToModel<QualOqcParameterGroupDetailOutputDto>();

                var parameterEntity = parameterEntities.FirstOrDefault(e => e.Id == m.ParameterId);
                if (parameterEntity != null)
                {
                    item.ParameterCode = parameterEntity.ParameterCode;
                    item.ParameterName = parameterEntity.ParameterName;
                    item.ParameterUnit = parameterEntity.ParameterUnit;
                }
                return item;
            });

            return result;

        }

    }
}
