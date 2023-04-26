﻿using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.QualUnqualifiedCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Report
{
    public record WorkshopJobControlReportViewDto:BaseEntityDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public SfcStatusEnum SFCStatus { get; set; }

        /// <summary>
        /// 物料编码/版本
        /// </summary>
        public string MaterialCodeVersion { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderTypeEnum OrderType { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// Bom编码/版本
        /// </summary>
        public string BomCodeVersion { get; set; }

        /// <summary>
        /// bom名称
        /// </summary>
        public string BomName { get; set; }

        /// <summary>
        /// 条码数量
        /// </summary>
        public decimal Qty { get; set; }
    }

    /// <summary>
    /// 车间作业控制 分页参数
    /// </summary>
    public class WorkshopJobControlReportPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string? MaterialVersion { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string? OrderCode { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public SfcStatusEnum? SFCStatus { get; set; }

        /// <summary>
        /// 条码在制状态
        /// </summary>
        public SfcProduceStatusEnum? SFCProduceStatus { get; set; }

        /// <summary>
        /// 是否锁
        /// </summary>
        public TrueOrFalseEnum? SFCIsLock { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string? ProcedureCode { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string? ResourceCode { get; set; }
    }

    public class WorkshopJobControlStepReportDto 
    {
        public string SFC { get; set; }

        public string OrderCode { get; set; }

        public string MaterialCodrNameVersion { get; set; }

        public string ProcessRouteCodeNameVersion { get; set; }

        public string ProcBomCodeNameVersion { get; set; }

        public List<WorkshopJobControlInOutSteptDto> WorkshopJobControlInOutSteptDtos { get; set; }=new List<WorkshopJobControlInOutSteptDto>();
    }

    public class WorkshopJobControlInOutSteptDto 
    {
        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 进站时间
        /// </summary>
        public DateTime InDateTime { get; set; }

        /// <summary>
        /// 出站时间
        /// </summary>
        public DateTime? OutDatetTime { get; set; }
    }

    /// <summary>
    /// 条码步骤表 分页参数
    /// </summary>
    public class ManuSfcStepBySFCPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string? SFC { get; set; }
    }

    public class ManuSfcStepBySFCViewDto 
    {
        /// <summary>
        /// sfc_step Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 作业名称  步骤类型
        /// </summary>
        public ManuSfcStepTypeEnum Operatetype { get; set; }

        /// <summary>
        /// 作业时间  创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 物料编码/版本
        /// </summary>
        public string MaterialCodeVersion { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set;}

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public string OrderCode { get; set;}
    }
}