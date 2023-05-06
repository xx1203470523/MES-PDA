﻿using Hymson.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Equipment.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class HostedService : IHostedService
    {
        private readonly JwtOptions _jwtOptions;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="jwtOptions"></param>
        public HostedService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }
        /// <summary>
        /// 启动时运行
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var equipmentModel = new EquipmentModel
            {
                FactoryId = 123456,
                Id = 1,
                Name = "设备1",
                SiteId = 123456,
            };
            var token = JwtHelper.GenerateJwtToken(equipmentModel, _jwtOptions);
            Console.WriteLine(token);
            return Task.CompletedTask;
        }
        /// <summary>
        /// 关闭时运行
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return  Task.CompletedTask;
        }
    }
}