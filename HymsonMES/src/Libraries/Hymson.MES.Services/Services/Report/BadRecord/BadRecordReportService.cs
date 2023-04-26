using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.IQualityRepository;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Transactions;

namespace Hymson.MES.Services.Services.Report
{
    /// <summary>
    /// 不良记录报表 服务
    /// </summary>
    public class BadRecordReportService : IBadRecordReportService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 工单信息表 仓储
        /// </summary>
        private readonly IManuProductBadRecordRepository _manuProductBadRecordRepository;
        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        public BadRecordReportService(ICurrentUser currentUser, ICurrentSite currentSite, IManuProductBadRecordRepository manuProductBadRecordRepository, IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;

            _manuProductBadRecordRepository=manuProductBadRecordRepository;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
        }

        /// <summary>
        /// 根据查询条件获取不良报表分页数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuProductBadRecordReportViewDto>> GetPageListAsync(BadRecordReportDto param)
        {
            var pagedQuery = param.ToQuery<ManuProductBadRecordReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _manuProductBadRecordRepository.GetPagedInfoReportAsync(pagedQuery);

            var unqualifiedIds= pagedInfo.Data.Select(x=>x.UnqualifiedId).ToArray();
            var unqualifiedCodeEntities = await _qualUnqualifiedCodeRepository.GetByIdsAsync(unqualifiedIds);

            List< ManuProductBadRecordReportViewDto > listDto=new List< ManuProductBadRecordReportViewDto >();
            foreach (var item in pagedInfo.Data)
            {
                var unqualifiedCodeEntitie = unqualifiedCodeEntities.Where(y => y.Id == item.UnqualifiedId).FirstOrDefault();

                listDto.Add(new ManuProductBadRecordReportViewDto
                {
                    UnqualifiedId = item.UnqualifiedId,
                    Num = item.Num,
                    UnqualifiedCode = unqualifiedCodeEntitie?.UnqualifiedCode ?? "",
                    UnqualifiedCodeName = unqualifiedCodeEntitie?.UnqualifiedCodeName??""
                });
            }
            
            return new PagedInfo<ManuProductBadRecordReportViewDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据查询条件获取不良报表分页数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<ManuProductBadRecordReportViewDto>> GetTopTenBadRecordAsync(BadRecordReportDto param)
        {
            var pagedQuery = param.ToQuery<ManuProductBadRecordReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            pagedQuery.PageIndex = 1;
            pagedQuery.PageSize = 10;

            var badRecordslist = await _manuProductBadRecordRepository.GetTopNumReportAsync(pagedQuery);

            var unqualifiedIds = badRecordslist.Select(x => x.UnqualifiedId).ToArray();
            var unqualifiedCodeEntities = await _qualUnqualifiedCodeRepository.GetByIdsAsync(unqualifiedIds);

            List<ManuProductBadRecordReportViewDto> listDto = new List<ManuProductBadRecordReportViewDto>();
            foreach (var item in badRecordslist)
            {
                var unqualifiedCodeEntitie = unqualifiedCodeEntities.Where(y => y.Id == item.UnqualifiedId).FirstOrDefault();

                listDto.Add(new ManuProductBadRecordReportViewDto
                {
                    UnqualifiedId = item.UnqualifiedId,
                    Num = item.Num,
                    UnqualifiedCode = unqualifiedCodeEntitie?.UnqualifiedCode ?? "",
                    UnqualifiedCodeName = unqualifiedCodeEntitie?.UnqualifiedCodeName ?? ""
                });
            }

            return listDto;
        }

        /// <summary>
        /// 根据查询条件获取不良日志报表分页数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuProductBadRecordLogReportViewDto>> GetLogPageListAsync(ManuProductBadRecordLogReportPagedQueryDto param)
        {
            var pagedQuery = param.ToQuery<ManuProductBadRecordLogReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _manuProductBadRecordRepository.GetPagedInfoLogReportAsync(pagedQuery);

            List<ManuProductBadRecordLogReportViewDto> listDto = new List<ManuProductBadRecordLogReportViewDto>();
            foreach (var item in pagedInfo.Data)
            {
                listDto.Add( item.ToModel<ManuProductBadRecordLogReportViewDto>());
            }

            return new PagedInfo<ManuProductBadRecordLogReportViewDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }
    }
}