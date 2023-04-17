﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// 包装容器状态枚举 ：1打开，2关闭
    /// </summary>
    public enum ManuContainerBarcodeStatusEnum :sbyte
    {
        /// <summary>
        /// 打开
        /// </summary>
        [Description("打开")]
        Open = 1,
        /// <summary>
        /// 关闭
        /// </summary>
        [Description("关闭")]
        Close = 2

    }
    /// <summary>
    /// 操作类型;1、装载2、移除
    /// </summary>
    public enum ManuContainerBarcodeOperateTypeEnum : sbyte
    {
        /// <summary>
        /// 打开
        /// </summary>
        [Description("装载")]
        Load = 1,
        /// <summary>
        /// 关闭
        /// </summary>
        [Description("移除")]
        Unload = 2

    }

}
