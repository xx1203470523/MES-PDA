/*
 *creator: Karl
 *
 *describe: 条码打印    Dto | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-21 04:33:58
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Plan
{
    /// <summary>
    /// 条码打印Dto
    /// </summary>
    public record PlanSfcPrintDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

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
        /// SFC
        /// </summary>
        public string SFC { get; set; }


        /// <summary>
        /// 使用状态
        /// </summary>
        public PlanSFCUsedStatusEnum IsUsed { get; set; }


        /// <summary>
        /// 打印状态
        /// </summary>
        public PlanSFCPrintStatusEnum PrintStatus { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 工单数量
        /// </summary>
        public string Qty { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }


    }


    /// <summary>
    /// 条码打印新增Dto
    /// </summary>
    public record PlanSfcPrintCreateDto : BaseEntityDto
    {

        /// <summary>
        /// 条码
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 关联工单
        /// </summary>
        public long RelevanceWorkOrderId { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanSFCReceiveTypeEnum ReceiveType { get; set; }


    }

    /// <summary>
    /// 条码打印更新Dto
    /// </summary>
    public record PlanSfcPrintModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 状态;1：在制；2：完成；3：已入库；4：报废；
        /// </summary>
        public bool? Status { get; set; }

        /// <summary>
        /// 是否在用
        /// </summary>
        public long? IsUsed { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }



    }

    /// <summary>
    /// 条码打印分页Dto
    /// </summary>
    public class PlanSfcPrintPagedQueryDto : PagerInfo
    {
        ///// <summary>
        ///// 描述 :站点编码 
        ///// 空值 : false  
        ///// </summary>
        //public string SiteCode { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string? WorkOrderCode { get; set; }

        /// <summary>
        /// SFC 
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// 使用状态
        /// </summary>
        public long? IsUsed { get; set; } = 0;

        /// <summary>
        /// 打印状态
        /// </summary>
        public long? PrintStatus { get; set; } = 0;
    }
}
