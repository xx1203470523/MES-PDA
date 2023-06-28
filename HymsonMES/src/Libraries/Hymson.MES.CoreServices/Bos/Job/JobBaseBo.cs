﻿using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Services.Job.JobUtility.Context;

namespace Hymson.MES.CoreServices.Bos.Job
{
    /// <summary>
    /// 作业公共类
    /// </summary>
    public class JobBaseBo: MultiSfcBo
    {
        /// <summary>
        /// 
        /// </summary>
        public IJobContextProxy Proxy { get; set; }
    }
}
