﻿using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Services.Dtos.Report
{
    public class PackagingQueryDto
    {
        /// <summary>
        /// 条码信息（容器条码、装载条码、工单号)
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 查询类型
        /// </summary>
        public PackagingTypeEnum? Type { get; set; }
    }

    /// <summary>
    /// 容器
    /// </summary>
    public record ManuContainerBarcodeViewDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 状态;1：打开 2：关闭
        /// </summary>
        public ManuContainerBarcodeStatusEnum Status { get; set; }

        /// <summary>
        /// 包装等级;1：一级；2：二级；3：三级；
        /// </summary>
        public int? Level { get; set; }

        /// <summary>
        /// 当前装载数量
        /// </summary>
        public int PackQuantity{get;set;}
    }

    public record PlanWorkPackDto: BaseEntityDto
    {
        /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string MaterialVersion { get; set; }

        /// <summary>
        /// 包装等级;1：一级；2：二级；3：三级；
        /// </summary>
        public LevelEnum? Level { get; set; }
    }

    public record PlanWorkPackingDto :BaseEntityDto
    {
        /// <summary>
        /// 父级容器编码
        /// </summary>
        public string ParentContainerCode { get; set; }

        /// <summary>
        /// 容器条码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 容器id
        /// </summary>
        public long ContainerBarCodeId { get; set; }

        /// <summary>
        /// 当前装载数量
        /// </summary>
        public int PackQuantity { get; set; }

        /// <summary>
        /// 状态;1：打开 2：关闭
        /// </summary>
        public ManuContainerBarcodeStatusEnum Status { get; set; } = ManuContainerBarcodeStatusEnum.Open;

        /// <summary>
        /// 包装等级;1：一级；2：二级；3：三级；
        /// </summary>
        public int PackLevel { get; set; }

        /// <summary>
        /// 包装人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 包装时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }
}
