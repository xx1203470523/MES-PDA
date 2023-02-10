using Hymson.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 新增输入对象（设备注册）
    /// </summary>
    public record EquEquipmentCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 编码（设备注册）
        /// </summary>
        [Required(ErrorMessage = "编码不能为空")]
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 名称（设备注册）
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        public string EquipmentName { get; set; }

        /// <summary>
        /// 设备描述
        /// </summary>
        public string EquipmentDesc { get; set; }

        /// <summary>
        /// 工作中心工厂id
        /// </summary>
        public long? WorkCenterFactoryId { get; set; }

        /// <summary>
        /// 工作中心车间id
        /// </summary>
        public long? WorkCenterShopId { get; set; }

        /// <summary>
        /// 工作中心产线id
        /// </summary>
        public long? WorkCenterLineId { get; set; }

        /// <summary>
        /// 存放位置
        /// </summary>
        [Required(ErrorMessage = "存放位置不能为空")]
        public string Location { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public string EquipmentType { get; set; }

        /// <summary>
        /// 使用部门
        /// </summary>
        public string UseDepartment { get; set; }

        /// <summary>
        /// 入厂日期
        /// </summary>
        public string EntryDate { get; set; }

        /// <summary>
        /// 质保期限（月）
        /// </summary>
        public int QualTime { get; set; }

        /// <summary>
        /// 厂商
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public string Supplier { get; set; }

        /// <summary>
        /// 使用状态
        /// </summary>
        [Required(ErrorMessage = "使用状态不能为空")]
        public string UseStatus { get; set; }

        /// <summary>
        /// 功率
        /// </summary>
        public string Power { get; set; }

        /// <summary>
        /// 能耗等级
        /// </summary>
        public string EnergyLevel { get; set; }

        /// <summary>
        /// ip地址
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 节拍时间(秒)
        /// </summary>
        public int? TaktTime { get; set; }

        #region 子
        /// <summary>
        /// 设备关联硬件设备
        /// </summary>
        public List<EquEquipmentLinkHardwareCreateDto> HardwareLinks { get; set; }

        /// <summary>
        /// 设备关联Api
        /// </summary>
        public List<EquEquipmentLinkApiCreateDto> ApiLinks { get; set; }

        #endregion
    }

    /// <summary>
    /// 修改输入对象（设备注册）
    /// </summary>
    public record EquEquipmentModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 名称（设备注册）
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        public string EquipmentName { get; set; }

        /// <summary>
        /// 设备描述
        /// </summary>
        public string EquipmentDesc { get; set; }

        /// <summary>
        /// 工作中心工厂id
        /// </summary>
        public long? WorkCenterFactoryId { get; set; }

        /// <summary>
        /// 工作中心车间id
        /// </summary>
        public long? WorkCenterShopId { get; set; }

        /// <summary>
        /// 工作中心产线id
        /// </summary>
        public long? WorkCenterLineId { get; set; }

        /// <summary>
        /// 存放位置
        /// </summary>
        [Required(ErrorMessage = "存放位置不能为空")]
        public string Location { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public string EquipmentType { get; set; }

        /// <summary>
        /// 使用部门
        /// </summary>
        public string UseDepartment { get; set; }

        /// <summary>
        /// 入厂日期
        /// </summary>
        public string EntryDate { get; set; }

        /// <summary>
        /// 质保期限（月）
        /// </summary>
        public int QualTime { get; set; }

        /// <summary>
        /// 厂商
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public string Supplier { get; set; }

        /// <summary>
        /// 使用状态
        /// </summary>
        [Required(ErrorMessage = "使用状态不能为空")]
        public string UseStatus { get; set; }

        /// <summary>
        /// 功率
        /// </summary>
        public string Power { get; set; }

        /// <summary>
        /// 能耗等级
        /// </summary>
        public string EnergyLevel { get; set; }

        /// <summary>
        /// ip地址
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 节拍时间(秒)
        /// </summary>
        public int? TaktTime { get; set; }

        #region 子
        /// <summary>
        /// 设备关联硬件设备
        /// </summary>
        public List<EquEquipmentLinkHardwareModifyDto> HardwareLinks { get; set; }

        /// <summary>
        /// 设备关联Api
        /// </summary>
        public List<EquEquipmentLinkApiModifyDto> ApiLinks { get; set; }

        #endregion
    }

    /// <summary>
    /// 查询对象（设备注册）
    /// </summary>
    public class EquEquipmentPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 编码（设备注册）
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 名称（设备注册）
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 类型（设备注册）
        /// </summary>
        public string EquipmentType { get; set; }

        /// <summary>
        /// 使用状态（设备注册）
        /// </summary>
        public string UseStatus { get; set; }

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkCenterShopName { get; set; }

        /// <summary>
        /// 使用部门
        /// </summary>
        public string UseDepartment { get; set; }

        /// <summary>
        /// 存放位置
        /// </summary>
        public string Location { get; set; }
    }

    /// <summary>
    /// 自定义实体列表（设备注册）
    /// </summary>
    public record EquEquipmentListDto : BaseEntityDto
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 描述 :使用状态 
        /// 空值 : false  
        /// </summary>
        public string UseStatus { get; set; }

        /// <summary>
        /// 描述 :功率 
        /// 空值 : true  
        /// </summary>
        public string Power { get; set; }

        /// <summary>
        /// 描述 :能耗等级 
        /// 空值 : true  
        /// </summary>
        public string EnergyLevel { get; set; }

        /// <summary>
        /// 描述 :ip地址 
        /// 空值 : true  
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 描述 :备注 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }

        /// <summary>
        /// 描述 :节拍时间(秒) 
        /// 空值 : true  
        /// </summary>
        public int? TaktTime { get; set; }

        /// <summary>
        /// 描述 :设备编码 
        /// 空值 : false  
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 描述 :设备名称 
        /// 空值 : false  
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 描述 :设备组id 
        /// 空值 : false  
        /// </summary>
        public long EquipmentGroupId { get; set; }

        /// <summary>
        /// 描述 :设备描述 
        /// 空值 : true  
        /// </summary>
        public string EquipmentDesc { get; set; }

        /// <summary>
        /// 描述 :工作中心工厂id 
        /// 空值 : false  
        /// </summary>
        public long WorkCenterFactoryId { get; set; }

        /// <summary>
        /// 描述 :工作中心车间id 
        /// 空值 : true  
        /// </summary>
        public long? WorkCenterShopId { get; set; }

        /// <summary>
        /// 描述 :工作中心产线id 
        /// 空值 : true  
        /// </summary>
        public long? WorkCenterLineId { get; set; }

        /// <summary>
        /// 描述 :位置 
        /// 空值 : false  
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 描述 :设备类型 
        /// 空值 : true  
        /// </summary>
        public string EquipmentType { get; set; }

        /// <summary>
        /// 描述 :使用部门 
        /// 空值 : true  
        /// </summary>
        public string UseDepartment { get; set; }

        /// <summary>
        /// 描述 :入场日期 
        /// 空值 : true  
        /// </summary>
        public DateTime? EntryDate { get; set; }

        /// <summary>
        /// 描述 :质保期限（月） 
        /// 空值 : true  
        /// </summary>
        public int? QualTime { get; set; }

        /// <summary>
        /// 描述 :过期时间，根据进厂日期+质保日期得出 
        /// 空值 : true  
        /// </summary>
        public DateTime? ExpireDate { get; set; }

        /// <summary>
        /// 描述 :厂商 
        /// 空值 : true  
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// 描述 :供应商 
        /// 空值 : true  
        /// </summary>
        public string Supplier { get; set; }


        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkCenterShopName { get; set; } = "";
    }

    /// <summary>
    /// 自定义实体对象（设备注册）
    /// </summary>
    public record EquEquipmentDto : BaseEntityDto
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 描述 :使用状态 
        /// 空值 : false  
        /// </summary>
        public string UseStatus { get; set; }

        /// <summary>
        /// 描述 :功率 
        /// 空值 : true  
        /// </summary>
        public string Power { get; set; }

        /// <summary>
        /// 描述 :能耗等级 
        /// 空值 : true  
        /// </summary>
        public string EnergyLevel { get; set; }

        /// <summary>
        /// 描述 :ip地址 
        /// 空值 : true  
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 描述 :备注 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }

        /// <summary>
        /// 描述 :节拍时间(秒) 
        /// 空值 : true  
        /// </summary>
        public int? TaktTime { get; set; }

        /// <summary>
        /// 描述 :设备编码 
        /// 空值 : false  
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 描述 :设备名称 
        /// 空值 : false  
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 描述 :设备组id 
        /// 空值 : false  
        /// </summary>
        public long EquipmentGroupId { get; set; }

        /// <summary>
        /// 描述 :设备描述 
        /// 空值 : true  
        /// </summary>
        public string EquipmentDesc { get; set; }

        /// <summary>
        /// 描述 :工作中心工厂id 
        /// 空值 : false  
        /// </summary>
        public long WorkCenterFactoryId { get; set; }

        /// <summary>
        /// 描述 :工作中心车间id 
        /// 空值 : true  
        /// </summary>
        public long? WorkCenterShopId { get; set; }

        /// <summary>
        /// 描述 :工作中心产线id 
        /// 空值 : true  
        /// </summary>
        public long? WorkCenterLineId { get; set; }

        /// <summary>
        /// 描述 :位置 
        /// 空值 : false  
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 描述 :设备类型 
        /// 空值 : true  
        /// </summary>
        public string EquipmentType { get; set; }

        /// <summary>
        /// 描述 :使用部门 
        /// 空值 : true  
        /// </summary>
        public string UseDepartment { get; set; }

        /// <summary>
        /// 描述 :入场日期 
        /// 空值 : true  
        /// </summary>
        public DateTime? EntryDate { get; set; }

        /// <summary>
        /// 描述 :质保期限（月） 
        /// 空值 : true  
        /// </summary>
        public int? QualTime { get; set; }

        /// <summary>
        /// 描述 :过期时间，根据进厂日期+质保日期得出 
        /// 空值 : true  
        /// </summary>
        public DateTime? ExpireDate { get; set; }

        /// <summary>
        /// 描述 :厂商 
        /// 空值 : true  
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// 描述 :供应商 
        /// 空值 : true  
        /// </summary>
        public string Supplier { get; set; }


        /// <summary>
        /// 设备组名称
        /// </summary>
        public string EquipmentGroupName { get; set; } = "";
    }

    /// <summary>
    /// 自定义实体列表（设备注册）
    /// </summary>
    public record EquEquipmentBaseDto : BaseEntityDto
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 编码（设备注册）
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 名称（设备注册）
        /// </summary>
        public string EquipmentName { get; set; }
    }

    /// <summary>
    /// 自定义实体列表（设备注册）
    /// </summary>
    public class EquEquipmentDictionaryDto
    {
        /// <summary>
        /// 设备类型
        /// </summary>
        public string EquipmentType { get; set; } = "";

        /// <summary>
        /// 集合（设备注册）
        /// </summary>
        public IEnumerable<EquEquipmentBaseDto> Equipments { get; set; }
    }

}