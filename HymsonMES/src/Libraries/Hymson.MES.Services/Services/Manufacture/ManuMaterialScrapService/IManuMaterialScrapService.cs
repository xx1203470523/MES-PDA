﻿using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Services.Dtos.Manufacture.ManuSFCScrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Manufacture.ManuSfcScrapservice
{
    /// <summary>
    /// 部分报废
    /// </summary>
    public interface IManuMaterialScrapService
    {
        /// <summary>
        /// 报废
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task ScrapAsync(ManuMaterialScrapDto param);

        /// <summary>
        /// 扫码校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        Task<MaterialScrapBarCodeDto> BarcodeScanningAsync(MaterialScrapScanningDto param);
    }
}
