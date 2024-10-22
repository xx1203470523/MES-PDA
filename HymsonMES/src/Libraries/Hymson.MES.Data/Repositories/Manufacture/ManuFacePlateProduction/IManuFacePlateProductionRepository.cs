using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 生产过站面板仓储接口
    /// </summary>
    public interface IManuFacePlateProductionRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuFacePlateProductionEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuFacePlateProductionEntity manuFacePlateProductionEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuFacePlateProductionEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuFacePlateProductionEntity> manuFacePlateProductionEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuFacePlateProductionEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuFacePlateProductionEntity manuFacePlateProductionEntity);

        /// <summary>
        /// 根据FacePlateId更新
        /// </summary>
        /// <param name="manuFacePlateProductionEntity"></param>
        /// <returns></returns>
        Task<int> UpdateByFacePlateIdAsync(ManuFacePlateProductionEntity manuFacePlateProductionEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuFacePlateProductionEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuFacePlateProductionEntity> manuFacePlateProductionEntitys);

        /// <summary>
        /// 删除  
        /// 最好使用批量删除，可以设置更新人和更新时间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand param);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuFacePlateProductionEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuFacePlateProductionEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuFacePlateProductionQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuFacePlateProductionEntity>> GetManuFacePlateProductionEntitiesAsync(ManuFacePlateProductionQuery manuFacePlateProductionQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuFacePlateProductionPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuFacePlateProductionEntity>> GetPagedInfoAsync(ManuFacePlateProductionPagedQuery manuFacePlateProductionPagedQuery);
        /// <summary>
        /// 通过FacePlateId获取明细
        /// </summary>
        /// <param name="facePlateId"></param>
        /// <returns></returns>
        Task<ManuFacePlateProductionEntity> GetByFacePlateIdAsync(long facePlateId);
        #endregion
    }
}
