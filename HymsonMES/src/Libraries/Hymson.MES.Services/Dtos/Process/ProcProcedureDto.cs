/*
 *creator: Karl
 *
 *describe: 工序表    Dto | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-13 09:06:05
 */

using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 工序表Dto
    /// </summary>
    public record ProcProcedureDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 工序BOM代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 工序BOM名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 包装等级（字典数据）
        /// </summary>
        public string PackingLevel { get; set; }

        /// <summary>
        /// 所属资源类型ID
        /// </summary>
        public long? ResourceTypeId { get; set; }

        /// <summary>
        /// 循环次数
        /// </summary>
        public int? Cycle { get; set; }

        /// <summary>
        /// 是否维修返回
        /// </summary>
        public bool? IsRepairReturn { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

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
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public bool? IsDeleted { get; set; }


    }

    public record ProcProcedureOperDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 工序BOM代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 工序BOM名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 包装等级（字典数据）
        /// </summary>
        public string PackingLevel { get; set; }

        /// <summary>
        /// 所属资源类型ID
        /// </summary>
        public long? ResourceTypeId { get; set; }

        /// <summary>
        /// 循环次数
        /// </summary>
        public int? Cycle { get; set; }

        /// <summary>
        /// 是否维修返回
        /// </summary>
        public bool? IsRepairReturn { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 工序表分页查询Dto
    /// </summary>
    public class ProcProcedurePagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 工序编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// 描述 :资源类型名称 
        /// </summary>
        public string? ResTypeName { get; set; }
    }

    /// <summary>
    /// 分页查询返回实体
    /// </summary>
    public record ProcProcedureViewDto : ProcProcedureDto
    {
        /// <summary>
        /// 资源类型
        /// </summary>
        public string ResType { get; set; }

        /// <summary>
        /// 资源类型名称
        /// </summary>
        public string ResTypeName { get; set; }
    }

    public class QueryProcProcedureDto
    {
        /// <summary>
        /// 工序信息
        /// @author wangkeming
        /// @date 2022-09-30
        /// </summary>
        public ProcProcedureDto Procedure { get; set; }

        /// <summary>
        /// 资源类型
        /// </summary>
        public ProcResourceTypeDto ResourceType { get; set; }
    }

    /// <summary>
    /// 工序表新增Dto
    /// </summary>
    public record ProcProcedureCreateDto 
    {
        /// <summary>
        /// 工序信息
        /// </summary>
        public ProcProcedureOperDto Procedure { get; set; }

        /// <summary>
        /// 工序打印配置信息
        /// </summary>

       // public List<ProcProcedureBomConfigPrintDto> ProcedurePrintList { get; set; }

        /// <summary>
        /// 工序工作配置信息
        /// </summary>

       // public List<ProcProcedureBomConfigJobDto> ProcedureJobList { get; set; }
    }

    /// <summary>
    /// 工序表更新Dto
    /// </summary>
    public record ProcProcedureModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 工序BOM代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 工序BOM名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 包装等级（字典数据）
        /// </summary>
        public string PackingLevel { get; set; }

        /// <summary>
        /// 所属资源类型ID
        /// </summary>
        public long? ResourceTypeId { get; set; }

        /// <summary>
        /// 循环次数
        /// </summary>
        public int? Cycle { get; set; }

        /// <summary>
        /// 是否维修返回
        /// </summary>
        public bool? IsRepairReturn { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

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
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public bool? IsDeleted { get; set; }
    }
}