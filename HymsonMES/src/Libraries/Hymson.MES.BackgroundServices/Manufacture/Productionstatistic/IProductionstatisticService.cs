﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Manufacture.Productionstatistic
{
    public interface IProductionstatisticService
    {
        /// <summary>
        /// 执行统计
        /// </summary>
        /// <returns></returns>
        Task ExecuteAsync();
    }
}