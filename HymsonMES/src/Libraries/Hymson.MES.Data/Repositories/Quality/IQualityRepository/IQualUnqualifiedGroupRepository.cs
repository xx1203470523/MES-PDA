/*
 *creator: Karl
 *
 *describe: QualUnqualifiedGroupEntity仓储类 | 代码由框架生成
 *builder:  wangkeming
 *build datetime: 2023-02-13 02:05:50
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedCode.Query;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedGroup.Query;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedGroup.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Quality
{
    /// <summary>
    /// 不合格组仓储接口
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public interface IQualUnqualifiedGroupRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> InsertAsync(QualUnqualifiedGroupEntity param);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> InsertRangAsync(List<QualUnqualifiedGroupEntity> param);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(QualUnqualifiedGroupEntity param);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> UpdateRangAsync(List<QualUnqualifiedGroupEntity> param);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeleteRangAsync(DeleteCommand param);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QualUnqualifiedGroupEntity> GetByIdAsync(long id);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<QualUnqualifiedGroupEntity>> GetListByProcedureIdAsync(QualUnqualifiedGroupQuery param);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PagedInfo<QualUnqualifiedGroupEntity>> GetPagedInfoAsync(QualUnqualifiedGroupPagedQuery param);

        /// <summary>
        /// 根据编码获取数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<QualUnqualifiedGroupEntity> GetByCodeAsync(QualUnqualifiedGroupByCodeQuery param);

        /// <summary>
        /// 插入不合格代码组关联不合格代码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> InsertQualUnqualifiedCodeGroupRelationRangAsync(List<QualUnqualifiedCodeGroupRelation> param);

        /// <summary>
        /// 删除不合格代码组关联不合格代码
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<int> RealDelteQualUnqualifiedCodeGroupRelationAsync(long id);

        /// <summary>
        /// 插入不合格组关联工序
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> InsertQualUnqualifiedGroupProcedureRelationRangAsync(List<QualUnqualifiedGroupProcedureRelation> param);

        /// <summary>
        /// 删除不合格组关联工序
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<int> RealDelteQualUnqualifiedGroupProcedureRelationAsync(long id);

        /// <summary>
        /// 获取不合格组关联不合格代码关系表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<QualUnqualifiedGroupProcedureRelationView>> GetQualUnqualifiedCodeProcedureRelationAsync(long id);

        /// <summary>
        /// 获取不合格组关联不合格代码关系表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<QualUnqualifiedGroupCodeRelationView>> GetQualUnqualifiedCodeGroupRelationAsync(long id);
    }
}
