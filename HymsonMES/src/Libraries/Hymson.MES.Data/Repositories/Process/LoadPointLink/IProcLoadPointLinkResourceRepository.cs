/*
 *creator: Karl
 *
 *describe: 上料点关联资源表仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-18 09:36:09
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 上料点关联资源表仓储接口
    /// </summary>
    public interface IProcLoadPointLinkResourceRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procLoadPointLinkResourceEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcLoadPointLinkResourceEntity procLoadPointLinkResourceEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procLoadPointLinkResourceEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ProcLoadPointLinkResourceEntity> procLoadPointLinkResourceEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procLoadPointLinkResourceEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcLoadPointLinkResourceEntity procLoadPointLinkResourceEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procLoadPointLinkResourceEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ProcLoadPointLinkResourceEntity> procLoadPointLinkResourceEntitys);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand param);

        /// <summary>
        /// 根据LoadPointId批量真删除 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesByLoadPointIdTrueAsync(long[] ids);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcLoadPointLinkResourceEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcLoadPointLinkResourceEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcLoadPointLinkResourceView>> GetLoadPointLinkResourceAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procLoadPointLinkResourceQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcLoadPointLinkResourceEntity>> GetProcLoadPointLinkResourceEntitiesAsync(ProcLoadPointLinkResourceQuery procLoadPointLinkResourceQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procLoadPointLinkResourcePagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcLoadPointLinkResourceEntity>> GetPagedInfoAsync(ProcLoadPointLinkResourcePagedQuery procLoadPointLinkResourcePagedQuery);
    }
}
