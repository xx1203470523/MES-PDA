/*
 *creator: Karl
 *
 *describe: 生产领料单    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  zhaoqing
 *build datetime: 2023-07-04 02:34:15
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 生产领料单，数据实体对象   
    /// manu_requistion_order
    /// @author zhaoqing
    /// @date 2023-07-04 02:34:15
    /// </summary>
    public class ManuRequistionOrderEntity : BaseEntity
    {
        /// <summary>
        /// 领料单据号
        /// </summary>
        public string ReqOrderCode { get; set; }

       /// <summary>
        /// 工单Code
        /// </summary>
        public string WorkOrderCode { get; set; }

        /// <summary>
        /// 领料单类型 0:工单领料 1:工单补料
        /// </summary>
        public ManuRequistionTypeEnum Type { get; set; }

       /// <summary>
        /// 状态
        /// </summary>
        public WhWarehouseRequistionStatusEnum? Status { get; set; }

       /// <summary>
        /// 物料描述
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}