/*
 *creator: Karl
 *
 *describe: 物料维护    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-07 11:16:51
 */

using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Process
{
	/// <summary>
    /// 物料维护Dto
    /// </summary>
    public record ProcMaterialDto : BaseEntityDto
    {
        ///// <summary>
        ///// 描述 :站点编码 
        ///// 空值 : false  
        ///// </summary>
        //public string SiteCode { get; set; }
        
    }

    /// <summary>
    /// {entityCNName}新增Dto
    /// </summary>
    public record ProcMaterialCreateDto : BaseEntityDto
    {

    }

    /// <summary>
    /// {entityCNName}更新Dto
    /// </summary>
    public record ProcMaterialModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public long Id { get; set; }

    }

    /// <summary>
    /// 物料维护分页Dto
    /// </summary>
    public class ProcMaterialPagedQueryDto : PagerInfo
    {
        ///// <summary>
        ///// 描述 :站点编码 
        ///// 空值 : false  
        ///// </summary>
        //public string SiteCode { get; set; }
    }
}
