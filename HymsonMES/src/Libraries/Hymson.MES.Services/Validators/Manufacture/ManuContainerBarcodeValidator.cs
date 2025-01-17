/*
 *creator: Karl
 *
 *describe: 容器条码表    验证规则 | 代码由框架生成  
 *builder:  wxk
 *build datetime: 2023-04-12 02:29:23
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Validators.Manufacture
{
    /// <summary>
    /// 容器条码表 更新 验证
    /// </summary>
    internal class ManuContainerBarcodeCreateValidator: AbstractValidator<ManuContainerBarcodeCreateDto>
    {
        public ManuContainerBarcodeCreateValidator()
        {

        }
    }

    /// <summary>
    /// 容器条码表 修改 验证
    /// </summary>
    internal class ManuContainerBarcodeModifyValidator : AbstractValidator<ManuContainerBarcodeModifyDto>
    {
        public ManuContainerBarcodeModifyValidator()
        {

        }
    }

    /// <summary>
    /// 包装时验证
    /// </summary>
    internal class CreateManuContainerBarcodeValidator : AbstractValidator<CreateManuContainerBarcodeDto>
    {
        public CreateManuContainerBarcodeValidator()
        {
            RuleFor(x => x.FacePlateCode).NotEmpty().WithErrorCode("MES16704");

        }
    }

    /// <summary>
    /// 更新状态验证
    /// </summary>
    internal class UpdateManuContainerBarcodeStatusValidator: AbstractValidator<UpdateManuContainerBarcodeStatusDto>
    {
        public UpdateManuContainerBarcodeStatusValidator()
        {

        }
    }
}
