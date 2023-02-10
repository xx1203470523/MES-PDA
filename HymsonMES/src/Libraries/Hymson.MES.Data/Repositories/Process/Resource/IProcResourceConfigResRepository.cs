/*
 *creator: Karl
 *
 *describe: 资源配置表仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-10 10:21:26
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 资源配置表仓储接口
    /// </summary>
    public interface IProcResourceConfigResRepository
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcResourceConfigResEntity>> GetPagedInfoAsync(ProcResourceConfigResPagedQuery query);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procResourceConfigResEntity"></param>
        /// <returns></returns>
        Task InsertAsync(ProcResourceConfigResEntity procResourceConfigResEntity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procResourceConfigResEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcResourceConfigResEntity procResourceConfigResEntity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] idsArr);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcResourceConfigResEntity> GetByIdAsync(long id);
    }
}