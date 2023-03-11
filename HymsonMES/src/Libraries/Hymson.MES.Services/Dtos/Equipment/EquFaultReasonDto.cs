/*
 *creator: pengxin
 *
 *describe: 设备故障原因表    Dto | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-02-28 15:15:20
 */

using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 设备故障原因表Dto
    /// </summary>
    public record EquFaultReasonDto : BaseEntityDto
    {
        //
        // 摘要:
        //     站点id
        public long? SiteId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 故障原因代码
        /// </summary>
        public string FaultReasonCode { get; set; }

        /// <summary>
        /// 故障原因名称
        /// </summary>
        public string FaultReasonName { get; set; }

        /// <summary>
        /// 故障原因状态（字典定义）
        /// </summary>
        public string UseStatus { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public bool? IsDeleted { get; set; }


    }

    /// <summary>
    /// 
    /// </summary>
    public record CustomEquFaultReasonDto : EquFaultReasonDto
    {

    }


    /// <summary>
    /// 设备故障原因表新增Dto
    /// </summary>
    public record EquFaultReasonCreateDto : BaseEntityDto
    {
        //
        // 摘要:
        //     站点id
        long? SiteId { get; }

        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 故障原因代码
        /// </summary>
        public string FaultReasonCode { get; set; } = "";

        /// <summary>
        /// 故障原因名称
        /// </summary>
        public string FaultReasonName { get; set; } = "";

        /// <summary>
        /// 故障原因状态（字典定义）
        /// </summary>
        public string UseStatus { get; set; } = "";

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; } = "";

    }

    /// <summary>
    /// 设备故障原因表更新Dto
    /// </summary>
    public record EquFaultReasonModifyDto : BaseEntityDto
    {
        //
        // 摘要:
        //     站点id
        long? SiteId { get; }

        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 故障原因代码
        /// </summary>
        public string FaultReasonCode { get; set; } = "";

        /// <summary>
        /// 故障原因名称
        /// </summary>
        public string FaultReasonName { get; set; } = "";

        /// <summary>
        /// 故障原因状态（字典定义）
        /// </summary>
        public string UseStatus { get; set; } = "";

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; } = "";

    }

    /// <summary>
    /// 设备故障原因表分页Dto
    /// </summary>
    public class EquFaultReasonPagedQueryDto : PagerInfo
    {
        ///// <summary>
        ///// 所属站点代码
        ///// </summary>
        //public long SiteId { get; set; }

        //
        // 摘要:
        //     站点id
        public long? SiteId { get; set; }

        /// <summary>
        /// 编码（设备故障原因）
        /// </summary>
        public string FaultReasonCode { get; set; } = "";

        /// <summary>
        /// 名称（设备故障原因）
        /// </summary>
        public string FaultReasonName { get; set; } = "";

        /// <summary>
        /// 描述（设备故障原因）
        /// </summary>
        public string Remark { get; set; } = "";
    }
}