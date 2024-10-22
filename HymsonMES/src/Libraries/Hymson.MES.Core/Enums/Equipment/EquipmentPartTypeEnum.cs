﻿using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 备件/工装类型枚举
    /// </summary>
    public enum EquipmentPartTypeEnum : sbyte
    {
        /// <summary>
        /// 备件
        /// </summary>
        [Description("备件")]
        SparePart = 1,
        /// <summary>
        /// 工装
        /// </summary>
        [Description("工装")]
        Consumable = 2
    }
}
