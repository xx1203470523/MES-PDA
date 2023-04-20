/*
 *creator: Karl
 *
 *describe: 容器装载表（物理删除） 仓储类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-04-12 02:33:13
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 容器装载表（物理删除）仓储
    /// </summary>
    public partial class ManuContainerPackRepository : BaseRepository, IManuContainerPackRepository
    {

        public ManuContainerPackRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

        #region 方法
        /// <summary>
        /// 删除（软删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand param)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, param);
        }

        /// <summary>
        /// 根据容器Id 删除所有容器装载记录（物理删除）
        /// </summary>
        /// <param name="containerBarCodeId"></param>
        /// <returns></returns>
        public async Task<int> DeleteAllAsync(long containerBarCodeId)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteAllSql, new { ContainerBarCodeId = containerBarCodeId });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuContainerPackEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuContainerPackEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuContainerPackEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuContainerPackEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuContainerPackPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuContainerPackView>> GetPagedInfoAsync(ManuContainerPackPagedQuery manuContainerPackPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("pack.IsDeleted=0");
            sqlBuilder.Select("pack.Id,pack.SiteId,pack.ContainerBarCodeId,barcode.BarCode,barcode.ProductId,pack.LadeBarCode");
            sqlBuilder.LeftJoin("manu_container_barcode barcode on barcode.Id =pack.ContainerBarCodeId and barcode.IsDeleted=0");
            if (!string.IsNullOrWhiteSpace(manuContainerPackPagedQuery.BarCode))
            {
                sqlBuilder.Where("barcode.BarCode=@BarCode");
            }
            if (manuContainerPackPagedQuery.BarCodeId.HasValue)
            {
                sqlBuilder.Where("barcode.Id=@BarCodeId");
            }

            var offSet = (manuContainerPackPagedQuery.PageIndex - 1) * manuContainerPackPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuContainerPackPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuContainerPackPagedQuery);

            using var conn = GetMESDbConnection();
            var manuContainerPackEntitiesTask = conn.QueryAsync<ManuContainerPackView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuContainerPackEntities = await manuContainerPackEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuContainerPackView>(manuContainerPackEntities, manuContainerPackPagedQuery.PageIndex, manuContainerPackPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuContainerPackQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuContainerPackEntity>> GetManuContainerPackEntitiesAsync(ManuContainerPackQuery manuContainerPackQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuContainerPackEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var manuContainerPackEntities = await conn.QueryAsync<ManuContainerPackEntity>(template.RawSql, manuContainerPackQuery);
            return manuContainerPackEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuContainerPackEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuContainerPackEntity manuContainerPackEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuContainerPackEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuContainerPackEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuContainerPackEntity> manuContainerPackEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuContainerPackEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuContainerPackEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuContainerPackEntity manuContainerPackEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuContainerPackEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuContainerPackEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuContainerPackEntity> manuContainerPackEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuContainerPackEntitys);
        }

        public async Task<ManuContainerPackEntity> GetByLadeBarCodeAsync(string sfc)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuContainerPackEntity>(GetBysfcSql, new { SFC = sfc });
        }

        public async Task<IEnumerable<ManuContainerPackEntity>> GetByContainerBarCodeIdAsync(long cid)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuContainerPackEntity>(GetByPackcodeSql, new { ContainerBarCodeId = cid });
        }
        #endregion

    }

    public partial class ManuContainerPackRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_container_pack` pack /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_container_pack` pack /**leftjoin**/  /**where**/ ";
        const string GetManuContainerPackEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_container_pack` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_container_pack`(  `Id`, `SiteId`, `ContainerBarCodeId`, `LadeBarCode`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @ContainerBarCodeId, @LadeBarCode, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_container_pack`(  `Id`, `SiteId`, `ContainerBarCodeId`, `LadeBarCode`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @ContainerBarCodeId, @LadeBarCode, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_container_pack` SET   SiteId = @SiteId, ContainerBarCodeId = @ContainerBarCodeId, LadeBarCode = @LadeBarCode, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_container_pack` SET   SiteId = @SiteId, ContainerBarCodeId = @ContainerBarCodeId, LadeBarCode = @LadeBarCode, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_container_pack` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_container_pack` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";
        const string DeleteAllSql = "DELETE FROM `manu_container_pack`  WHERE ContainerBarCodeId = @ContainerBarCodeId ";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `ContainerBarCodeId`, `LadeBarCode`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_container_pack`  WHERE Id = @Id ";
        const string GetBysfcSql = @"SELECT 
                               `Id`, `SiteId`, `ContainerBarCodeId`, `LadeBarCode`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_container_pack`  WHERE LadeBarCode = @SFC ";
        const string GetByPackcodeSql = @"SELECT 
                               `Id`, `SiteId`, `ContainerBarCodeId`, `LadeBarCode`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_container_pack`  WHERE ContainerBarCodeId = @ContainerBarCodeId ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `ContainerBarCodeId`, `LadeBarCode`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_container_pack`  WHERE Id IN @Ids ";
        #endregion
    }
}