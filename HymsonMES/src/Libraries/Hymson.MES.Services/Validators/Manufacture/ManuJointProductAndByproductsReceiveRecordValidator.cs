using FluentValidation;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Validators.Manufacture
{
    /// <summary>
    /// 联副产品收货 验证
    /// </summary>
    internal class ManuJointProductAndByproductsReceiveRecordSaveValidator: AbstractValidator<ManuJointProductAndByproductsReceiveRecordSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public ManuJointProductAndByproductsReceiveRecordSaveValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111");
        }
    }

}