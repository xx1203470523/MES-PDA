﻿using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 
    /// </summary>
    public class ProcReplaceMaterialView : BaseEntity
    {
        /// <summary>
        /// 主物料ID
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 替代物料ID（已经把 ReplaceMaterialId 复制到ID列）
        /// </summary>
        public long ReplaceMaterialId { get; set; }

        /// <summary>
        /// 描述 :物料编码 
        /// 空值 : false  
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 描述 :物料名称 
        /// 空值 : false  
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 描述 :版本 
        /// 空值 : true  
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 数据收集方式 （等同于 DataCollectionWay ）
        /// </summary>
        public MaterialSerialNumberEnum? SerialNumber { get; set; }

    }
}
