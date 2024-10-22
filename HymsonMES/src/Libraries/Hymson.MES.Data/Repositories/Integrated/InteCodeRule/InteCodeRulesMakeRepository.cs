/*
 *creator: Karl
 *
 *describe: 编码规则组成 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-17 05:02:19
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 编码规则组成仓储
    /// </summary>
    public partial class InteCodeRulesMakeRepository : BaseRepository, IInteCodeRulesMakeRepository
    {
        public InteCodeRulesMakeRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

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
        /// 根据CodeRulesId删除（真删除）
        /// </summary>
        /// <param name="codeRulesId"></param>
        /// <returns></returns>
        public async Task<int> DeleteByCodeRulesIdAsync(long codeRulesId)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteByCodeRulesIdSql, new { codeRulesId = codeRulesId });
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
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteCodeRulesMakeEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteCodeRulesMakeEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteCodeRulesMakeEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteCodeRulesMakeEntity>(GetByIdsSql, new { ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteCodeRulesMakePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteCodeRulesMakeEntity>> GetPagedInfoAsync(InteCodeRulesMakePagedQuery inteCodeRulesMakePagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId=@SiteId");

            var offSet = (inteCodeRulesMakePagedQuery.PageIndex - 1) * inteCodeRulesMakePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = inteCodeRulesMakePagedQuery.PageSize });
            sqlBuilder.AddParameters(inteCodeRulesMakePagedQuery);

            using var conn = GetMESDbConnection();
            var inteCodeRulesMakeEntitiesTask = conn.QueryAsync<InteCodeRulesMakeEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var inteCodeRulesMakeEntities = await inteCodeRulesMakeEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteCodeRulesMakeEntity>(inteCodeRulesMakeEntities, inteCodeRulesMakePagedQuery.PageIndex, inteCodeRulesMakePagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="inteCodeRulesMakeQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteCodeRulesMakeEntity>> GetInteCodeRulesMakeEntitiesAsync(InteCodeRulesMakeQuery inteCodeRulesMakeQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetInteCodeRulesMakeEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where(" SiteId=@SiteId ");

            if (inteCodeRulesMakeQuery.CodeRulesId>0)
            {
                sqlBuilder.Where(" CodeRulesId=@CodeRulesId ");
            }

            using var conn = GetMESDbConnection();
            var inteCodeRulesMakeEntities = await conn.QueryAsync<InteCodeRulesMakeEntity>(template.RawSql, inteCodeRulesMakeQuery);
            return inteCodeRulesMakeEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteCodeRulesMakeEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteCodeRulesMakeEntity inteCodeRulesMakeEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, inteCodeRulesMakeEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteCodeRulesMakeEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<InteCodeRulesMakeEntity> inteCodeRulesMakeEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, inteCodeRulesMakeEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteCodeRulesMakeEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteCodeRulesMakeEntity inteCodeRulesMakeEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, inteCodeRulesMakeEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="inteCodeRulesMakeEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(IEnumerable<InteCodeRulesMakeEntity> inteCodeRulesMakeEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, inteCodeRulesMakeEntitys);
        }
    }

    public partial class InteCodeRulesMakeRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `inte_code_rules_make` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `inte_code_rules_make` /**where**/ ";
        const string GetInteCodeRulesMakeEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `inte_code_rules_make` /**where**/  ";

        const string InsertSql = "INSERT INTO `inte_code_rules_make`(  `Id`, CodeRulesId, `Seq`, `ValueTakingType`, `SegmentedValue`, CustomValue, `Remark`, `CreatedBy`, `CreatedOn`, `SiteId` ) VALUES (   @Id, @CodeRulesId, @Seq, @ValueTakingType, @SegmentedValue, @CustomValue, @Remark, @CreatedBy, @CreatedOn,  @SiteId )  ";
        const string InsertsSql = "INSERT INTO `inte_code_rules_make`(  `Id`,  CodeRulesId, `Seq`, `ValueTakingType`, `SegmentedValue`, CustomValue, `Remark`, `CreatedBy`, `CreatedOn`, `SiteId` ) VALUES (   @Id, @CodeRulesId, @Seq, @ValueTakingType, @SegmentedValue, @CustomValue, @Remark, @CreatedBy, @CreatedOn, @SiteId )  ";
        const string UpdateSql = "UPDATE `inte_code_rules_make` SET  CodeRulesId=@CodeRulesId,  Seq = @Seq, ValueTakingType = @ValueTakingType, SegmentedValue = @SegmentedValue, CustomValue=@CustomValue, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, SiteId = @SiteId, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `inte_code_rules_make` SET  CodeRulesId=@CodeRulesId, Seq = @Seq, ValueTakingType = @ValueTakingType, SegmentedValue = @SegmentedValue, CustomValue=@CustomValue, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, SiteId = @SiteId, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `inte_code_rules_make` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `inte_code_rules_make` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids ";
        const string DeleteByCodeRulesIdSql= "DELETE FROM `inte_code_rules_make` WHERE CodeRulesId = @codeRulesId ";
        const string GetByIdSql = @"SELECT 
                               `Id`, CodeRulesId, `Seq`, `ValueTakingType`, `SegmentedValue`, CustomValue, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`
                            FROM `inte_code_rules_make`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, CodeRulesId, `Seq`, `ValueTakingType`, `SegmentedValue`, CustomValue, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`
                            FROM `inte_code_rules_make`  WHERE Id IN @ids ";
    }
}
