using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.Query;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 仓储接口（消息管理）
    /// </summary>
    public interface IInteMessageManageRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(InteMessageManageEntity entity);

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<InteMessageManageEntity> entities);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteMessageManageEntity entity);

        /// <summary>
        /// 接收
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> ReceiveAsync(InteMessageManageEntity entity);

        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> HandleAsync(InteMessageManageEntity entity);

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> CloseAsync(InteMessageManageEntity entity);

        /// <summary>
        /// 软删除  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 软删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand command);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteMessageManageEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<InteMessageManageEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<InteMessageManageEntity>> GetEntitiesAsync(InteMessageManageQuery query);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<InteMessageManageView>> GetPagedListAsync(InteMessageManagePagedQuery pagedQuery);

    }
}
