﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Manufacture.ManuSFCScrap
{
    /// <summary>
    /// 
    /// </summary>
    public class PartialScrapScanningDto
    {
      public string BarCode { set; get; }
    }

    public class PartialScrapBarCodeDto
    {
        /// <summary>
        /// 条码信息
        /// </summary>
        public string BarCode { set; get; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderCode { set; get; }

        /// <summary>
        /// 产品信息编码
        /// </summary>
        public string ProductCode { set; get; }

        /// <summary>
        /// 产品信息名称
        /// </summary>
        public string ProductName { set; get; }

        /// <summary>
        /// 产品信息数量
        /// </summary>
        public decimal Qty { set; get; }
    }
}
