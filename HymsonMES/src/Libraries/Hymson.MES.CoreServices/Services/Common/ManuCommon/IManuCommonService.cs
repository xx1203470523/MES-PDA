﻿using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Manufacture;

namespace Hymson.MES.CoreServices.Services.Common.ManuCommon
{
    /// <summary>
    /// 生产公共类
    /// </summary>
    public interface IManuCommonService
    {
        /// <summary>
        /// 批量验证条码是否锁定
        /// </summary>
        /// <param name="procedureBo"></param>
        /// <returns></returns>
        Task VerifySfcsLockAsync(ManuProcedureBo procedureBo);

        /// <summary>
        /// 批量验证条码是否被容器包装
        /// </summary>
        /// <param name="sfcBos"></param>
        /// <returns></returns>
        Task VerifyContainerAsync(MultiSFCBo sfcBos);

        /// <summary>
        /// 验证条码BOM清单用量
        /// </summary>
        /// <param name="procedureBomBo"></param>
        /// <returns></returns>
        Task VerifyBomQtyAsync(ManuProcedureBomBo procedureBomBo);

        /// <summary>
        /// 验证条码掩码规则
        /// </summary>
        /// <param name="barCode"></param>
        /// <param name="materialId"></param>
        /// <returns></returns>
        Task<bool> CheckBarCodeByMaskCodeRuleAsync(string barCode, long materialId);

        /// <summary>
        /// 获取载具里面的条码（带验证）
        /// </summary>
        /// <param name="requestBo"></param>
        /// <returns></returns>
        Task<IEnumerable<VehicleSFCResponseBo>> GetSFCsByVehicleCodesAsync(VehicleSFCRequestBo requestBo);

        /// <summary>
        /// 获取当前生产对象
        /// </summary>
        /// <param name="requestBo"></param>
        /// <returns></returns>
        Task<ManufactureResponseBo> GetManufactureBoAsync(ManufactureRequestBo requestBo);

    }
}
