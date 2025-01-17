﻿using Hymson.EventBus.Abstractions;
using Hymson.MES.CoreServices.Events.ProcessEvents.PrintEvents;
using Hymson.MES.CoreServices.Services.Process.Print;

namespace Hymson.MES.BackgroundServices.EventHandling.ProcessEventHandling.PrintEventHandling
{
    /// <summary>
    /// 执行打印
    /// </summary>
    public class ExecPrintIntegrationEventHandler : IIntegrationEventHandler<PrintIntegrationEvent>
    {
        /// <summary>
        /// 消息服务
        /// </summary>
        private readonly IExecPrintService _execPrintService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="execPrintService"></param>
        public ExecPrintIntegrationEventHandler(IExecPrintService execPrintService)
        {
            _execPrintService = execPrintService;
        }

        /// <summary>
        /// 处理方法
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public async Task Handle(PrintIntegrationEvent @event)
        {
            await _execPrintService.PrintAsync(@event);
        }
    }
}
