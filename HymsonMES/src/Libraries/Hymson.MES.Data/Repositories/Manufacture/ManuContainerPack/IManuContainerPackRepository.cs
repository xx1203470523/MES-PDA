/*
 *creator: Karl
 *
 *describe: 容器装载表（物理删除）仓储类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-04-12 02:33:13
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 容器装载表（物理删除）仓储接口
    /// </summary>
    public interface IManuContainerPackRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuContainerPackEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuContainerPackEntity manuContainerPackEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuContainerPackEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(IEnumerable<ManuContainerPackEntity> manuContainerPackEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuContainerPackEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuContainerPackEntity manuContainerPackEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuContainerPackEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuContainerPackEntity> manuContainerPackEntitys);

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
        /// 批量硬删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>

        Task<int> DeleteTrueAsync(DeleteCommand param);

        /// <summary>
        /// 根据容器Id 删除所有容器装载记录（物理删除）
        /// </summary>
        /// <param name="containerBarCodeId"></param>
        /// <returns></returns>
        Task<int> DeleteAllAsync(long containerBarCodeId);


        /// <summary>
        /// 根据容器Id 查询所有容器装载记录
        /// </summary>
        /// <param name="containerBarCodeId"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuContainerPackEntity>> GetByContainerBarCodeIdIdAsync(long containerBarCodeId);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuContainerPackEntity> GetByIdAsync(long id);
        /// <summary>
        /// 根据条码编码获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ManuContainerPackEntity> GetByLadeBarCodeAsync(ManuContainerPackQuery query);

        /// <summary>
        /// 根据SFC批量获取装箱数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuContainerPackEntity>> GetByLadeBarCodesAsync(ManuContainerPackQuery query);

        /// <summary>
        /// 根据容器ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuContainerPackEntity>> GetByContainerBarCodeIdAsync(long cid, long siteid);

        /// <summary>
        /// 根据容器ID获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuContainerPackEntity>> GetByContainerBarCodeIdsAsync(long[] ids, long siteId);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuContainerPackEntity>> GetByIdsAsync(IEnumerable<long> ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuContainerPackQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuContainerPackEntity>> GetManuContainerPackEntitiesAsync(ManuContainerPackQuery manuContainerPackQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuContainerPackPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuContainerPackView>> GetPagedInfoAsync(ManuContainerPackPagedQuery manuContainerPackPagedQuery);

        /// <summary>
        /// 获取容器的包装数量
        /// </summary>
        /// <param name="containerBarCodeId"></param>
        /// <returns></returns>
        Task<int> GetCountByrBarCodeIdAsync(long containerBarCodeId);
        Task<ManuContainerPackEntity> GetOneAsync(ManuContainerPackQuery query);
        Task<int> GetCountAsync(ManuContainerPackQuery query);
        Task<IEnumerable<ManuContainerPackEntity>> GetListAsync(ManuContainerPackQuery query);
        Task<int> InsertIgnoreAsync(ManuContainerPackEntity manuContainerPackEntity);
        //Task<int> UpdateOutermostContainerBarCodeAndDeepAsync(IEnumerable<UpdateOutermostContainerBarCodeAndDeepCommand> commands);
        #endregion
    }
}
