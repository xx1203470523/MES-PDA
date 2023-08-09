﻿using Hymson.MES.EquipmentServices.Dtos.Parameter;
using Hymson.MES.EquipmentServices.Services.Parameter.ProductProcessCollection;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Equipment.Api.Controllers.Parameter
{
    /// <summary>
    /// 控制器（参数）
    /// @author wangkeming
    /// @date 2023-08-07
    /// </summary>

    [ApiController]
    //[AllowAnonymous]
    [Route("EquipmentService/api/v1/[controller]")]
    public class ParameterController : ControllerBase
    {
        /// <summary>
        /// 参数采集
        /// </summary>
        private readonly IProductProcessCollectionService _productProcessCollectionService;

        /// <summary>
        /// 参数采集
        /// </summary>
        /// <param name="productProcessCollectionService"></param>
        public ParameterController(IProductProcessCollectionService productProcessCollectionService)
        {
            _productProcessCollectionService = productProcessCollectionService;
        }

        /// <summary>
        ///产品过程参数采集
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("collection")]
        public async Task Collection(ProductProcessParameterDto param)
        {
             await _productProcessCollectionService.Collection(param);
        }
    }
}
