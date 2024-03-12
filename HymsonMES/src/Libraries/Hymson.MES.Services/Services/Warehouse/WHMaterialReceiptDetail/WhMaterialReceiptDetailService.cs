using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.WHMaterialReceiptDetail;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Query;
using Hymson.MES.Data.Repositories.WhMaterialReceiptDetail;
using Hymson.MES.Services.Dtos.WHMaterialReceiptDetail;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.WhMaterialReceiptDetail
{
    /// <summary>
    /// 服务（收料单详情） 
    /// </summary>
    public class WhMaterialReceiptDetailService : IWhMaterialReceiptDetailService
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
        private readonly AbstractValidator<WHMaterialReceiptDetailSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（收料单详情）
        /// </summary>
        private readonly IWhMaterialReceiptDetailRepository _whMaterialReceiptDetailRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="whMaterialReceiptDetailRepository"></param>
        public WhMaterialReceiptDetailService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<WHMaterialReceiptDetailSaveDto> validationSaveRules, 
            IWhMaterialReceiptDetailRepository whMaterialReceiptDetailRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _whMaterialReceiptDetailRepository = whMaterialReceiptDetailRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(WHMaterialReceiptDetailSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<WHMaterialReceiptDetailEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _whMaterialReceiptDetailRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(WHMaterialReceiptDetailSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

             // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<WHMaterialReceiptDetailEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _whMaterialReceiptDetailRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _whMaterialReceiptDetailRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _whMaterialReceiptDetailRepository.DeletesAsync(new DeleteCommand
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
        public async Task<WHMaterialReceiptDetailDto?> QueryByIdAsync(long id) 
        {
           var WHMaterialReceiptDetailEntity = await _whMaterialReceiptDetailRepository.GetByIdAsync(id);
           if (WHMaterialReceiptDetailEntity == null) return null;
           
           return WHMaterialReceiptDetailEntity.ToModel<WHMaterialReceiptDetailDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WHMaterialReceiptDetailDto>> GetPagedListAsync(WHMaterialReceiptDetailPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<WhMaterialReceiptDetailPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _whMaterialReceiptDetailRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<WHMaterialReceiptDetailDto>());
            return new PagedInfo<WHMaterialReceiptDetailDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}