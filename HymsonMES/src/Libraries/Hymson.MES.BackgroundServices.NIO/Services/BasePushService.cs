﻿using Hymson.MES.BackgroundServices.NIO.Dtos;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Mavel;
using Hymson.MES.Core.Enums.Plan;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.BackgroundServices.NIO.Services
{
    /// <summary>
    /// 推送服务基类
    /// </summary>
    public class BasePushService
    {
        /// <summary>
        /// 仓储接口（蔚来推送开关）
        /// </summary>
        private readonly INioPushSwitchRepository _nioPushSwitchRepository;

        /// <summary>
        /// 仓储接口（蔚来推送）
        /// </summary>
        private readonly INioPushRepository _nioPushRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nioPushSwitchRepository"></param>
        /// <param name="nioPushRepository"></param>
        public BasePushService(INioPushSwitchRepository nioPushSwitchRepository, INioPushRepository nioPushRepository)
        {
            _nioPushSwitchRepository = nioPushSwitchRepository;
            _nioPushRepository = nioPushRepository;
        }

        /// <summary>
        /// 是否允许推送
        /// </summary>
        /// <param name="buzSceneEnum"></param>
        /// <returns></returns>
        public async Task<NioPushSwitchEntity?> GetSwitchEntityAsync(BuzSceneEnum buzSceneEnum)
        {
            // 总开关是否开启
            var masterConfig = await _nioPushSwitchRepository.GetBySceneAsync(BuzSceneEnum.All);
            if (masterConfig == null || masterConfig.IsEnabled != TrueOrFalseEnum.Yes) return default;

            // 子开关是否开启
            var config = await _nioPushSwitchRepository.GetBySceneAsync(buzSceneEnum);
            if (config == null || config.IsEnabled != TrueOrFalseEnum.Yes) return default;

            return config;
        }

        /// <summary>
        /// 添加到推送队列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <param name="buzSceneEnum"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task AddToPushQueueAsync<T>(NioPushSwitchEntity config, BuzSceneEnum buzSceneEnum, IEnumerable<T> data)
        {
            var user = "AddToPushQueue";
            var time = HymsonClock.Now();

            // 保存
            await _nioPushRepository.InsertAsync(new NioPushEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SchemaCode = config.SchemaCode,
                BuzScene = buzSceneEnum,
                Status = PushStatusEnum.Wait,
                Content = new NIORequestDto<T>
                {
                    SchemaCode = config.SchemaCode,
                    List = data
                }.ToSerializeLower(),
                Remark = buzSceneEnum.GetDescription(),
                CreatedBy = user,
                CreatedOn = time,
                UpdatedBy = user,
                UpdatedOn = time
            });
        }

    }
}