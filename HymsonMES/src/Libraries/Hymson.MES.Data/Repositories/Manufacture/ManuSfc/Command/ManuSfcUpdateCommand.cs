﻿using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command
{
    /// <summary>
    /// 条码状态更新
    /// </summary>
    public class ManuSfcUpdateCommand
    {
        /// <summary>
        /// 产品条码列表
        /// </summary>
        public string[] Sfcs { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public SfcStatusEnum? Status { get; set; }
    }

    /// <summary>
    /// 条码状态更新 
    /// </summary>
    public class MultiSFCUpdateStatusCommand : UpdateCommand
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public SfcStatusEnum? Status { get; set; }

        /// <summary>
        /// 产品条码列表
        /// </summary>
        public IEnumerable<string> SFCs { get; set; }
    }

    /// <summary>
    /// 条码状态更新 
    /// </summary>
    public class ManuSfcUpdateStatusAndIsUsedCommand : ManuSfcUpdateCommand
    {
        /// <summary>
        /// 是否使用
        /// </summary>
        public YesOrNoEnum IsUsed { get; set; }
    }

    public class ManuSfcUpdateRouteCommand : UpdateCommand
    {
        /// <summary>
        /// 条码在制品id列表
        /// </summary>
        public long[] Ids { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public long ProcessRouteId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 状态;1：排队；2：活动；
        /// </summary>
        public SfcStatusEnum Status { get; set; }
    }

    /// <summary>
    /// 更新在制品表工艺路线 工序为在制
    /// </summary>
    public class ManuSfcUpdateRouteByIdCommand : UpdateCommand
    {
        /// <summary>
        /// 条码在制品id列表
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public long ProcessRouteId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 状态;1：排队；2：活动；
        /// </summary>
        public SfcStatusEnum Status { get; set; }

        /// <summary>
        /// 是否维修
        /// </summary>
        public TrueOrFalseEnum IsRepair { get; set; }
    }

    /// <summary>
    /// 条码状态更新
    /// </summary>
    public class ManuSfcUpdateStatusCommand
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 产品条码列表
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public SfcStatusEnum? Status { get; set; }
    }

    /// <summary>
    /// 更具条码Id 更新条码状态
    /// </summary>
    public class ManuSfcUpdateStatusByIdsCommand : UpdateCommand
    {
        /// <summary>
        /// 条码在制品id列表
        /// </summary>
        public IEnumerable<long> Ids { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public SfcStatusEnum Status { get; set; }
    }

    /// <summary>
    /// 更具条码Id 更新条码状态
    /// </summary>
    public class ManuSfcUpdateStatusByIdCommand : UpdateCommand
    {
        /// <summary>
        /// 条码在制品id列表
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public SfcStatusEnum Status { get; set; }

        /// <summary>
        /// 条码当前状态
        /// </summary>
        public SfcStatusEnum CurrentStatus { get; set; }
    }

    /// <summary>
    /// 清空复投次数
    /// </summary>
    public class CleanRepeatedCountCommand : UpdateCommand
    {
        /// <summary>
        /// 条码在制品id列表
        /// </summary>
        public long Id { get; set; }
    }
}
