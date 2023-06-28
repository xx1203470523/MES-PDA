﻿using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.CoreServices.Services.Job.JobUtility;

namespace Hymson.MES.CoreServices.Services.NewJob
{
    /// <summary>
    /// 条码转换
    /// </summary>
    [Job("条码转换", JobTypeEnum.Standard)]
    public class SfcConvertJobService : IJobService
    {
        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <param name="proxy"></param> 
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public async Task<TResult> DataAssemblingAsync<T, TResult>(T param) where T : JobBaseBo where TResult : JobResultBo, new()
        {
            await Task.CompletedTask;
            return new TResult();
        }

        /// <summary>
        /// 执行入库
        /// </summary>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public async Task ExecuteAsync()
        {
            await Task.CompletedTask;
        }

    }
}
