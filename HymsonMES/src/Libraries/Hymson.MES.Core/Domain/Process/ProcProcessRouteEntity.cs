using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 工艺路线表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcProcessRouteEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }
        
        /// <summary>
        /// 描述 :工艺路线代码 
        /// 空值 : false  
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// 描述 :工艺路线名称 
        /// 空值 : false  
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 描述 :状态 
        /// 空值 : true  
        /// </summary>
        public string Status { get; set; }
        
        /// <summary>
        /// 描述 :类型 
        /// 空值 : true  
        /// </summary>
        public string Type { get; set; }
        
        /// <summary>
        /// 描述 :版本 
        /// 空值 : true  
        /// </summary>
        public string Version { get; set; }
        
        /// <summary>
        /// 描述 :是否当前版本 
        /// 空值 : true  
        /// </summary>
        public byte IsCurrentVersion { get; set; }
        
        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }
        }
}