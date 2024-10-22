/*
 *creator: Karl
 *
 *describe: 上料点表    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-02-17 08:57:53
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 上料点表 更新 验证
    /// </summary>
    internal class ProcLoadPointCreateValidator : AbstractValidator<ProcLoadPointCreateDto>
    {
        public ProcLoadPointCreateValidator()
        {
            RuleFor(x => x.LoadPoint).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10712));
            RuleFor(x => x.LoadPoint).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10714));
            RuleFor(x => x.LoadPointName).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10713));
            RuleFor(x => x.LoadPointName).MaximumLength(60).WithErrorCode(nameof(ErrorCode.MES10715));

            RuleFor(x => x.LinkMaterials).NotNull().WithErrorCode(nameof(ErrorCode.MES10718));
            RuleFor(x => x.LinkResources).NotNull().WithErrorCode(nameof(ErrorCode.MES10719));

        }
    }

    internal class ProcLoadPointImportValidator : AbstractValidator<ImportLoadPointDto>
    {
        public ProcLoadPointImportValidator()
        {
            RuleFor(x => x.LoadPoint).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10712));
            RuleFor(x => x.LoadPoint).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10714));
            RuleFor(x => x.LoadPointName).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10713));
            RuleFor(x => x.LoadPointName).MaximumLength(60).WithErrorCode(nameof(ErrorCode.MES10715));
        }
    }

    /// <summary>
    /// 上料点表 修改 验证
    /// </summary>
    internal class ProcLoadPointModifyValidator : AbstractValidator<ProcLoadPointModifyDto>
    {
        public ProcLoadPointModifyValidator()
        {
            RuleFor(x => x.LoadPoint).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10712));
            RuleFor(x => x.LoadPoint).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10714));
            RuleFor(x => x.LoadPointName).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10713));
            RuleFor(x => x.LoadPointName).MaximumLength(60).WithErrorCode(nameof(ErrorCode.MES10715));

            RuleFor(x => x.LinkMaterials).NotNull().WithErrorCode(nameof(ErrorCode.MES10718));
            RuleFor(x => x.LinkResources).NotNull().WithErrorCode(nameof(ErrorCode.MES10719));
        }
    }
}
