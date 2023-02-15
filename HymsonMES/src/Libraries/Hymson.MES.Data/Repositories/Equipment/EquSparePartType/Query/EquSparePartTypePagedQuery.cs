using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Equipment.EquSparePartType.Query
{
    /// <summary>
    /// 备件类型 分页参数
    /// </summary>
    public class EquSparePartTypePagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public string SiteCode { get; set; } = "";

        /// <summary>
        /// 备件类型编码
        /// </summary>
        public string SparePartTypeCode { get; set; }

        /// <summary>
        /// 备件类型名称
        /// </summary>
        public string SparePartTypeName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; } = 0;
    }
}