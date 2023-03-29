/*
 *creator: Karl
 *
 *describe: 工单激活    控制器 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-03-29 10:23:51
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Services.Plan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;

namespace Hymson.MES.Api.Controllers.Plan
{
    /// <summary>
    /// 控制器（工单激活）
    /// @author Karl
    /// @date 2023-03-29 10:23:51
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PlanWorkOrderActivationController : ControllerBase
    {
        /// <summary>
        /// 接口（工单激活）
        /// </summary>
        private readonly IPlanWorkOrderActivationService _planWorkOrderActivationService;
        private readonly ILogger<PlanWorkOrderActivationController> _logger;

        /// <summary>
        /// 构造函数（工单激活）
        /// </summary>
        /// <param name="planWorkOrderActivationService"></param>
        public PlanWorkOrderActivationController(IPlanWorkOrderActivationService planWorkOrderActivationService, ILogger<PlanWorkOrderActivationController> logger)
        {
            _planWorkOrderActivationService = planWorkOrderActivationService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（工单激活）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<PlanWorkOrderActivationListDetailViewDto>> QueryPagedPlanWorkOrderActivationAsync([FromQuery] PlanWorkOrderActivationPagedQueryDto parm)
        {
            return await _planWorkOrderActivationService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（工单激活）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<PlanWorkOrderActivationDto> QueryPlanWorkOrderActivationByIdAsync(long id)
        {
            return await _planWorkOrderActivationService.QueryPlanWorkOrderActivationByIdAsync(id);
        }

        /// <summary>
        /// 添加（工单激活）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddPlanWorkOrderActivationAsync([FromBody] PlanWorkOrderActivationCreateDto parm)
        {
             await _planWorkOrderActivationService.CreatePlanWorkOrderActivationAsync(parm);
        }

        /// <summary>
        /// 更新（工单激活）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdatePlanWorkOrderActivationAsync([FromBody] PlanWorkOrderActivationModifyDto parm)
        {
             await _planWorkOrderActivationService.ModifyPlanWorkOrderActivationAsync(parm);
        }

        /// <summary>
        /// 删除（工单激活）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeletePlanWorkOrderActivationAsync([FromBody] long[] ids)
        {
            await _planWorkOrderActivationService.DeletesPlanWorkOrderActivationAsync(ids);
        }

    }
}