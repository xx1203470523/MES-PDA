﻿using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture.ManuSfcOperate;
using Hymson.MES.Services.Dtos.Manufacture.ManuSfcOperateDto;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 生产服务接口
    /// </summary>
    public interface IManuSfcOperateService
    {
        /// <summary>
        /// 创建条码（半成品）
        /// </summary>
        /// <param name="baseDto"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> CreateBarcodeBySemiProductIdAsync(BaseDto baseDto);

        /// <summary>
        /// 创建条码（电芯）
        /// </summary>
        /// <param name="baseDto"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> CreateCellBarCodeAsync(BaseDto baseDto);

        /// <summary>
        /// 进站
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task InBoundAsync(InBoundDto request);

        /// <summary>
        /// 生成条码并进站
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task GenerateBarcodeAndInBoundAsync(BaseDto request);

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
        Task InBoundCarrierAsync(InBoundCarrierDto request);

        /// <summary>
        /// 载具出站
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task OutBoundCarrierAsync(OutBoundCarrierDto request);

        /// <summary>
        /// 中止
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task StopStationMoreAsync(StopBoundDto request);

        /// <summary>
        /// 分页查询列表（PDA条码出站）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcInstationPagedQueryOutputDto>> GetPagedListAsync(ManuSfcInstationPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 获取条码信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<ManuSfcOutstationConfirmSfcInfoOutputDto> GetSfcInfoToPdaAsync(string sfc);
    }
}