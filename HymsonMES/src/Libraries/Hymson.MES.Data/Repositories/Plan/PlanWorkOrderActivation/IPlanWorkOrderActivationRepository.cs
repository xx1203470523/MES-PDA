/*
 *creator: Karl
 *
 *describe: 工单激活仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-29 10:23:51
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Repositories.Common.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 工单激活仓储接口
    /// </summary>
    public interface IPlanWorkOrderActivationRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="planWorkOrderActivationEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(PlanWorkOrderActivationEntity planWorkOrderActivationEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="planWorkOrderActivationEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<PlanWorkOrderActivationEntity> planWorkOrderActivationEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="planWorkOrderActivationEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(PlanWorkOrderActivationEntity planWorkOrderActivationEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="planWorkOrderActivationEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<PlanWorkOrderActivationEntity> planWorkOrderActivationEntitys);

        /// <summary>
        /// 删除
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
        /// 删除（硬删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteTrueAsync(long id);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PlanWorkOrderActivationEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkOrderActivationEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="planWorkOrderActivationQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkOrderActivationEntity>> GetPlanWorkOrderActivationEntitiesAsync(PlanWorkOrderActivationQuery planWorkOrderActivationQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="planWorkOrderActivationPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<PlanWorkOrderActivationListDetailView>> GetPagedInfoAsync(PlanWorkOrderActivationPagedQuery planWorkOrderActivationPagedQuery);
    }
}