using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 服务接口（产品检验参数组）
    /// </summary>
    public interface IProcProductParameterGroupService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(ProcProductParameterGroupSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(ProcProductParameterGroupSaveDto saveDto);

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
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcProductParameterGroupInfoDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 根据ID获取项目明细列表
        /// </summary>
        /// <param name="parameterGroupId"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProductParameterGroupDetailDto>> QueryDetailsByParameterGroupIdAsync(long parameterGroupId);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcProductParameterGroupDto>> GetPagedListAsync(ProcProductParameterGroupPagedQueryDto pagedQueryDto);

    }
}