using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Services.Equipment;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（点检记录表）
    /// @author User
    /// @date 2024-04-03 04:50:07
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquInspectionRecordController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquInspectionRecordController> _logger;
        /// <summary>
        /// 服务接口（点检记录表）
        /// </summary>
        private readonly IEquInspectionRecordService _equInspectionRecordService;


        /// <summary>
        /// 构造函数（点检记录表）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="equInspectionRecordService"></param>
        public EquInspectionRecordController(ILogger<EquInspectionRecordController> logger, IEquInspectionRecordService equInspectionRecordService)
        {
            _logger = logger;
            _equInspectionRecordService = equInspectionRecordService;
        }

        /// <summary>
        /// 添加（点检记录表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("点检记录表", BusinessType.INSERT)]
        public async Task AddAsync([FromBody] EquInspectionRecordSaveDto saveDto)
        {
             await _equInspectionRecordService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（点检记录表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("点检记录表", BusinessType.UPDATE)]
        public async Task UpdateAsync([FromBody] EquInspectionRecordSaveDto saveDto)
        {
             await _equInspectionRecordService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（点检记录表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("点检记录表", BusinessType.DELETE)]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _equInspectionRecordService.DeletesAsync(ids);
        }

        /// <summary>
        /// 分页查询列表（设备点检录入）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquInspectionRecordDto>> QueryPagedListAsync([FromQuery] EquInspectionRecordPagedQueryDto pagedQueryDto)
        {
            return await _equInspectionRecordService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquInspectionOperateDto?> QueryByRecordIdAsync(long id)
        {
            return await _equInspectionRecordService.QueryByRecordIdAsync(id);
        }

        /// <summary>
        /// 开始校验
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("start")]
        [LogDescription("点检记录表", BusinessType.OTHER)]
        public async Task<long> StartVerificationAsync(EquInspectionCompleteDto requestDto)
        {
            return await _equInspectionRecordService.StartVerificationAsync(requestDto);
        }

        /// <summary>
        /// 保存检验单
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("save")]
        [LogDescription("点检记录表", BusinessType.OTHER)]
        public async Task<long> SaveVerificationnAsync(EquInspectionSaveDto requestDto)
        {
            return await _equInspectionRecordService.SaveVerificationnAsync(requestDto);
        }

        /// <summary>
        /// 完成检验单
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("complete")]
        [LogDescription("点检记录表", BusinessType.OTHER)]
        public async Task<long> CompleteVerificationAsync(EquInspectionCompleteDto requestDto)
        {
            return await _equInspectionRecordService.CompleteVerificationAsync(requestDto);
        }
    }
}