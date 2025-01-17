/*
 *creator: Karl
 *
 *describe: 设备点检模板    控制器 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2024-05-13 03:06:41
 */
using Elastic.Clients.Elasticsearch;
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquSpotcheckTemplate;
using Hymson.MES.Services.Services.EquSpotcheckTemplate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.EquSpotcheckTemplate
{
    /// <summary>
    /// 控制器（设备点检模板）
    /// @author pengxin
    /// @date 2024-05-13 03:06:41
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquSpotcheckTemplateController : ControllerBase
    {
        /// <summary>
        /// 接口（设备点检模板）
        /// </summary>
        private readonly IEquSpotcheckTemplateService _equSpotcheckTemplateService;
        private readonly ILogger<EquSpotcheckTemplateController> _logger;

        /// <summary>
        /// 构造函数（设备点检模板）
        /// </summary>
        /// <param name="equSpotcheckTemplateService"></param>
        public EquSpotcheckTemplateController(IEquSpotcheckTemplateService equSpotcheckTemplateService, ILogger<EquSpotcheckTemplateController> logger)
        {
            _equSpotcheckTemplateService = equSpotcheckTemplateService;
            _logger = logger;
        }

        #region 框架生成方法

        /// <summary>
        /// 分页查询列表（设备点检模板）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquSpotcheckTemplateDto>> QueryPagedEquSpotcheckTemplateAsync([FromQuery] EquSpotcheckTemplatePagedQueryDto parm)
        {
            return await _equSpotcheckTemplateService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 查询详情（设备点检模板）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquSpotcheckTemplateDto> QueryEquSpotcheckTemplateByIdAsync(long id)
        {
            return await _equSpotcheckTemplateService.QueryEquSpotcheckTemplateByIdAsync(id);
        }

        /// <summary>
        /// 添加（设备点检模板）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddEquSpotcheckTemplateAsync([FromBody] EquSpotcheckTemplateCreateDto parm)
        {
            await _equSpotcheckTemplateService.CreateEquSpotcheckTemplateAsync(parm);
        }

        /// <summary>
        /// 更新（设备点检模板）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateEquSpotcheckTemplateAsync([FromBody] EquSpotcheckTemplateModifyDto parm)
        {
            await _equSpotcheckTemplateService.ModifyEquSpotcheckTemplateAsync(parm);
        }

        /// <summary>
        /// 删除（设备点检模板）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteEquSpotcheckTemplateAsync([FromBody] EquSpotcheckTemplateDeleteDto param)
        {
            await _equSpotcheckTemplateService.DeletesEquSpotcheckTemplateAsync(param);
        }


        /// <summary>
        /// 获取模板关联信息（项目）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getitem")]
        public async Task<List<GetSpotcheckItemRelationListDto>> QueryItemRelationListAsync([FromQuery] GetEquSpotcheckTemplateItemRelationDto param)
        {
            return await _equSpotcheckTemplateService.QueryItemRelationListAsync(param);
        }

        /// <summary>
        /// 获取模板关联信息（设备组）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getgroup")]
        public async Task<List<QuerySpotcheckEquipmentGroupRelationListDto>> QueryEquipmentGroupRelationListAsync([FromQuery] GetEquSpotcheckTemplateItemRelationDto param)
        {
            return await _equSpotcheckTemplateService.QueryEquipmentGroupRelationListAsync(param);
        }
        #endregion
    }
}