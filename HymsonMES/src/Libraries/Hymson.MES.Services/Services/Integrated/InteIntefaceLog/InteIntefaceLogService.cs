﻿using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Elasticsearch;
using Hymson.Infrastructure.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.Logging;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;

namespace Hymson.MES.Services.Services.Integrated.InteIntefaceLog
{
    public class InteIntefaceLogService : IInteIntefaceLogService
    {
        /// <summary>
        /// ES日志查询
        /// </summary>
        private readonly ILogDataService _logDataService;

        /// <summary>
        public InteIntefaceLogService(ILogDataService logDataService)
        {
            _logDataService = logDataService;
        }

        /// <summary>
        /// 根据查询条件获取分页数据 InteIntefaceLogDto
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<TraceLogEntry>> GetPagedListAsync(InteIntefaceLogPagedQueryDto pagedQueryDto)
        {
            var logDataPagedQuery = new LogDataPagedQuery()
            {
                PageIndex = pagedQueryDto.PageIndex,
                PageSize = pagedQueryDto.PageSize,
            };
            string? queryType = string.Empty;

            if (pagedQueryDto.QueryType.HasValue)
            {
                queryType = pagedQueryDto?.QueryType!.ToString();
            }

            if (pagedQueryDto?.QueryType == InterfaceLogQueryTyeEnum.SystemLog)
            {
                logDataPagedQuery.ServiceType = ServiceTypeEnum.MES;
            }

            if (pagedQueryDto?.TimeStamp != null)
            {
                logDataPagedQuery.BeginTime = pagedQueryDto.TimeStamp[0];
                logDataPagedQuery.EndTime = pagedQueryDto.TimeStamp[1].AddDays(1);
            }

            if (pagedQueryDto?.Id != null)
            {
                logDataPagedQuery.Id = pagedQueryDto.Id;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(queryType)) throw new CustomerValidationException(nameof(ErrorCode.MES10138));
                logDataPagedQuery.Type = queryType;
            }

            if (pagedQueryDto?.InterfaceCode != null)
            {
                logDataPagedQuery.InterfaceCode = pagedQueryDto.InterfaceCode;
            }

            if (pagedQueryDto?.Message != null)
            {
                logDataPagedQuery.Message = pagedQueryDto.Message;
            }

            var data = new Dictionary<string, string> { };

            if (pagedQueryDto?.Data?.Code != null)
            {
                data.Add("Code", pagedQueryDto.Data.Code);
            }

            if (pagedQueryDto?.Data?.Name != null)
            {
                data.Add("Name", pagedQueryDto.Data.Name);
            }

            if (pagedQueryDto?.Data?.ReceiverType != null)
            {
                data.Add("ReceiverType", pagedQueryDto.Data.ReceiverType);
            }

            if (pagedQueryDto?.Data?.IsSuccess != null)
            {
                data.Add("IsSuccess", pagedQueryDto.Data.IsSuccess.ToString() ?? "0");
            }

            logDataPagedQuery.Data = data;

            return await _logDataService.GetLogDataPagedAsync(logDataPagedQuery);
        }

    }
}
