using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// FQC检验单创建Dto
    /// </summary>
    public record QualFqcOrderCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 成品条码产出记录Ids(qual_finally_output_record)
        /// </summary>
        public IEnumerable<long> OutputRecordIds { get; set; }
    }

    /// <summary>
    /// FQC检验单创建Dto
    /// </summary>
    public record QualFqcOrderCreateTestDto : BaseEntityDto
    {
        /// <summary>
        /// 产品Id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 产线Id
        /// </summary>
        public long? WorkCenterId { get; set; }

        /// <summary>
        /// 条码类型(1-托盘 2-栈板 3-SFC 4-箱)
        /// </summary>
        public FQCLotUnitEnum CodeType { get; set; }
    }

    /// <summary>
    /// FQC检验单新增/更新Dto
    /// </summary>
    public record QualFqcOrderSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 检验单号
        /// </summary>
        public string InspectionOrder { get; set; }

        /// <summary>
        /// FQC检验参数组快照Id
        /// </summary>
        public long GroupSnapshootId { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 样本数量
        /// </summary>
        public int SampleQty { get; set; }

        /// <summary>
        /// 状态(1-待检验 2-检验中 3-已检验 4-已关闭)
        /// </summary>
        public InspectionStatusEnum Status { get; set; }

        /// <summary>
        /// 是否合格(0-否 1-是)
        /// </summary>
        public TrueOrFalseEnum? IsQualified { get; set; }

        /// <summary>
        /// 是否为预生成单(0-否 1-是)
        /// </summary>
        public TrueOrFalseEnum IsPreGenerated { get; set; }

        /// <summary>
        /// 备注
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
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }


    }

    /// <summary>
    /// FQC检验单Dto
    /// </summary>
    public record QualFqcOrderDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 检验单号
        /// </summary>
        public string InspectionOrder { get; set; }

        /// <summary>
        /// FQC检验参数组快照Id
        /// </summary>
        public long GroupSnapshootId { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 样本数量
        /// </summary>
        public int SampleQty { get; set; }

        /// <summary>
        /// 状态(1-待检验 2-检验中 3-已检验 4-已关闭)
        /// </summary>
        public InspectionStatusEnum Status { get; set; }

        /// <summary>
        /// 是否合格(0-否 1-是)
        /// </summary>
        public TrueOrFalseEnum? IsQualified { get; set; }

        /// <summary>
        /// 是否为预生成单(0-否 1-是)
        /// </summary>
        public TrueOrFalseEnum? IsPreGenerated { get; set; }

        /// <summary>
        /// 备注
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
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

        /// <summary>
        /// 检验人
        /// </summary>
        public string InspectionBy { get; set; }

        /// <summary>
        /// 检验时间
        /// </summary>
        public DateTime? InspectionOn { get; set; }

        /// <summary>
        /// 不合格处理方式
        /// </summary>
        public HandMethodEnum? HandMethod { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>
        public string HandledBy { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? HandledOn { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specifications { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string MaterialVersion { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 状态;1、待检验2、检验中3、已检验4、已关闭
        /// </summary>
        public string StatusText { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public string IsQualifiedText { get; set; }

    }

    /// <summary>
    /// FQC检验单分页Dto
    /// </summary>
    public class QualFqcOrderPagedQueryDto : PagerInfo
    {

        /// <summary>
        /// 检验单号
        /// </summary>
        public string? InspectionOrder { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string? OrderCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string? MaterialName { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string? MaterialVersion { get; set; }

        /// <summary>
        /// 状态;1、待检验2、检验中3、已检验4、已关闭
        /// </summary>
        public InspectionStatusEnum? Status { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public TrueOrFalseEnum? IsQualified { get; set; }

        /// <summary>
        /// 不合格处理方式
        /// </summary>
        public FQCHandMethodEnum? HandMethod { get; set; }
    }


    /// <summary>
    /// 完成Dto
    /// </summary>
    public record QualFqcOrderCloseDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long FQCOrderId { get; set; }

        /// <summary>
        /// 不合格处理方式
        /// </summary>
        public HandMethodEnum HandMethod { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

    }


    /// <summary>
    /// 检验参数Dto
    /// </summary>
    public record FQCParameterDetailQueryDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long FQCOrderId { get; set; }

        /// <summary>
        /// 样品条码
        /// </summary>
        public string? Barcode { get; set; }

    }

    /// <summary>
    /// 检验参数Dto
    /// </summary>
    public record FQCParameterDetailDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 样本条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 参数Id proc_parameter 的id
        /// </summary>
        public long ParameterId { get; set; }

        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParameterCode { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// 参数单位
        /// </summary>
        public string ParameterUnit { get; set; }

        /// <summary>
        /// 参数数据类型
        /// </summary>
        public DataTypeEnum ParameterDataType { get; set; }

        /// <summary>
        /// 检验器具
        /// </summary>
        //public IQCUtensilTypeEnum Utensil { get; set; }

        /// <summary>
        /// 小数位数
        /// </summary>
        //public int? Scale { get; set; }

        /// <summary>
        /// 规格下限
        /// </summary>
        public decimal UpperLimit { get; set; }

        /// <summary>
        /// 参考值
        /// </summary>
        public string ReferenceValue { get; set; }

        /// <summary>
        /// 规格中心
        /// </summary>
        public decimal? CenterValue { get; set; }

        /// <summary>
        /// 输入次数
        /// </summary>
        public int EnterNumber { get; set; }

        /// <summary>
        /// 规格上限
        /// </summary>
        public decimal LowerLimit { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public TrueOrFalseEnum IsQualified { get; set; }

        /// <summary>
        /// 是否设备采集
        /// </summary>
        public TrueOrFalseEnum IsDeviceCollect { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 检验类型;1、常规检验2、外观检验3、包装检验4、特殊性检验5、破坏性检验
        /// </summary>
        public IQCInspectionTypeEnum InspectionType { get; set; }

        /// <summary>
        /// 检测值
        /// </summary>
        public string? InspectionValue { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 附件集合
        /// </summary>
        public IEnumerable<InteAttachmentBaseDto> Attachments { get; set; }

    }

    /// <summary>
    /// 检验参数Dto
    /// </summary>
    public class FQCParameterDetailPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long FQCOrderId { get; set; }

        /// <summary>
        /// 检验类型ID
        /// </summary>
        //public long? IQCOrderTypeId { get; set; }

        /// <summary>
        /// 样品条码
        /// </summary>
        public string? Barcode { get; set; }

        /// <summary>
        /// 项目编码
        /// </summary>
        public string? ParameterCode { get; set; }

    }

    /// <summary>
    /// 检验单状态Dto
    /// </summary>
    public record OrderOperationStatusDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public OrderOperateTypeEnum OperationType { get; set; }

    }


    /// <summary>
    ///检验单新增/更新Dto
    /// </summary>
    public record QualFqcOrderSampleSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long FQCOrderId { get; set; }

        /// <summary>
        /// 样品条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 样品参数
        /// </summary>
        public IEnumerable<QualFqcOrderParameterSaveDto> Details { get; set; }

    }

    public record QualFqcOrderParameterSaveDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 检验值
        /// </summary>
        public string? InspectionValue { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public TrueOrFalseEnum IsQualified { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 参数附件
        /// </summary>
        public IEnumerable<InteAttachmentBaseDto>? Attachments { get; set; }

    }


    /// <summary>
    /// FQC 已检更新明细Dto
    /// </summary>
    public record FQCParameterDetailSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public TrueOrFalseEnum IsQualified { get; set; }

        /// <summary>
        /// 检测值
        /// </summary>
        public string? InspectionValue { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 附件集合
        /// </summary>
        public IEnumerable<InteAttachmentBaseDto> Attachments { get; set; }

    }


    /// <summary>
    /// 完成Dto
    /// </summary>
    public record QualFqcOrderCompleteDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long FQCOrderId { get; set; }

    }

    /// <summary>
    /// 附件保存dto
    /// </summary>
    public record QualFqcOrderSaveAttachmentDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long FQCOrderId { get; set; }

        /// <summary>
        /// 检验单（附件）
        /// </summary>
        public IEnumerable<InteAttachmentBaseDto> Attachments { get; set; }

    }

    /// <summary>
    /// FQC检验参数组快照输出
    /// 执行检验查询
    /// </summary>
    public record QualFqcParameterGroupSnapshootOut
    {
        /// <summary>
        /// 参数集编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 样本数量
        /// </summary>
        public int SampleQty { get; set; }

        /// <summary>
        /// 已检数量
        /// </summary>
        public int CheckedQty { get; set; }
    }

    /// <summary>
    /// 不合格挑选输出
    /// </summary>
    public record FqcSelectionView : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// FQCId
        /// </summary>
        public long? FQCOrderId { get; set; }

        /// <summary>
        /// 样品条码
        /// </summary>
        public string? Barcode { get; set; }

        /// <summary>
        /// 检验单号
        /// </summary>
        public string? InspectionOrder { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string? OrderCode { get; set; }


        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string? MaterialName { get; set; }

        /// <summary>
        /// 物料单位
        /// </summary>
        public string? MaterialUnit { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string? MaterialVersion { get; set; }

        /// <summary>
        /// 更新人(操作人)
        /// </summary>
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间(操作时间)
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 处置状态
        /// </summary>
        public string? HandMethodText { get; set; }
    }

    /// <summary>
    /// 不合格挑选查询
    /// </summary>
    public class FqcSelectionQueryDto : PagerInfo
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long FQCOrderId { get; set; }

        /// <summary>
        /// 样品条码
        /// </summary>
        public string? Barcode { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string? OrderCode { get; set; }

        /// <summary>
        /// 检验单号
        /// </summary>
        public string? InspectionOrder { get; set; }

    }

    /// <summary>
    /// 不合格挑选-处理更新
    /// </summary>
    public record FqcSelectionUpdateDto
    {
        /// <summary>
        /// 处理类型
        /// </summary>
        public FQCSFCHandMethodSelectEnum? HandMethod { get; set; }

        /// <summary>
        /// 明细
        /// </summary>
        public IEnumerable<long>? DetailIds { get; set; }
    }
 

}
