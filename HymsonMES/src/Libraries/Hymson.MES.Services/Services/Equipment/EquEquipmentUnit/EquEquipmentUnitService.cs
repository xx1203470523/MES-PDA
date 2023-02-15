﻿using FluentValidation;
using Hymson.Authentication;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentUnit;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentUnit.Query;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.Snowflake;

namespace Hymson.MES.Services.Services.Equipment.EquEquipmentUnit
{
    /// <summary>
    /// 
    /// </summary>
    public class EquEquipmentUnitService : IEquEquipmentUnitService
    {
        /// <summary>
        /// 当前登录用户对象
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 
        /// </summary>
        private readonly IEquEquipmentUnitRepository _equEquipmentUnitRepository;
        private readonly AbstractValidator<EquEquipmentUnitCreateDto> _validationCreateRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="equEquipmentUnitRepository"></param>
        /// <param name="validationRules"></param>
        public EquEquipmentUnitService(ICurrentUser currentUser,
            IEquEquipmentUnitRepository equEquipmentUnitRepository,
            AbstractValidator<EquEquipmentUnitCreateDto> validationRules)
        {
            _currentUser = currentUser;
            _equEquipmentUnitRepository = equEquipmentUnitRepository;
            _validationCreateRules = validationRules;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task<int> CreateEquipmentUnitAsync(EquEquipmentUnitCreateDto createDto)
        {
            // 验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(createDto);

            // DTO转换实体
            var entity = createDto.ToEntity<EquEquipmentUnitEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;

            // 保存实体
            return await _equEquipmentUnitRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyEquipmentUnitAsync(EquEquipmentUnitModifyDto modifyDto)
        {
            // DTO转换实体
            var entity = modifyDto.ToEntity<EquEquipmentUnitEntity>();
            entity.UpdatedBy = _currentUser.UserName;

            // 保存实体
            return await _equEquipmentUnitRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeleteEquipmentUnitAsync(long[] idsArr)
        {
            return await _equEquipmentUnitRepository.DeleteAsync(idsArr);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquEquipmentUnitDto>> GetPagedListAsync(EquEquipmentUnitPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquEquipmentUnitPagedQuery>();
            var pagedInfo = await _equEquipmentUnitRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquEquipmentUnitDto>());
            return new PagedInfo<EquEquipmentUnitDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquEquipmentUnitDto> GetEntityAsync(long id)
        {
            return (await _equEquipmentUnitRepository.GetByIdAsync(id)).ToModel<EquEquipmentUnitDto>();
        }


    }
}
