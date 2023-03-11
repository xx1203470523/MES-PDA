/*
 *creator: Karl
 *
 *describe: 仓库标签模板仓储类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-03-09 02:51:26
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 仓库标签模板仓储接口
    /// </summary>
    public interface IProcLabelTemplateRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procLabelTemplateEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcLabelTemplateEntity procLabelTemplateEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procLabelTemplateEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ProcLabelTemplateEntity> procLabelTemplateEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procLabelTemplateEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcLabelTemplateEntity procLabelTemplateEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procLabelTemplateEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ProcLabelTemplateEntity> procLabelTemplateEntitys);

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
        Task<int> DeletesAsync(long[] ids);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcLabelTemplateEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcLabelTemplateEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procLabelTemplateQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcLabelTemplateEntity>> GetProcLabelTemplateEntitiesAsync(ProcLabelTemplateQuery procLabelTemplateQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procLabelTemplatePagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcLabelTemplateEntity>> GetPagedInfoAsync(ProcLabelTemplatePagedQuery procLabelTemplatePagedQuery);
    }
}