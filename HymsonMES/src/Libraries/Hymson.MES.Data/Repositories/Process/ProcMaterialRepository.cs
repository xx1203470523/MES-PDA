/*
 *creator: Karl
 *
 *describe: 物料维护 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-08 04:47:44
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Process;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 物料维护仓储
    /// </summary>
    public partial class ProcMaterialRepository : IProcMaterialRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public ProcMaterialRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 删除（软删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeletesSql, new { ids=ids });

        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcMaterialView> GetByIdAsync(long id, string siteCode)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcMaterialView>(GetByIdSql, new { Id=id,siteCode= siteCode });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcMaterialEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcMaterialEntity>(GetByIdsSql, new { ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procMaterialPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcMaterialEntity>> GetPagedInfoAsync(ProcMaterialPagedQuery procMaterialPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");

            if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.SiteCode))
            {
                sqlBuilder.Where(" SiteCode=@SiteCode ");
            }
            if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.MaterialCode))
            {
                procMaterialPagedQuery.MaterialCode = $"%{procMaterialPagedQuery.MaterialCode}%";
                sqlBuilder.Where(" MaterialCode like @MaterialCode ");
            }
            if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.MaterialName))
            {
                procMaterialPagedQuery.MaterialName = $"%{procMaterialPagedQuery.MaterialName}%";
                sqlBuilder.Where(" MaterialName like %@MaterialName% ");
            }
            if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.Version))
            {
                procMaterialPagedQuery.Version = $"%{procMaterialPagedQuery.Version}%";
                sqlBuilder.Where(" Version like @Version ");
            }
            if (procMaterialPagedQuery.GroupId!=null)
            {
                sqlBuilder.Where(" GroupId = @GroupId ");
            }
            if (procMaterialPagedQuery.Status != null)
            {
                sqlBuilder.Where(" Status = @Status ");
            }
            if (procMaterialPagedQuery.Origin != null)
            {
                sqlBuilder.Where(" Origin = @Origin ");
            }

            var offSet = (procMaterialPagedQuery.PageIndex - 1) * procMaterialPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procMaterialPagedQuery.PageSize });
            sqlBuilder.AddParameters(procMaterialPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procMaterialEntitiesTask = conn.QueryAsync<ProcMaterialEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procMaterialEntities = await procMaterialEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcMaterialEntity>(procMaterialEntities, procMaterialPagedQuery.PageIndex, procMaterialPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procMaterialPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcMaterialEntity>> GetPagedInfoForGroupAsync(ProcMaterialPagedQuery procMaterialPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");

            if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.SiteCode))
            {
                sqlBuilder.Where(" SiteCode=@SiteCode ");
            }
            if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.MaterialCode))
            {
                procMaterialPagedQuery.MaterialCode = $"%{procMaterialPagedQuery.MaterialCode}%";
                sqlBuilder.Where(" MaterialCode like @MaterialCode ");
            }
            if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.MaterialName))
            {
                procMaterialPagedQuery.MaterialName = $"%{procMaterialPagedQuery.MaterialName}%";
                sqlBuilder.Where(" MaterialName like %@MaterialName% ");
            }
            if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.Version))
            {
                procMaterialPagedQuery.Version = $"%{procMaterialPagedQuery.Version}%";
                sqlBuilder.Where(" Version like @Version ");
            }
            if (procMaterialPagedQuery.GroupId != null)
            {
                if (procMaterialPagedQuery.GroupId == 0)
                {
                    //predicate = predicate.And(it => it.GroupId == 0);
                    sqlBuilder.Where(" GroupId = 0 ");
                }
                else
                {
                    sqlBuilder.Where(" ( GroupId = 0 or GroupId =@GroupId ) ");
                }

                sqlBuilder.Where(" GroupId = @GroupId ");
            }
            if (procMaterialPagedQuery.Status != null)
            {
                sqlBuilder.Where(" Status = @Status ");
            }
            if (procMaterialPagedQuery.Origin != null)
            {
                sqlBuilder.Where(" Origin = @Origin ");
            }

            var offSet = (procMaterialPagedQuery.PageIndex - 1) * procMaterialPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procMaterialPagedQuery.PageSize });
            sqlBuilder.AddParameters(procMaterialPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procMaterialEntitiesTask = conn.QueryAsync<ProcMaterialEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procMaterialEntities = await procMaterialEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcMaterialEntity>(procMaterialEntities, procMaterialPagedQuery.PageIndex, procMaterialPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procMaterialQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcMaterialEntity>> GetProcMaterialEntitiesAsync(ProcMaterialQuery procMaterialQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcMaterialEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");

            if (!string.IsNullOrWhiteSpace(procMaterialQuery.SiteCode))
            {
                sqlBuilder.Where(" SiteCode=@SiteCode ");
            }
            if (!string.IsNullOrWhiteSpace(procMaterialQuery.MaterialCode))
            {
                procMaterialQuery.MaterialCode = $"%{procMaterialQuery.MaterialCode}%";
                sqlBuilder.Where(" MaterialCode like @MaterialCode ");
            }
            if (!string.IsNullOrWhiteSpace(procMaterialQuery.Version))
            {
                procMaterialQuery.Version = $"%{procMaterialQuery.Version}%";
                sqlBuilder.Where(" Version like @Version ");
            }
            sqlBuilder.AddParameters(procMaterialQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procMaterialEntities = await conn.QueryAsync<ProcMaterialEntity>(template.RawSql, template.Parameters);
            return procMaterialEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procMaterialEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcMaterialEntity procMaterialEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, procMaterialEntity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procMaterialEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcMaterialEntity procMaterialEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procMaterialEntity);
        }

        /// <summary>
        /// 更新 同编码的其他物料设置为非当前版本
        /// </summary>
        /// <param name="procMaterialEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateSameMaterialCodeToNoVersionAsync(ProcMaterialEntity procMaterialEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSameMaterialCodeToNoVersionSql, procMaterialEntity);
        }
    }

    public partial class ProcMaterialRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_material` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `proc_material` /**where**/ ";
        const string GetProcMaterialEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_material` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_material`(  `Id`, `SiteCode`, `GroupId`, `MaterialCode`, `MaterialName`, `Status`, `Origin`, `Version`, `IsDefaultVersion`, `Remark`, `BuyType`, `ProcessRouteId`, `ProcedureBomId`, `Batch`, `Unit`, `SerialNumber`, `ValidationMaskGroup`, `BaseTime`, `ConsumptionTolerance`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @GroupId, @MaterialCode, @MaterialName, @Status, @Origin, @Version, @IsDefaultVersion, @Remark, @BuyType, @ProcessRouteId, @ProcedureBomId, @Batch, @Unit, @SerialNumber, @ValidationMaskGroup, @BaseTime, @ConsumptionTolerance, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_material` SET  GroupId = @GroupId, MaterialName = @MaterialName, Status = @Status, Origin = @Origin, Version = @Version, Remark = @Remark, BuyType = @BuyType, ProcessRouteId = @ProcessRouteId, ProcedureBomId = @ProcedureBomId, Batch = @Batch, Unit = @Unit, SerialNumber = @SerialNumber, BaseTime = @BaseTime, ConsumptionTolerance = @ConsumptionTolerance, IsDefaultVersion=@IsDefaultVersion, ValidationMaskGroup=@ValidationMaskGroup, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_material` SET IsDeleted = '1' WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_material` SET IsDeleted = '1' WHERE Id in @ids ";
        const string GetByIdSql = @"SELECT 
                                        g.`Id`,
                                        g.`SiteCode`,
                                        o.GroupName,
                                        g.MaterialCode,
                                        g.MaterialName,
                                        g.Status, 
                                        g.Origin, 
                                        g.Version, 
                                        g.Remark, 
                                        g.BuyType, 
                                        p.Id AS ProcessRouteId, 
                                        p.Name as ProcessRouteName,
                                        p.Version as ProcessRouteVersion, 
                                        q.Id as ProcedureBomId, 
                                        q.Name as ProcedureBomName, 
                                        '' as ProcedureBomVersion, 
                                        g.Batch as Batch, 
                                        g.Unit as Unit, 
                                        g.SerialNumber,
                                        g.ValidationMaskGroup,
                                        g.BaseTime,
                                        g.ConsumptionTolerance,
                                        g.IsDefaultVersion,
                                        g.UpdatedBy,
                                        g.UpdatedOn
                            FROM `proc_material` g 
                            LEFT JOIN proc_material_group o on o.Id=g.GroupId
                            LEFT JOIN proc_process_route p on g.ProcessRouteId = p.Id
                            LEFT JOIN proc_procedure_bom q on g.ProcedureBomId == q.Id 
                            WHERE g.Id = @Id and g.SiteCode=@SiteCode ";
        const string GetByIdsSql = @"SELECT 
                                        `Id`, `SiteCode`, `GroupId`, `MaterialCode`, `MaterialName`, `Status`, `Origin`, `Version`, `IsDefaultVersion`, `Remark`, `BuyType`, `ProcessRouteId`, `ProcedureBomId`, `Batch`, `Unit`, `SerialNumber`, `ValidationMaskGroup`, `BaseTime`, `ConsumptionTolerance`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_material`
                            WHERE Id IN @ids ";
        const string UpdateSameMaterialCodeToNoVersionSql = "UPDATE `proc_material` SET IsDefaultVersion= 0 WHERE MaterialCode= @MaterialCode ";
    }
}