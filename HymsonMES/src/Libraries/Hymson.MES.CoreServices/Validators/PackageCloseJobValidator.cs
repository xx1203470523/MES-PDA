using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.CoreServices.Bos.Job;

namespace Hymson.MES.CoreServices.Validators
{
    /// <summary>
    /// 维修Job 验证(结束)
    /// </summary>
    internal class PackageCloseJobValidator : AbstractValidator<PackageCloseRequestBo>
    {
        /// <summary>
        ///  
        /// </summary>
        public PackageCloseJobValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(ErrorCode.MES16332);
            RuleFor(x => x.SiteId).NotEmpty().WithErrorCode(ErrorCode.MES10112);
            RuleFor(x => x.ProcedureId).NotEmpty().WithErrorCode(ErrorCode.MES16335);
            RuleFor(x => x.ResourceId).NotEmpty().WithErrorCode(ErrorCode.MES16334);

        }
    }
}
