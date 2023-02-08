/*
 *creator: Karl
 *
 *describe: 物料维护 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-07 11:16:51
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Process;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

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
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, idsArr);

        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcMaterialEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcMaterialEntity>(GetByIdSql, new { Id=id});
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
            //sqlBuilder.Select("*");

            //if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.SiteCode))
            //{
            //    sqlBuilder.Where("SiteCode=@SiteCode");
            //}
           
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
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procMaterialEntities = await conn.QueryAsync<ProcMaterialEntity>(template.RawSql, procMaterialQuery);
            return procMaterialEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procMaterialEntity"></param>
        /// <returns></returns>
        public async Task InsertAsync(ProcMaterialEntity procMaterialEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var id = await conn.ExecuteScalarAsync<long>(InsertSql, procMaterialEntity);
            procMaterialEntity.Id = id;
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
    }

    public partial class ProcMaterialRepository
    {
        const string GetPagedInfoDataSqlTemplate = "SELECT /**select**/ FROM `proc_material` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_material` /**where**/";
        const string GetProcMaterialEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_material` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_material`(`Id`, `SiteCode`, `GroupId`, `MaterialCode`, `MaterialName`, `Status`, `Origin`, `Version`, `IsDefaultVersion`, `Remark`, `BuyType`, `ProcessRouteId`, `ProcedureBomId`, `Batch`, `Unit`, `SerialNumber`, `ValidationMaskGroup`, `BaseTime`, `ConsumptionTolerance`, `CreateBy`, `CreateOn`, `UpdateBy`, `UpdateOn`, `IsDeleted`) VALUES (@Id, @SiteCode, @GroupId, @MaterialCode, @MaterialName, @Status, @Origin, @Version, @IsDefaultVersion, @Remark, @BuyType, @ProcessRouteId, @ProcedureBomId, @Batch, @Unit, @SerialNumber, @ValidationMaskGroup, @BaseTime, @ConsumptionTolerance, @CreateBy, @CreateOn, @UpdateBy, @UpdateOn, @IsDeleted ) ; ";
        const string UpdateSql = "UPDATE `proc_material` SET IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_material` SET IsDeleted = '1' WHERE Id = @Id ";
        const string GetByIdSql = @"SELECT 
                             `Id`, `SiteCode`, `GroupId`, `MaterialCode`, `MaterialName`, `Status`, `Origin`, `Version`, `IsDefaultVersion`, `Remark`, `BuyType`, `ProcessRouteId`, `ProcedureBomId`, `Batch`, `Unit`, `SerialNumber`, `ValidationMaskGroup`, `BaseTime`, `ConsumptionTolerance`, `CreateBy`, `CreateOn`, `UpdateBy`, `UpdateOn`, `IsDeleted`
                            FROM `proc_material`  WHERE Id = @Id ";
    }
}
