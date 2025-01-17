using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.WhWarehouseShelf
{
    /// <summary>
    /// 货架新增/更新Dto
    /// </summary>
    public record WhWarehouseShelfSaveDto : BaseEntityDto
    {
        ///// <summary>
        ///// 
        ///// </summary>
        //public long Id { get; set; }

       /// <summary>
        /// 货架编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 货架名称
        /// </summary>
        public string Name { get; set; }

       ///// <summary>
       // /// 仓库id
       // /// </summary>
       // public long? WarehouseId { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string? WarehouseCode { get; set; }

        /// <summary>
        /// 库区编码
        /// </summary>
        public string? WarehouseRegionCode { get; set; }

        ///// <summary>
        ///// 库区id
        ///// </summary>
        //public long? WarehouseRegionId { get; set; }

       /// <summary>
        /// 库位行列
        /// </summary>
        public int Column { get; set; }

       /// <summary>
        /// 库位列数
        /// </summary>
        public int Row { get; set; }

       /// <summary>
        /// 状态;1、启用  2、未启用
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

       /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; }

       ///// <summary>
       // /// 创建人
       // /// </summary>
       // public string CreatedBy { get; set; }

       ///// <summary>
       // /// 创建时间
       // /// </summary>
       // public DateTime CreatedOn { get; set; }

       ///// <summary>
       // /// 最后修改人
       // /// </summary>
       // public string UpdatedBy { get; set; }

       ///// <summary>
       // /// 修改时间
       // /// </summary>
       // public DateTime UpdatedOn { get; set; }

       ///// <summary>
       // /// 是否逻辑删除
       // /// </summary>
       // public long IsDeleted { get; set; }

       ///// <summary>
       // /// 站点Id
       // /// </summary>
       // public long SiteId { get; set; }

       
    }

    /// <summary>
    /// 货架Dto
    /// </summary>
    public record WhWarehouseShelfDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 货架编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 货架名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 仓库id
        /// </summary>
        public long WarehouseId { get; set; }

        /// <summary>
        /// 库区id
        /// </summary>
        public long WarehouseRegionId { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string? WarehouseCode { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string? WarehouseName { get; set; }

        /// <summary>
        /// 库区编码
        /// </summary>
        public string? WarehouseRegionCode { get; set; }

        /// <summary>
        /// 货架编码
        /// </summary>
        public string? WarehouseShelfCode { get; set; }

        /// <summary>
        /// 库区名称
        /// </summary>
        public string? WarehouseRegionName { get; set; }

        /// <summary>
        /// 库位行列
        /// </summary>
        public int Column { get; set; }

       /// <summary>
        /// 库位列数
        /// </summary>
        public int Row { get; set; }

       /// <summary>
        /// 状态;1、启用  2、未启用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

       /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; } = "";

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public long IsDeleted { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }

    /// <summary>
    /// 货架分页Dto
    /// </summary>
    public class WhWarehouseShelfPagedQueryDto : PagerInfo {
        /// <summary>
        /// 货架编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 货架名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string? WareHouseCode { get; set; }

        /// <summary>
        /// 库区编码
        /// </summary>
        public string? WareHouseRegionCode { get; set; }

        /// <summary>
        /// 状态;1、启用  2、未启用
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }
    }

    /// <summary>
    /// 更新Dto
    /// </summary>
    public record WhWarehouseShelfModifyDto : BaseEntityDto {
        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 货架名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 库位行列
        /// </summary>
        public int? Column { get; set; }

        /// <summary>
        /// 库位列数
        /// </summary>
        public int? Row { get; set; }

        /// <summary>
        /// 状态;1、启用  2、未启用
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; }
    }
}
