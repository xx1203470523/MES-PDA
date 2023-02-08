/*
 *creator: Karl
 *
 *describe: 物料维护    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-02-08 02:32:38
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 物料维护 更新 验证
    /// </summary>
    internal class ProcMaterialCreateValidator: AbstractValidator<ProcMaterialCreateDto>
    {
        public ProcMaterialCreateValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }

    /// <summary>
    /// 物料维护 修改 验证
    /// </summary>
    internal class ProcMaterialModifyValidator : AbstractValidator<ProcMaterialModifyDto>
    {
        public ProcMaterialModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }
}
