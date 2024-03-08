using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（IQC检验项目详情快照表）   
    /// qual_iqc_inspection_item_detail_snapshot
    /// @author User
    /// @date 2024-03-06 02:20:33
    /// </summary>
    public class QualIqcInspectionItemDetailSnapshotEntity : BaseEntity
    {
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
        /// 参数数据类型
        /// </summary>
        public bool ParameterDataType { get; set; }

        /// <summary>
        /// 项目类型;1、计量2、计数
        /// </summary>
        public bool? Type { get; set; }

        /// <summary>
        /// 检验器具
        /// </summary>
        public bool? Utensil { get; set; }

        /// <summary>
        /// 小数位数
        /// </summary>
        public int? Scale { get; set; }

        /// <summary>
        /// 规格下限
        /// </summary>
        public decimal? LowerLimit { get; set; }

        /// <summary>
        /// 规格中心
        /// </summary>
        public decimal? Center { get; set; }

        /// <summary>
        /// 规格上限
        /// </summary>
        public decimal? UpperLimit { get; set; }

        /// <summary>
        /// 检验类型;1、常规检验2、外观检验3、包装检验4、特殊性检验5、破坏性检验
        /// </summary>
        public bool? InspectionType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }


    }
}
