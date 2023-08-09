/*
 *creator: Karl
 *
 *describe: 工序表仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-13 09:06:05
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 工序表仓储接口
    /// </summary>
    public interface IProcProcedureRepository
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procProcedurePagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcProcedureView>> GetPagedInfoAsync(ProcProcedurePagedQuery procProcedurePagedQuery);

        /// <summary>
        ///分页查询工艺路线的工序列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcProcedureEntity>> GetPagedInfoByProcessRouteIdAsync(ProcProcedurePagedQuery query);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcProcedureEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据资源ID获取工序 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcProcedureEntity> GetProcProdureByResourceIdAsync(ProcProdureByResourceIdQuery param);

        /// <summary>
        /// 判断工序是否存在
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<bool> IsExistsAsync(ProcProcedureQuery query);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcedureEntity>> GetByIdsAsync(IEnumerable<long>  ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procProcedureQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcedureEntity>> GetProcProcedureEntitiesAsync(ProcProcedureQuery procProcedureQuery);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procProcedureEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcProcedureEntity procProcedureEntity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procProcedureEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcProcedureEntity procProcedureEntity);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeleteRangeAsync(DeleteCommand command);
    }
}
