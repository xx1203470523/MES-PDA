/*
 *creator: Karl
 *
 *describe: 载具注册表    控制器 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-07-14 10:03:53
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// 控制器（载具注册表）
    /// @author Karl
    /// @date 2023-07-14 10:03:53
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteVehicleController : ControllerBase
    {
        /// <summary>
        /// 接口（载具注册表）
        /// </summary>
        private readonly IInteVehicleService _inteVehicleService;
        private readonly ILogger<InteVehicleController> _logger;

        /// <summary>
        /// 构造函数（载具注册表）
        /// </summary>
        /// <param name="inteVehicleService"></param>
        public InteVehicleController(IInteVehicleService inteVehicleService, ILogger<InteVehicleController> logger)
        {
            _inteVehicleService = inteVehicleService;
            _logger = logger;
        }

        #region 框架生成方法

        /// <summary>
        /// 分页查询列表（载具注册表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<InteVehicleViewDto>> QueryPagedInteVehicleAsync([FromQuery] InteVehiclePagedQueryDto parm)
        {
            return await _inteVehicleService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 查询详情（载具注册表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteVehicleDto> QueryInteVehicleByIdAsync(long id)
        {
            return await _inteVehicleService.QueryInteVehicleByIdAsync(id);
        }

        /// <summary>
        /// 添加（载具注册表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("载具注册", BusinessType.INSERT)]
        [PermissionDescription("inte:inteVehicle:insert")]
        public async Task AddInteVehicleAsync([FromBody] InteVehicleCreateDto parm)
        {
             await _inteVehicleService.CreateInteVehicleAsync(parm);
        }

        /// <summary>
        /// 更新（载具注册表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("载具注册", BusinessType.UPDATE)]
        [PermissionDescription("inte:inteVehicle:update")]
        public async Task UpdateInteVehicleAsync([FromBody] InteVehicleModifyDto parm)
        {
             await _inteVehicleService.ModifyInteVehicleAsync(parm);
        }

        /// <summary>
        /// 删除（载具注册表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("载具注册", BusinessType.DELETE)]
        [PermissionDescription("inte:inteVehicle:delete")]
        public async Task DeleteInteVehicleAsync([FromBody] long[] ids)
        {
            await _inteVehicleService.DeletesInteVehicleAsync(ids);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getVehicleVerifyByVehicleId/{id}")]
        public async Task<InteVehicleVerifyDto> QueryVehicleVerifyByVehicleIdAsync(long id) 
        {
            return await _inteVehicleService.QueryVehicleVerifyByVehicleIdAsync(id);
        }
    }
}