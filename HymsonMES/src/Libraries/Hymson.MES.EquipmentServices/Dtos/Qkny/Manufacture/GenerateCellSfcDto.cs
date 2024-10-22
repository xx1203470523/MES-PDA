﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 电芯码下发
    /// </summary>
    public record GenerateCellSfcDto : QknyBaseDto
    {
        /// <summary>
        /// 产品型号
        /// </summary>
        public string ProductCode { get; set; } = "";

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; } = 10;
    }
}
