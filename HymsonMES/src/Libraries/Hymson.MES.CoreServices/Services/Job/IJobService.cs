﻿using Hymson.MES.CoreServices.Bos.Job;

namespace Hymson.MES.CoreServices.Services.Job
{
    /// <summary>
    /// 作业模版
    /// </summary>
    public interface IJobService<T, TResult> where T : JobBaseBo
    {
        /// <summary>
        /// 参数校验
        /// </summary>
        Task VerifyParamAsync(T param);

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <returns></returns>
        Task<TResult> DataAssemblingAsync(T param);

        /// <summary>
        /// 执行入库
        /// </summary>
        /// <returns></returns>
        Task ExecuteAsync();

    }
}
