using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process.PrintConfig;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers
{
    /// <summary>
    /// ��ӡ������
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcPrintConfigController : ControllerBase
    {
        /// <summary>
        /// ��ӡ���ñ��ӿ�
        /// </summary>
        private readonly IProcPrintConfigService _procPrintConfigService;
        private readonly ILogger<ProcPrintConfigController> _logger;

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="procResourceService"></param>
        /// <param name="logger"></param>
        public ProcPrintConfigController(IProcPrintConfigService procResourceService, ILogger<ProcPrintConfigController> logger)
        {
            _procPrintConfigService = procResourceService;
            _logger = logger;
        }

        /// <summary>
        /// ��ȡ��ҳ����
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("list")]
        [HttpGet]
        public async Task<PagedInfo<ProcPrinterDto>> QueryProcPrintConfigAsync([FromQuery] ProcPrinterPagedQueryDto query)
        {
            return await _procPrintConfigService.GetPageListAsync(query);
        }

        /// <summary>
        /// ��ȡ��ҳ����
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("querylist")]
        [HttpGet]
        public async Task<PagedInfo<ProcPrinterDto>> GetProcPrintConfigListAsync([FromQuery] ProcPrinterPagedQueryDto query)
        {
            return await _procPrintConfigService.GetListAsync(query);
        }

        /// <summary>
        /// ��ѯ��ӡ���ñ�����
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcPrinterDto> GetProcPrintConfigAsync(long id)
        {
            return await _procPrintConfigService.GetByIdAsync(id);
        }
        /// <summary>
        /// ��ѯ��ӡ���ñ�����
        /// </summary>
        /// <param name="printName"></param>
        /// <returns></returns>
        [Route("printName")]
        [HttpGet]
        public async Task<ProcPrinterDto> GetProcPrintConfigAsync(string printName)
        {
            return await _procPrintConfigService.GetByPrintNameAsync(printName);
        }

        /// <summary>
        /// ���Ӵ�ӡ����
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("��ӡ������", BusinessType.INSERT)]
        [PermissionDescription("proc:printConfig:insert")]
        public async Task AddProcPrintConfigAsync([FromBody] ProcPrinterDto parm)
        {
            await _procPrintConfigService.AddProcPrintConfigAsync(parm);
        }

        /// <summary>
        /// ���´�ӡ���ñ�
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [LogDescription("��ӡ������", BusinessType.UPDATE)]
        [PermissionDescription("proc:printConfig:update")]
        public async Task UpdateProcPrintConfigAsync([FromBody] ProcPrinterUpdateDto parm)
        {
            await _procPrintConfigService.UpdateProcPrintConfigAsync(parm);
        }

        /// <summary>
        /// ɾ����ӡ���ñ�
        /// </summary>
        /// <param name="deleteDto"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("��ӡ������", BusinessType.DELETE)]
        [PermissionDescription("proc:printConfig:delete")]
        public async Task DeleteProcPrintConfigAsync(DeleteDto deleteDto)
        {
            await _procPrintConfigService.DeleteProcPrintConfigAsync(deleteDto.Ids);
        }
    }
}