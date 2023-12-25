using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 仓储（备件注册表）
    /// </summary>
    public partial class EquSparePartsRepository : BaseRepository, IEquSparePartsRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquSparePartsRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquSparePartsEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<EquSparePartsEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquSparePartsEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（备件关联备件类型）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateTypeAsync(UpdateSparePartsTypeEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateTypeSql, entity);
        }

        /// <summary>
        /// 更新（清空备件关联备件类型）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> CleanTypeAsync(UpdateSparePartsTypeEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(CleanTypeSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<EquSparePartsEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, entities);
        }

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 软删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, command);
        }

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<EquSparePartsEntity> GetByCodeAsync(EntityByCodeQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquSparePartsEntity>(GetByCodeSql, query);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquSparePartsEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquSparePartsEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSparePartsEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquSparePartsEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 获取关联的备件
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSparePartsEntity>> GetSparePartsGroupRelationAsync(long Id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquSparePartsEntity>(GetSparePartsGroupRelationSqlTemplate, new { Id = Id });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSparePartsEntity>> GetEntitiesAsync(EquSparePartsQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquSparePartsEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquSparePartsEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSparePartsEntity>> GetPagedInfoAsync(EquSparePartsPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("esp.Id,ESP.`Code`,ESP.`Name`,ESP.`Status`,ESPG.`Code` AS sparePartsGroup,esp.Remark,esp.CreatedBy,esp.CreatedOn,esp.UpdatedOn,esp.UpdatedBy");
            sqlBuilder.Where("esp.SiteId = @SiteId");
            sqlBuilder.Where("esp.IsDeleted = 0");
            sqlBuilder.LeftJoin("equ_spare_parts_group espg ON espg.Id = esp.SparePartsGroupId");
            sqlBuilder.OrderBy("esp.UpdatedOn DESC");

            if (!string.IsNullOrWhiteSpace(pagedQuery.Code))
            {
                pagedQuery.Code = $"%{pagedQuery.Code}%";
                sqlBuilder.Where("esp.Code LIKE @Code");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.Name))
            {
                pagedQuery.Name = $"%{pagedQuery.Name}%";
                sqlBuilder.Where("esp.Name LIKE @Name");
            }
            if (pagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("esp.Status = @Status");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.SparePartsGroup))
            {
                pagedQuery.SparePartsGroup = $"%{pagedQuery.SparePartsGroup}%";
                sqlBuilder.Where("espg.`Code` LIKE @SparePartsGroup");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<EquSparePartsEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquSparePartsEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 分页查询(过滤掉已有类型的备件)
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSparePartsEntity>> GetPagedInfoNotWithTypeoAsync(EquSparePartsPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("esp.Id,ESP.`Code`,ESP.`Name`,ESP.`Status`,esp.Remark,esp.CreatedBy,esp.CreatedOn,esp.UpdatedOn,esp.UpdatedBy,esp.IsDeleted");
            sqlBuilder.Where("esp.SiteId = @SiteId");
            sqlBuilder.Where("esp.IsDeleted = 0");
            sqlBuilder.Where("(esp.SparePartsGroupId is null Or esp.SparePartsGroupId=0 Or esp.SparePartsGroupId=@Id)");
            sqlBuilder.OrderBy("esp.UpdatedOn DESC");

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<EquSparePartsEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquSparePartsEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>a
    public partial class EquSparePartsRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_spare_parts` esp /**innerjoin**/ /**join**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_spare_parts` esp /**innerjoin**/ /**join**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEquSparePartsEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_spare_parts` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_spare_parts`(  `Id`, `SiteId`, `Code`, `Name`, `Manufacturer`, `Supplier`, `Status`, `SparePartsGroupId`, `DrawCode`, `Model`, `Position`, `IsAssociatedDevice`, `IsStandardPart`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @Code, @Name, @Manufacturer, @Supplier, @Status, @SparePartsGroupId, @DrawCode, @Model, @Position, @IsAssociatedDevice, @IsStandardPart, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `equ_spare_parts`(  `Id`, `SiteId`, `Code`, `Name`, `Manufacturer`, `Supplier`, `Status`, `SparePartsGroupId`, `DrawCode`, `Model`, `Position`, `IsAssociatedDevice`, `IsStandardPart`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @Code, @Name, @Manufacturer, @Supplier, @Status, @SparePartsGroupId, @DrawCode, @Model, @Position, @IsAssociatedDevice, @IsStandardPart, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `equ_spare_parts` SET   SiteId = @SiteId, Code = @Code, Name = @Name, Manufacturer = @Manufacturer, Supplier = @Supplier, Status = @Status, SparePartsGroupId = @SparePartsGroupId, DrawCode = @DrawCode, Model = @Model, Position = @Position, IsAssociatedDevice = @IsAssociatedDevice, IsStandardPart = @IsStandardPart, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_spare_parts` SET   SiteId = @SiteId, Code = @Code, Name = @Name, Manufacturer = @Manufacturer, Supplier = @Supplier, Status = @Status, SparePartsGroupId = @SparePartsGroupId, DrawCode = @DrawCode, Model = @Model, Position = @Position, IsAssociatedDevice = @IsAssociatedDevice, IsStandardPart = @IsStandardPart, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdateTypeSql = "UPDATE `equ_spare_parts` SET  SparePartsGroupId = @Id, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id in @SparePartIds ";
        const string CleanTypeSql = "UPDATE equ_spare_parts SET SparePartsGroupId=0 where SparePartsGroupId=@Id";

        const string DeleteSql = "UPDATE `equ_spare_parts` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `equ_spare_parts` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM `equ_spare_parts`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM `equ_spare_parts`  WHERE Id IN @Ids ";

        const string GetByCodeSql = "SELECT * FROM `equ_spare_parts` WHERE `IsDeleted` = 0 AND SiteId = @Site AND Code = @Code LIMIT 1";

        const string GetSparePartsGroupRelationSqlTemplate = @"SELECT
	                                                                       Id,
	                                                                       Code,
	                                                                       Name,
	                                                                       SparePartsGroupId,
                                                                           IsDeleted,
	                                                                       CreatedBy,
	                                                                       CreatedOn
                                                                        FROM
	                                                                        equ_spare_parts 

                                                                        WHERE
	                                                                        SparePartsGroupId = @Id 
	                                                                        AND IsDeleted = 0";

    }
}