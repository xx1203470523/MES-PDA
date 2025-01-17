using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MessagePush.Enum;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 消息组推送方式新增/更新Dto
    /// </summary>
    public record InteMessageGroupPushMethodSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 推送类型;1、企微2、钉钉3、邮箱
        /// </summary>
        public MessageTypeEnum Type { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 秘钥
        /// </summary>
        public string? SecretKey { get; set; }

        /// <summary>
        /// 关键词
        /// </summary>
        public string? KeyWord { get; set; }

    }

    /// <summary>
    /// 消息组推送方式Dto
    /// </summary>
    public record InteMessageGroupPushMethodDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 推送类型;1、企微2、钉钉3、邮箱
        /// </summary>
        public MessageTypeEnum Type { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 秘钥
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 关键词
        /// </summary>
        public string KeyWord { get; set; }

    }

    /// <summary>
    /// 消息组推送方式分页Dto
    /// </summary>
    public class InteMessageGroupPushMethodPagedQueryDto : PagerInfo { }

}
