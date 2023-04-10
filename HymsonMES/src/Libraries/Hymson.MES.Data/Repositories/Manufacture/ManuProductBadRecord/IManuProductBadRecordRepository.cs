/*
 *creator: Karl
 *
 *describe: 产品不良录入仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-03-27 03:49:17
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuProductBadRecord.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 产品不良录入仓储接口
    /// </summary>
    public interface IManuProductBadRecordRepository
    {
        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuProductBadRecordEntity> GetByIdAsync(long id);

        /// <summary>
        /// 查询条码的不合格代码信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuProductBadRecordView>> GetBadRecordsBySfcAsync(ManuProductBadRecordQuery query);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuProductBadRecordEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuProductBadRecordQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuProductBadRecordEntity>> GetManuProductBadRecordEntitiesAsync(ManuProductBadRecordQuery manuProductBadRecordQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuProductBadRecordPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuProductBadRecordEntity>> GetPagedInfoAsync(ManuProductBadRecordPagedQuery manuProductBadRecordPagedQuery);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuProductBadRecordEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuProductBadRecordEntity manuProductBadRecordEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuProductBadRecordEntitys"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<ManuProductBadRecordEntity> manuProductBadRecordEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuProductBadRecordEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuProductBadRecordEntity manuProductBadRecordEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuProductBadRecordEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<ManuProductBadRecordEntity> manuProductBadRecordEntitys);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeleteRangeAsync(DeleteCommand command);

        /// <summary>
        /// 关闭条码不合格标识和缺陷
        /// </summary>
        /// <param name="manuSfcInfoEntity"></param>
        /// <returns></returns>
        Task<int> UpdateStatusRangeAsync(List<ManuProductBadRecordCommand> commands);
    }
}