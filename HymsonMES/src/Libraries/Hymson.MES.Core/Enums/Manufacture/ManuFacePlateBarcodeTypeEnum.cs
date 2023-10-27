﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Manufacture
{
    public enum ManuFacePlateBarcodeTypeEnum
    {
        /// <summary>
        /// 产品序列码
        /// </summary>
        [Description("产品序列码")]
        Product = 0,
        /// <summary>
        /// 载具编码
        /// </summary>
        [Description("载具编码")]
        Vehicle = 1,
    }
}