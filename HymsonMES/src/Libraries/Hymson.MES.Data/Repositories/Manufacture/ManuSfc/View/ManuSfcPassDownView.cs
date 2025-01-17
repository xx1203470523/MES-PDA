﻿using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    public class ManuSfcPassDownView
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 产品条码
        /// </summary>
        public string SFC { get; set; }
        /// <summary>
        /// 使用状态
        /// </summary>
        public YesOrNoEnum IsUsed { get; set; }

        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 采购类型 
        /// </summary>
        public MaterialBuyTypeEnum BuyType { get; set; }

    }
}
