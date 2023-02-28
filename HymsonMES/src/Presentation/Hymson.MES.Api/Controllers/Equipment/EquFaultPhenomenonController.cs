using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.Equipment.EquFaultPhenomenon;
using Hymson.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（设备故障现象）
    /// @author Czhipu
    /// @date 2023-02-15 08:56:34
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquFaultPhenomenonController : ControllerBase
    {
        /// <summary>
        /// 接口（设备故障现象）
        /// </summary>
        private readonly IEquFaultPhenomenonService _equFaultPhenomenonService;
        private readonly ILogger<EquFaultPhenomenonController> _logger;

        /// <summary>
        /// 构造函数（设备故障现象）
        /// </summary>
        /// <param name="equFaultPhenomenonService"></param>
        public EquFaultPhenomenonController(IEquFaultPhenomenonService equFaultPhenomenonService, ILogger<EquFaultPhenomenonController> logger)
        {
            _equFaultPhenomenonService = equFaultPhenomenonService;
            _logger = logger;
        }


        /// <summary>
        /// 添加（设备故障现象）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task CreateAsync(EquFaultPhenomenonCreateDto createDto)
        {
            await _equFaultPhenomenonService.CreateAsync(createDto);
        }

        /// <summary>
        /// 更新（设备故障现象）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task ModifyAsync(EquFaultPhenomenonModifyDto modifyDto)
        {
            await _equFaultPhenomenonService.ModifyAsync(modifyDto);
        }

        /// <summary>
        /// 删除（设备故障现象）
        /// </summary>
        /// <param name="deleteDto"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeletesAsync(EquFaultPhenomenonDeleteDto deleteDto)
        {
            await _equFaultPhenomenonService.DeletesAsync(deleteDto.Ids);
        }

        /// <summary>
        /// 分页查询列表（设备故障现象）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("page")]
        public async Task<PagedInfo<EquFaultPhenomenonDto>> GetPagedListAsync([FromQuery] EquFaultPhenomenonPagedQueryDto pagedQueryDto)
        {
            return await _equFaultPhenomenonService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询详情（设备故障现象）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquFaultPhenomenonDto> GetDetailAsync(long id)
        {
            return await _equFaultPhenomenonService.GetDetailAsync(id);
        }

    }
}