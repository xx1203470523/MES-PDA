
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Qual;

namespace Hymson.MES.Services.Qual;

/// <summary>
/// <para>@描述：IQC检验项目详情; 服务接口</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// </summary>
public interface IQualIqcInspectionItemDetailService
{
    /// <summary>
    /// <para>@描述：IQC检验项目详情; 根据条件判断数据是否存在</para>
    /// <para>@作者：Jim</para>
    /// <para>@创建时间：2024-2-5</para>
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns></returns>
    Task<bool> IsExistAsync(QualIqcInspectionItemDetailQueryDto queryDto);

    /// <summary>
    /// <para>@描述：IQC检验项目详情; 根据条件获取一组数据</para>
    /// <para>@作者：Jim</para>
    /// <para>@创建时间：2024-2-5</para>
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns></returns>
    Task<IEnumerable<QualIqcInspectionItemDetailOutputDto>> GetListAsync(QualIqcInspectionItemDetailQueryDto queryDto);

    /// <summary>
    /// <para>@描述：IQC检验项目详情; 根据条件获取一行数据</para>
    /// <para>@作者：Jim</para>
    /// <para>@创建时间：2024-2-5</para>
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns></returns>
    Task<QualIqcInspectionItemDetailOutputDto> GetOneAsync(QualIqcInspectionItemDetailQueryDto queryDto);

    /// <summary>
    /// <para>@描述：IQC检验项目详情; 分页查询数据</para>
    /// <para>@作者：Jim</para>
    /// <para>@创建时间：2024-2-5</para>
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns></returns>
    Task<PagedInfo<QualIqcInspectionItemDetailOutputDto>> GetPagedAsync(QualIqcInspectionItemDetailPagedQueryDto queryDto);

    /// <summary>
    /// <para>@描述：IQC检验项目详情; 创建数据</para>
    /// <para>@作者：Jim</para>
    /// <para>@创建时间：2024-2-5</para>
    /// </summary>
    /// <param name="createDto">创建数据输入对象</param>
    /// <returns></returns>
    Task CreateAsync(QualIqcInspectionItemDetailDto createDto);

    /// <summary>
    /// <para>@描述：IQC检验项目详情; 更新数据</para>
    /// <para>@作者：Jim</para>
    /// <para>@创建时间：2024-2-5</para>
    /// </summary>
    /// <param name="updateDto">更新数据输入对象</param>
    /// <returns></returns>
    Task UpdateAsync(QualIqcInspectionItemDetailUpdateDto updateDto);

    /// <summary>
    /// <para>@描述：IQC检验项目详情; 删除数据</para>
    /// <para>@作者：Jim</para>
    /// <para>@创建时间：2024-2-5</para>
    /// </summary>
    /// <param name="deleteDto">删除数据输入对象</param>
    /// <returns></returns>
    Task DeleteAsync(QualIqcInspectionItemDetailDeleteDto deleteDto);
}