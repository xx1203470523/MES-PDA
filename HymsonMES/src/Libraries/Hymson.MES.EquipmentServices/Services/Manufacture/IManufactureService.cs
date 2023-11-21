﻿using Hymson.MES.EquipmentServices.Dtos;

namespace Hymson.MES.EquipmentServices.Services.Manufacture
{
    /// <summary>
    /// 生产服务接口
    /// </summary>
    public interface IManufactureService
    {
        /// <summary>
        /// 创建条码
        /// </summary>
        /// <param name="baseDto"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> CreateBarcodeBySemiProductIdAsync(BaseDto baseDto);

        /// <summary>
        /// 进站
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task InBoundAsync(InBoundDto request);

        /// <summary>
        /// 进站（多个）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task InBoundMoreAsync(InBoundMoreDto request);

        /// <summary>
        /// 出站
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task OutBoundAsync(OutBoundDto request);

        /// <summary>
        /// 出站（多个）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task OutBoundMoreAsync(OutBoundMoreDto request);

        /// <summary>
        /// 载具进站
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task InBoundVehicleAsync(InBoundVehicleDto request);

        /// <summary>
        /// 载具出站
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task OutBoundVehicleAsync(OutBoundVehicleDto request);

        /// <summary>
        /// 参数收集（点击）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task InParameterAsync(InParameterDto request);

    }
}