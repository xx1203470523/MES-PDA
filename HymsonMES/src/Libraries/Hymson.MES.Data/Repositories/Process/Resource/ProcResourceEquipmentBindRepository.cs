/*
 *creator: Karl
 *
 *describe: 资源设备绑定表 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-10 11:20:47
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 资源设备绑定表仓储
    /// </summary>
    public partial class ProcResourceEquipmentBindRepository : BaseRepository, IProcResourceEquipmentBindRepository
    {
        public ProcResourceEquipmentBindRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procResourceEquipmentBindPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceEquipmentBindView>> GetPagedInfoAsync(ProcResourceEquipmentBindPagedQuery procResourceEquipmentBindPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("a.IsDeleted=0");
            sqlBuilder.Where("a.ResourceId=@ResourceId");
            //TODO 按更新时间倒序排列

            var offSet = (procResourceEquipmentBindPagedQuery.PageIndex - 1) * procResourceEquipmentBindPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procResourceEquipmentBindPagedQuery.PageSize });
            sqlBuilder.AddParameters(procResourceEquipmentBindPagedQuery);

            using var conn = GetMESDbConnection();
            var procResourceEquipmentBindEntitiesTask = conn.QueryAsync<ProcResourceEquipmentBindView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procResourceEquipmentBindEntities = await procResourceEquipmentBindEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcResourceEquipmentBindView>(procResourceEquipmentBindEntities, procResourceEquipmentBindPagedQuery.PageIndex, procResourceEquipmentBindPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 根据资源id和设备Id查询数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcResourceEquipmentBindEntity>> GetByResourceIdAsync(ProcResourceEquipmentBindQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetByResourceIdSqllTemplate);
            sqlBuilder.Where("IsDeleted=0");
            if (query.ResourceId > 0)
            {
                sqlBuilder.Where("ResourceId=@ResourceId");
            }
            if (query.IsMain)
            {
                sqlBuilder.Where("IsMain=@IsMain");
            }
            if (query.Ids != null && query.Ids.Length > 0)
            {
                sqlBuilder.Where("EquipmentId in @Ids");
            }
            if (query.SiteId.HasValue)
            {
                sqlBuilder.Where("SiteId = @SiteId");
            }
            sqlBuilder.AddParameters(query);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcResourceEquipmentBindView>(templateData.RawSql, templateData.Parameters);
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procResourceEquipmentBinds"></param>
        /// <returns></returns>
        public async Task InsertRangeAsync(IEnumerable<ProcResourceEquipmentBindEntity> procResourceEquipmentBinds)
        {
            using var conn = GetMESDbConnection();
            await conn.ExecuteAsync(InsertSql, procResourceEquipmentBinds);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procResourceEquipmentBinds"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ProcResourceEquipmentBindEntity> procResourceEquipmentBinds)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procResourceEquipmentBinds);
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesRangeAsync(long[] idsArr)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Ids = idsArr });
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteByResourceIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteByResourceIdSql, new { ResourceId = id });
        }
    }

    public partial class ProcResourceEquipmentBindRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"select a.*,b.EquipmentCode,b.EquipmentName,b.EquipmentDesc from proc_resource_equipment_bind a left join equ_equipment b on a.EquipmentId=b.Id and b.IsDeleted=0 /**where**/ ORDER BY a.UpdatedOn DESC LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "select count(*) from proc_resource_equipment_bind a left join equ_equipment b on a.EquipmentId=b.Id and b.IsDeleted=0 /**where**/";

        const string InsertSql = "INSERT INTO `proc_resource_equipment_bind`(  `Id`, `SiteId`, `ResourceId`, `EquipmentId`, `IsMain`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (@Id, @SiteId, @ResourceId, @EquipmentId, @IsMain, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_resource_equipment_bind` SET  EquipmentId=@EquipmentId,IsMain=@IsMain,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_resource_equipment_bind` SET IsDeleted = Id WHERE Id in @Ids ";
        const string DeleteByResourceIdSql = "delete from `proc_resource_equipment_bind` WHERE ResourceId = @ResourceId ";
        const string GetByResourceIdSqllTemplate = "SELECT * FROM proc_resource_equipment_bind /**where**/  ";
    }
}
