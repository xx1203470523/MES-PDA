﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.InBound
{
    /// <summary>
    /// 条码绑定
    /// </summary>
    public record SfcBindingDto : BaseDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; } = string.Empty;

        /// <summary>
        /// 条码明细
        /// </summary>
        public List<SfcBindingDetailDto> BindSfc { get; set; }
    }

    /// <summary>
    /// 条码绑定
    /// </summary>
    public record SfcBindingDetailDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; } = string.Empty;

        /// <summary>
        /// 位置
        /// </summary>
        public string Seq { get; set; } = string.Empty;

    }
}