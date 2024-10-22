using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.EquEquipmentGroup;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（设备组）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquEquipmentGroupController : ControllerBase
    {
        /// <summary>
        /// 接口（设备组）
        /// </summary>
        private readonly IEquEquipmentGroupService _equEquipmentGroupService;
        private readonly ILogger<EquEquipmentGroupController> _logger;

        /// <summary>
        /// 构造函数（设备组）
        /// </summary>
        /// <param name="equEquipmentGroupService"></param>
        /// <param name="logger"></param>
        public EquEquipmentGroupController(IEquEquipmentGroupService equEquipmentGroupService, ILogger<EquEquipmentGroupController> logger)
        {
            _equEquipmentGroupService = equEquipmentGroupService;
            _logger = logger;
        }

        /// <summary>
        /// 添加（设备组）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("设备组", BusinessType.INSERT)]
        [PermissionDescription("equ:equipmentGroup:insert")]
        public async Task<long> CreateAsync(EquEquipmentGroupSaveDto createDto)
        {
            return await _equEquipmentGroupService.CreateAsync(createDto);
        }

        /// <summary>
        /// 更新（设备组）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPut]
        [LogDescription("设备组", BusinessType.UPDATE)]
        [PermissionDescription("equ:equipmentGroup:update")]
        public async Task ModifyAsync(EquEquipmentGroupSaveDto modifyDto)
        {
            await _equEquipmentGroupService.ModifyAsync(modifyDto);
        }

        /// <summary>
        /// 删除（设备组）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("设备组", BusinessType.DELETE)]
        [PermissionDescription("equ:equipmentGroup:delete")]
        public async Task DeletesAsync(long[] ids)
        {
            await _equEquipmentGroupService.DeletesAsync(ids);
        }

        /// <summary>
        /// 分页查询列表（设备组）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("page")]
        //[PermissionDescription("equ:equipmentGroup:list")]
        public async Task<PagedInfo<EquEquipmentGroupListDto>> GetPagedListAsync([FromQuery] EquEquipmentGroupPagedQueryDto pagedQueryDto)
        {
            return await _equEquipmentGroupService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询详情（设备组）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquEquipmentGroupDto> GetDetailAsync(long id)
        {
            return await _equEquipmentGroupService.GetDetailAsync(id);
        }
    }
}