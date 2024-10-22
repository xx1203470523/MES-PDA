﻿using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Manufacture.ManuSfcOperateDto
{
    /// <summary>
    /// PDA条码进站条码列表查询Dto
    /// </summary>
    public class ManuSfcInstationPagedQueryDto: PagerInfo
    {
        ///// <summary>
        ///// 工单id
        ///// </summary>
        //public long WorkOrderId { get; set; }

        ///// <summary>
        ///// 资源Id
        ///// </summary>
        //public long? ResourceId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public SfcStatusEnum Status { get; set; }
    }

    /// <summary>
    /// PDA条码进站条码列表OutputDto
    /// </summary>
    public record ManuSfcInstationPagedQueryOutputDto {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string SFC { get; set; }

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
        /// 当前数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 状态;1：排队；2：活动；
        /// </summary>
        public SfcStatusEnum Status { get; set; }
        
    }

    /// <summary>
    /// PDA条码出站确认条码信息OutputDto
    /// </summary>
    public record ManuSfcOutstationConfirmSfcInfoOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 计划产出数量
        /// </summary>
        public decimal PlanOutputQty { get; set; }

        /// <summary>
        /// 不良数量
        /// </summary>
        public decimal UnqualifiedQty { get; set; }

        /// <summary>
        /// 良品数量
        /// </summary>
        public decimal QualifiedQty { get; set; }

    }
}
