using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// OQC检验参数组明细快照新增/更新Dto
    /// </summary>
    public record QualOqcParameterGroupDetailSnapshootSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// OQC检验参数组Id
        /// </summary>
        public int ParameterGroupId { get; set; }

        /// <summary>
        /// 标准参数Id
        /// </summary>
        public int ParameterId { get; set; }

        /// <summary>
        /// 规格上限
        /// </summary>
        public decimal? UpperLimit { get; set; }

        /// <summary>
        /// 中心值（均值）
        /// </summary>
        public decimal? CenterValue { get; set; }

        /// <summary>
        /// 规格下限
        /// </summary>
        public decimal? LowerLimit { get; set; }

        /// <summary>
        /// 参考值
        /// </summary>
        public string ReferenceValue { get; set; }

        /// <summary>
        /// 检验类型
        /// </summary>
        public bool InspectionType { get; set; }

        /// <summary>
        /// 录入次数
        /// </summary>
        public int EnterNumber { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }

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
        public int? IsDeleted { get; set; }


    }

    /// <summary>
    /// OQC检验参数组明细快照Dto
    /// </summary>
    public record QualOqcParameterGroupDetailSnapshootDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// OQC检验参数组Id
        /// </summary>
        public int ParameterGroupId { get; set; }

        /// <summary>
        /// 标准参数Id
        /// </summary>
        public int ParameterId { get; set; }

        /// <summary>
        /// 规格上限
        /// </summary>
        public decimal? UpperLimit { get; set; }

        /// <summary>
        /// 中心值（均值）
        /// </summary>
        public decimal? CenterValue { get; set; }

        /// <summary>
        /// 规格下限
        /// </summary>
        public decimal? LowerLimit { get; set; }

        /// <summary>
        /// 参考值
        /// </summary>
        public string ReferenceValue { get; set; }

        /// <summary>
        /// 检验类型
        /// </summary>
        public bool InspectionType { get; set; }

        /// <summary>
        /// 录入次数
        /// </summary>
        public int EnterNumber { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }

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
        public int? IsDeleted { get; set; }


    }

    /// <summary>
    /// OQC检验参数组明细快照分页Dto
    /// </summary>
    public class QualOqcParameterGroupDetailSnapshootPagedQueryDto : PagerInfo { }

}
