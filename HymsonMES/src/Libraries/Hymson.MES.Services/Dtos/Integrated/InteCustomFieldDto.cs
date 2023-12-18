using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Services.Dtos.Integrated
{
    public record InteCustomFieldSaveOrUpdateDto : BaseEntityDto
    {
        public IEnumerable<InteCustomFieldSaveDto> InteCustomFieldSaveDtos { get; set; }
    }

    /// <summary>
    /// 自定义字段新增/更新Dto
    /// </summary>
    public record InteCustomFieldSaveDto : BaseEntityDto
    {
       /// <summary>
        /// 业务类型
        /// </summary>
        public InteCustomFieldBusinessTypeEnum BusinessType { get; set; }

       /// <summary>
        /// 字段名
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 语言设置
        /// </summary>
        public IEnumerable<InteCustomFieldInternationalizationDto>? Languages { get; set; }
    }

    /// <summary>
    /// 字段国际化Dto
    /// </summary>
    public record InteCustomFieldInternationalizationDto : BaseEntityDto
    {
        /// <summary>
        /// 语言类型
        /// </summary>
        public string LanguageType { get; set; }

        /// <summary>
        /// 翻译值
        /// </summary>
        public string TranslationValue { get; set; }
    }

    public record InteCustomFieldDto : BaseEntityDto
    {
        /// <summary>
        /// 业务类型
        /// </summary>
        public InteCustomFieldBusinessTypeEnum BusinessType { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 语言设置
        /// </summary>
        public IEnumerable<InteCustomFieldInternationalizationDto>? Languages { get; set; }
    }

}
