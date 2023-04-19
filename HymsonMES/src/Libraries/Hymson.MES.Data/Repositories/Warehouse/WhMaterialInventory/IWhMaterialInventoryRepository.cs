/*
 *creator: Karl
 *
 *describe: 物料库存仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-06 03:27:59
 */
using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Warehouse
{
    /// <summary>
    /// 物料库存仓储接口
    /// </summary>
    public interface IWhMaterialInventoryRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="whMaterialInventoryEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(WhMaterialInventoryEntity whMaterialInventoryEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="whMaterialInventoryEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(IEnumerable<WhMaterialInventoryEntity> whMaterialInventoryEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="whMaterialInventoryEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(WhMaterialInventoryEntity whMaterialInventoryEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="whMaterialInventoryEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(IEnumerable<WhMaterialInventoryEntity> whMaterialInventoryEntitys);

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdatePointByBarCodeAsync(UpdateStatusByBarCodeCommand command);

        /// <summary>
        /// 更新库存数量(增加库存)
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        Task<int> UpdateIncreaseQuantityResidueAsync(UpdateQuantityCommand updateQuantityCommand);

        /// <summary>
        /// 更新库存数量(减少库存)
        /// </summary>
        /// <param name="updateQuantityCommand"></param>
        /// <returns></returns>
        Task<int> UpdateReduceQuantityResidueAsync(UpdateQuantityCommand updateQuantityCommand);

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
        Task<WhMaterialInventoryEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据物料条码获取数据
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        Task<WhMaterialInventoryEntity> GetByBarCodeAsync(string barCode);

        /// <summary>
        /// 根据物料条码获取数据
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        Task<IEnumerable<WhMaterialInventoryEntity>> GetByBarCodesAsync(WhMaterialInventoryBarcodeQuery param);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<WhMaterialInventoryEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="whMaterialInventoryQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<WhMaterialInventoryEntity>> GetWhMaterialInventoryEntitiesAsync(WhMaterialInventoryQuery whMaterialInventoryQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="whMaterialInventoryPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<WhMaterialInventoryPageListView>> GetPagedInfoAsync(WhMaterialInventoryPagedQuery whMaterialInventoryPagedQuery);



        /// <summary>
        /// 根据物料编码获取物料数据
        /// </summary>
        /// <param name="materialCode"></param>
        /// <returns></returns>
        Task<ProcMaterialInfoView> GetProcMaterialByMaterialCodeAsync(string materialCode);


        /// <summary>
        /// 根据物料编码获取供应商信息
        /// </summary>
        /// <param name="materialCode"></param>
        /// <returns></returns>
        Task<IEnumerable<WhSupplierInfoView>> GetWhSupplierByMaterialIdAsync(long materialId, long supplierCode = 0);
    }
}
