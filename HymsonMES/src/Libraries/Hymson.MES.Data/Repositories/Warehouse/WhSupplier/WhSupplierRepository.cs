/*
 *creator: Karl
 *
 *describe: 供应商 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-03 01:51:43
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Warehouse
{
    /// <summary>
    /// 供应商仓储
    /// </summary>
    public partial class WhSupplierRepository : IWhSupplierRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public WhSupplierRepository(IOptions<ConnectionOptions> connectionOptions)
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
        public async Task<int> DeletesAsync(DeleteCommand param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeletesSql, param);

        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WhSupplierEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<WhSupplierEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WhSupplierEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<WhSupplierEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="whSupplierPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WhSupplierEntity>> GetPagedInfoAsync(WhSupplierPagedQuery whSupplierPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Select("*");

            if (!string.IsNullOrWhiteSpace(whSupplierPagedQuery.Code))
            {
                //whSupplierPagedQuery.Code = $"%{whSupplierPagedQuery.Code}%";
                sqlBuilder.Where("Code=@Code");
            }
            if (!string.IsNullOrWhiteSpace(whSupplierPagedQuery.Name))
            {
                //whSupplierPagedQuery.Name = $"%{whSupplierPagedQuery.Name}%";
                sqlBuilder.Where("Name=@Name");
            }


            var offSet = (whSupplierPagedQuery.PageIndex - 1) * whSupplierPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = whSupplierPagedQuery.PageSize });
            sqlBuilder.AddParameters(whSupplierPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var whSupplierEntitiesTask = conn.QueryAsync<WhSupplierEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var whSupplierEntities = await whSupplierEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<WhSupplierEntity>(whSupplierEntities, whSupplierPagedQuery.PageIndex, whSupplierPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="whSupplierQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WhSupplierEntity>> GetWhSupplierEntitiesAsync(WhSupplierQuery whSupplierQuery)
        {
            try
            {
                var sqlBuilder = new SqlBuilder();
                var template = sqlBuilder.AddTemplate(GetWhSupplierEntitiesSqlTemplate);
                sqlBuilder.Select("*");
                if (!string.IsNullOrWhiteSpace(whSupplierQuery.Code))
                {
                    sqlBuilder.Where("Code=@Code");
                }
                if (!string.IsNullOrWhiteSpace(whSupplierQuery.Name))
                {
                    sqlBuilder.Where("Name=@Name");
                }

                using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
                var whSupplierEntities = await conn.QueryAsync<WhSupplierEntity>(template.RawSql, whSupplierQuery);
                return whSupplierEntities;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="whSupplierEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(WhSupplierEntity whSupplierEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, whSupplierEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="whSupplierEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<WhSupplierEntity> whSupplierEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertsSql, whSupplierEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="whSupplierEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(WhSupplierEntity whSupplierEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, whSupplierEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="whSupplierEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<WhSupplierEntity> whSupplierEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdatesSql, whSupplierEntitys);
        }

    }

    public partial class WhSupplierRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `wh_supplier` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `wh_supplier` /**where**/ ";
        const string GetWhSupplierEntitiesSqlTemplate = @"SELECT  
                                             /**select**/
                                            FROM  `wh_supplier`  /**where**/   ";

        const string InsertSql = "INSERT INTO `wh_supplier`(  `Id`, `Code`, `Name`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @Code, @Name, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string InsertsSql = "INSERT INTO `wh_supplier`(  `Id`, `Code`, `Name`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @Code, @Name, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        //const string UpdateSql = "UPDATE `wh_supplier` SET    Name = @Name, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";
        const string UpdateSql = "UPDATE `wh_supplier` SET    Name = @Name, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `wh_supplier` SET   Name = @Name, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `wh_supplier` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `wh_supplier` SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids";
        const string GetByIdSql = @"SELECT 
                               `Id`, `Code`, `Name`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `wh_supplier`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `Code`, `Name`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `wh_supplier`  WHERE Id IN @ids "
        ;
    }
}
