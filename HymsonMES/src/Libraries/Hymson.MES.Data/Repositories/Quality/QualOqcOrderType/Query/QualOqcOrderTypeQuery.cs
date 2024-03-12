namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// OQC检验类型 查询参数
    /// </summary>
    public class QualOqcOrderTypeQuery
    {
        /// <summary>
        /// OQC检验单Id
        /// </summary>
        public long? OQCOrderId { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long? SiteId { get; set; }
    }
}
