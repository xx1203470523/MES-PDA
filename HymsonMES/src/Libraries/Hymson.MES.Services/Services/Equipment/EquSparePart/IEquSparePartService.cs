using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Services.Equipment.EquSparePart
{
    /// <summary>
    /// 备件注册 service接口
    /// </summary>
    public interface IEquSparePartService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="equSparePartPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquSparePartDto>> GetPageListAsync(EquSparePartPagedQueryDto equSparePartPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equSparePartDto"></param>
        /// <returns></returns>
        Task<int> CreateEquSparePartAsync(EquSparePartCreateDto equSparePartCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="equSparePartDto"></param>
        /// <returns></returns>
        Task<int> ModifyEquSparePartAsync(EquSparePartModifyDto equSparePartModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteEquSparePartAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesEquSparePartAsync(long[] idsArr);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquSparePartDto> QueryEquSparePartByIdAsync(long id);
    }
}