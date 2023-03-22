/*
 *creator: Karl
 *
 *describe: 条码流转表    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  zhaoqing
 *build datetime: 2023-03-18 05:40:18
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 条码流转表，数据实体对象   
    /// manu_sfc_circulation
    /// @author zhaoqing
    /// @date 2023-03-18 05:40:18
    /// </summary>
    public class ManuSfcCirculationEntity : BaseEntity
    {
        /// <summary>
        /// 当前工序id
        /// </summary>
        public long ProcedureId { get; set; }

       /// <summary>
        /// 资源id
        /// </summary>
        public long? ResourceId { get; set; }

       /// <summary>
        /// 设备id
        /// </summary>
        public long? EquipmentId { get; set; }

       /// <summary>
        /// 流转前条码
        /// </summary>
        public string Sfc { get; set; }

       /// <summary>
        /// 流转前工单id
        /// </summary>
        public long? WorkOrderId { get; set; }

       /// <summary>
        /// 流转前产品id
        /// </summary>
        public long? ProductId { get; set; }

       /// <summary>
        /// 流转后条码信息
        /// </summary>
        public long CirculationBarCode { get; set; }

       /// <summary>
        /// 流转后工单id
        /// </summary>
        public long CirculationWorkOrderId { get; set; }

       /// <summary>
        /// 流转后产品id
        /// </summary>
        public long CirculationProductId { get; set; }

       /// <summary>
        /// 流转类型;1：拆分；2：合并；3：转换；
        /// </summary>
        public string Type { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}