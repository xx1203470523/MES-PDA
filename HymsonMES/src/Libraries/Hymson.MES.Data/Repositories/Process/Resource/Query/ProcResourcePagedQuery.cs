﻿using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Process.Resource
{
    /// <summary>
    /// 资源维护表查询对象
    /// </summary>
    public class ProcResourcePagedQuery : PagerInfo
    {
        /// <summary>
        /// 描述 :资源代码 
        /// 空值 : false  
        /// </summary>
        public string ResCode { get; set; }

        /// <summary>
        /// 描述 :资源名称 
        /// 空值 : false  
        /// </summary>
        public string ResName { get; set; }

        /// <summary>
        /// 资源类型
        /// </summary>
        public string ResType { get; set; }

        /// <summary>
        /// 资源类型id
        /// </summary>
        public long? ResTypeId { get; set; }

        /// <summary>
        /// 描述 :状态 
        /// 空值 : false  
        /// </summary>
        public string Status { get; set; }

        //站点
        public string SiteCode { get; set; }
    }

    public class ProcResourceQuery
    {
        /// <summary>
        /// 站点
        /// </summary>
        public string SiteCode { get; set; }

        public long[] IdsArr { get; set; }

        /// <summary>
        /// 描述 :状态 
        /// 空值 : false  
        /// </summary>
        public string Status { get; set; }
    }
}