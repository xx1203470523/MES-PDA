﻿using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentLinkHardware.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentUnit.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Equipment.EquEquipmentLinkApi
{
    /// <summary>
    /// 设备关联硬件设备
    /// </summary>
    public partial class EquEquipmentLinkHardwareRepository : BaseRepository,IEquEquipmentLinkHardwareRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquEquipmentLinkHardwareRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentUnitEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquEquipmentLinkHardwareEntity equipmentUnitEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, equipmentUnitEntity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<EquEquipmentLinkHardwareEntity> entitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entitys);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentUnitEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquEquipmentLinkHardwareEntity equipmentUnitEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, equipmentUnitEntity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<EquEquipmentLinkHardwareEntity> entitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entitys);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> SoftDeleteAsync(IEnumerable<long> idsArr)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(SoftDeleteSql, new { Id = idsArr });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long equipmentId)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteByEquipmentIdSql, new { EquipmentId = equipmentId });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentIds"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] equipmentIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesByEquipmentIdSql, new { EquipmentIds = equipmentIds });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquEquipmentLinkHardwareEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquEquipmentLinkHardwareEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hardwareCode"></param>
        /// <param name="hardwareType"></param>
        /// <returns></returns>
        public async Task<EquEquipmentLinkHardwareEntity> GetByHardwareCodeAsync(string hardwareCode, string hardwareType)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquEquipmentLinkHardwareEntity>(GetByHardwareCodeSql, new { hardwareCode, hardwareType });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquEquipmentLinkHardwareEntity>> GetListAsync(long equipmentId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquEquipmentLinkHardwareEntity>(GetByEquipmentSql, new { equipmentId });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquEquipmentLinkHardwareEntity>> GetEntitiesAsync(EquEquipmentLinkHardwareQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var equipmentUnitEntities = await conn.QueryAsync<EquEquipmentLinkHardwareEntity>(template.RawSql, query);
            return equipmentUnitEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquEquipmentLinkHardwareEntity>> GetPagedListAsync(EquEquipmentLinkHardwarePagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Select("*");

            sqlBuilder.Where("SiteId = @SiteId");

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entities = await conn.QueryAsync<EquEquipmentLinkHardwareEntity>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);

            return new PagedInfo<EquEquipmentLinkHardwareEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class EquEquipmentLinkHardwareRepository
    {
        /// <summary>
        /// 
        /// </summary>
        const string InsertSql = "INSERT INTO `equ_equipment_link_hardware`(  `Id`, `EquipmentId`, `HardwareCode`, `HardwareType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteCode`) VALUES (   @Id, @EquipmentId, @HardwareCode, @HardwareType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark, @SiteCode )  ";
        const string UpdateSql = "UPDATE `equ_equipment_link_hardware` SET   EquipmentId = @EquipmentId, HardwareCode = @HardwareCode, HardwareType = @HardwareType, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, Remark = @Remark, SiteCode = @SiteCode  WHERE Id = @Id ";
        const string SoftDeleteSql = "UPDATE `equ_equipment_link_hardware` SET `IsDeleted` = 1 WHERE `Id` = @Id;";
        const string DeleteByEquipmentIdSql = "DELETE FROM `equ_equipment_link_hardware` WHERE `EquipmentId`= @EquipmentId;";
        const string DeletesByEquipmentIdSql = "DELETE FROM `equ_equipment_link_hardware` WHERE `EquipmentId` in @EquipmentIds;";
        const string GetByIdSql = "SELECT * FROM `equ_equipment_link_hardware` WHERE `Id` = @Id;";
        const string GetByHardwareCodeSql = "SELECT * FROM `equ_equipment_link_hardware` WHERE `HardwareCode` = @HardwareCode AND `HardwareType` = @HardwareType;";
        const string GetByEquipmentSql = "SELECT * FROM `equ_equipment_link_hardware` WHERE `EquipmentId` = @EquipmentId;";
        const string GetPagedInfoDataSqlTemplate = "SELECT /**select**/ FROM `equ_equipment_link_hardware` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_equipment_link_hardware` /**innerjoin**/ /**leftjoin**/ /**where**/";
        const string GetEntitiesSqlTemplate = "";
    }
}
