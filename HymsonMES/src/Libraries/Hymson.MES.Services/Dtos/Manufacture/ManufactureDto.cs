using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Parameter;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 进站
    /// </summary>
    public record InStationRequestDto
    {
        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }
        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }
        /// <summary>
        /// 条码类型
        /// </summary>
        public ManuFacePlateBarcodeTypeEnum Type { get; set; }
        /// <summary>
        /// 条码/托盘
        /// </summary>
        public IEnumerable<string> Params { get; set; }
    }

    /// <summary>
    /// 进站
    /// </summary>
    public record OutStationRequestDto
    {
        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }
        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }
        /// <summary>
        /// 条码类型
        /// </summary>
        public ManuFacePlateBarcodeTypeEnum Type { get; set; }
        /// <summary>
        /// 条码/托盘
        /// </summary>
        public IEnumerable<string> Params { get; set; }
    }

    /// <summary>
    /// 进站
    /// </summary>
    public record ProductProcessParameterDto : ProductProcessParameterBo { }

}
