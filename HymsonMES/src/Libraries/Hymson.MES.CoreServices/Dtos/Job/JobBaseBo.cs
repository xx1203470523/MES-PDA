﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Dtos.Job
{
    /// <summary>
    /// 作业公共类
    /// </summary>
    public class JobBaseBo
    {
        /// <summary>
        /// 工厂Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码集合
        /// </summary>
        public IEnumerable<string> SFCs { get; set; } = new List<string>();
    }
}