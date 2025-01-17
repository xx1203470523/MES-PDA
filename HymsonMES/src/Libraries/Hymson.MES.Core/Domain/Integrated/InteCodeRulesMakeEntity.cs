using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 编码规则组成，数据实体对象   
    /// inte_code_rules_make
    /// @author Karl
    /// @date 2023-03-17 05:02:19
    /// </summary>
    public class InteCodeRulesMakeEntity : BaseEntity
    {
        /// <summary>
        /// 编码规则ID
        /// </summary>
        public long CodeRulesId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Seq { get; set; }

       /// <summary>
        /// 取值方式;1：固定值；2：可变值；
        /// </summary>
        public CodeValueTakingTypeEnum ValueTakingType { get; set; }

       /// <summary>
        /// 分段值
        /// </summary>
        public string SegmentedValue { get; set; }

        /// <summary>
        /// 自定义值
        /// </summary>
        public string CustomValue { get; set; }

       /// <summary>
       /// 描述
       /// </summary>
        public string Remark { get; set; } = "";

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
