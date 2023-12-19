﻿using Hymson.MES.Core.Enums.Job;

namespace Hymson.MES.Core.Attribute.Job
{
    /// <summary>
    /// 关联点
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class StartCorrelationAttribute : System.Attribute
    {
        public StartCorrelationAttribute(ConnectionTypeEnum connectionType)
        {
            this.ConnectionType = connectionType;
        }

        /// <summary>
        /// 关联类型
        /// </summary>
        public ConnectionTypeEnum ConnectionType { get; set; }
    }
}