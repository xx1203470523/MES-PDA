/*
 *creator: Karl
 *
 *describe: 标准参数关联类型表    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-13 05:06:17
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 标准参数关联类型表 服务
    /// </summary>
    public class ProcParameterLinkTypeService : IProcParameterLinkTypeService
    {
        /// <summary>
        /// 标准参数关联类型表 仓储
        /// </summary>
        private readonly IProcParameterLinkTypeRepository _procParameterLinkTypeRepository;
        private readonly AbstractValidator<ProcParameterLinkTypeCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcParameterLinkTypeModifyDto> _validationModifyRules;

        private readonly ICurrentUser _currentUser;

        public ProcParameterLinkTypeService(ICurrentUser currentUser, IProcParameterLinkTypeRepository procParameterLinkTypeRepository, AbstractValidator<ProcParameterLinkTypeCreateDto> validationCreateRules, AbstractValidator<ProcParameterLinkTypeModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _procParameterLinkTypeRepository = procParameterLinkTypeRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procParameterLinkTypeDto"></param>
        /// <returns></returns>
        public async Task CreateProcParameterLinkTypeAsync(ProcParameterLinkTypeCreateDto procParameterLinkTypeCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(procParameterLinkTypeCreateDto);

            //DTO转换实体
            var procParameterLinkTypeEntity = procParameterLinkTypeCreateDto.ToEntity<ProcParameterLinkTypeEntity>();
            procParameterLinkTypeEntity.Id = IdGenProvider.Instance.CreateId();
            procParameterLinkTypeEntity.CreatedBy = _currentUser.UserName;
            procParameterLinkTypeEntity.UpdatedBy = _currentUser.UserName;
            procParameterLinkTypeEntity.CreatedOn = HymsonClock.Now();
            procParameterLinkTypeEntity.UpdatedOn = HymsonClock.Now();

            //入库
            await _procParameterLinkTypeRepository.InsertAsync(procParameterLinkTypeEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteProcParameterLinkTypeAsync(long id)
        {
            await _procParameterLinkTypeRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesProcParameterLinkTypeAsync(string ids)
        {
            var idsArr = StringExtension.SpitLongArrary(ids);
            return await _procParameterLinkTypeRepository.DeletesAsync(idsArr);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procParameterLinkTypePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcParameterLinkTypeViewDto>> GetPageListAsync(ProcParameterLinkTypePagedQueryDto procParameterLinkTypePagedQueryDto)
        {
            var procParameterLinkTypePagedQuery = procParameterLinkTypePagedQueryDto.ToQuery<ProcParameterLinkTypePagedQuery>();
            procParameterLinkTypePagedQuery.SiteCode = "TODO";
            var pagedInfo = await _procParameterLinkTypeRepository.GetPagedInfoAsync(procParameterLinkTypePagedQuery);

            //实体到DTO转换 装载数据
            List<ProcParameterLinkTypeViewDto> procParameterLinkTypeDtos = PrepareProcParameterLinkTypeDtos(pagedInfo);
            return new PagedInfo<ProcParameterLinkTypeViewDto>(procParameterLinkTypeDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 分页查询详情（设备/产品参数）
        /// </summary>
        /// <param name="procParameterDetailPagerQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcParameterLinkTypeViewDto>> QueryPagedProcParameterLinkTypeByTypeAsync(ProcParameterDetailPagerQueryDto procParameterDetailPagerQueryDto) 
        {
            var procParameterDetailPagerQuery = procParameterDetailPagerQueryDto.ToQuery<ProcParameterDetailPagerQuery>();
            procParameterDetailPagerQuery.SiteCode = "TODO";
            var pagedInfo = await _procParameterLinkTypeRepository.GetPagedProcParameterLinkTypeByTypeAsync(procParameterDetailPagerQuery);

            //实体到DTO转换 装载数据
            List<ProcParameterLinkTypeViewDto> procParameterLinkTypeDtos = PrepareProcParameterLinkTypeDtos(pagedInfo);
            return new PagedInfo<ProcParameterLinkTypeViewDto>(procParameterLinkTypeDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcParameterLinkTypeViewDto> PrepareProcParameterLinkTypeDtos(PagedInfo<ProcParameterLinkTypeView>   pagedInfo)
        {
            var procParameterLinkTypeDtos = new List<ProcParameterLinkTypeViewDto>();
            foreach (var procParameterLinkTypeEntity in pagedInfo.Data)
            {
                var procParameterLinkTypeDto = procParameterLinkTypeEntity.ToModel<ProcParameterLinkTypeViewDto>();
                procParameterLinkTypeDtos.Add(procParameterLinkTypeDto);
            }

            return procParameterLinkTypeDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procParameterLinkTypeDto"></param>
        /// <returns></returns>
        public async Task ModifyProcParameterLinkTypeAsync(ProcParameterLinkTypeModifyDto procParameterLinkTypeModifyDto)
        {
             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(procParameterLinkTypeModifyDto);

            //DTO转换实体
            var procParameterLinkTypeEntity = procParameterLinkTypeModifyDto.ToEntity<ProcParameterLinkTypeEntity>();
            procParameterLinkTypeEntity.UpdatedBy = _currentUser.UserName;
            procParameterLinkTypeEntity.UpdatedOn = HymsonClock.Now();

            await _procParameterLinkTypeRepository.UpdateAsync(procParameterLinkTypeEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcParameterLinkTypeDto> QueryProcParameterLinkTypeByIdAsync(long id) 
        {
           var procParameterLinkTypeEntity = await _procParameterLinkTypeRepository.GetByIdAsync(id);
           if (procParameterLinkTypeEntity != null) 
           {
               return procParameterLinkTypeEntity.ToModel<ProcParameterLinkTypeDto>();
           }
            return null;
        }
    }
}
