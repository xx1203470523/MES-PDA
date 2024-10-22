using Hymson.Infrastructure;
using OfficeOpenXml.Attributes;

namespace Hymson.MES.Services.Dtos.Warehouse
{
    /// <summary>
    /// 供应商Dto
    /// </summary>
    public record WhSupplierDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
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
        /// 更新人
        /// </summary>
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

    }

    /// <summary>
    /// 更改供应商Dto
    /// </summary>
    public record UpdateWhSupplierDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";
    }

    /// <summary>
    /// 供应商新增Dto
    /// </summary>
    public record WhSupplierCreateDto : BaseEntityDto
    {
        ///// <summary>
        ///// 主键id
        ///// </summary>

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

    }

    /// <summary>
    /// 供应商更新Dto
    /// </summary>
    public record WhSupplierModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

    }

    /// <summary>
    /// 供应商分页Dto
    /// </summary>
    public class WhSupplierPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 描述 : 供应商编号
        /// 空值 : true  
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 描述 :供应商名称 
        /// 空值 : true  
        /// </summary>
        public string? Name { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WhSupplierExportResultDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FileName { get; set; }

    }

    /// <summary>
    /// 供应商导入模板
    /// </summary>
    public record WhSupplierImportDto : BaseExcelDto
    {
        /// <summary>
        /// 编码（标准参数）
        /// </summary>
        [EpplusTableColumn(Header = "供应商编码(必填)", Order = 1)]
        public string Code { get; set; }

        /// <summary>
        /// 名称（标准参数）
        /// </summary>
        [EpplusTableColumn(Header = "供应商名称(必填)", Order = 2)]
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [EpplusTableColumn(Header = "备注", Order = 3)]
        public string? Remark { get; set; }

    }

    /// <summary>
    /// 供应商导出
    /// </summary>
    public record WhSupplierExportDto : BaseExcelDto
    {
        /// <summary>
        /// 编码（标准参数）
        /// </summary>
        [EpplusTableColumn(Header = "供应商编码(必填)", Order = 1)]
        public string Code { get; set; }

        /// <summary>
        /// 名称（标准参数）
        /// </summary>
        [EpplusTableColumn(Header = "供应商名称(必填)", Order = 2)]
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [EpplusTableColumn(Header = "备注", Order = 3)]
        public string? Remark { get; set; }

    }
}
