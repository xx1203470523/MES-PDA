﻿using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Validators.Manufacture.Qkny.GlueHomogenate
{
    /// <summary>
    /// 验证器
    /// </summary>
    internal class FormulaDetailGetDtoVaildator : AbstractValidator<FormulaDetailGetDto>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FormulaDetailGetDtoVaildator()
        {
            //设备编码不能为空
            RuleFor(x => x.EquipmentCode).NotEmpty().NotNull().WithErrorCode(ErrorCode.MES45002);
            //资源编码不能为空
            RuleFor(x => x.ResourceCode).NotEmpty().NotNull().WithErrorCode(ErrorCode.MES45003);
        }
    }
}
