using FluentValidation;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 设备组关联设备表 验证
    /// </summary>
    internal class ProcProcessEquipmentGroupRelationSaveValidator: AbstractValidator<ProcProcessEquipmentGroupRelationSaveDto>
    {
        public ProcProcessEquipmentGroupRelationSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}