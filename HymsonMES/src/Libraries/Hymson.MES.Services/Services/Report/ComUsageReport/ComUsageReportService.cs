using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcInfo.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.IQualityRepository;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report;
using Hymson.Minio;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Transactions;

namespace Hymson.MES.Services.Services.Report
{
    /// <summary>
    /// 不良记录报表 服务
    /// </summary>
    public class ComUsageReportService : IComUsageReportService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 表 仓储
        /// </summary>
        private readonly IManuSfcCirculationRepository _circulationRepository;

        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IExcelService _excelService;
        private readonly IMinioService _minioService;
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        public ComUsageReportService(ICurrentUser currentUser, ICurrentSite currentSite, IProcProcedureRepository procProcedureRepository, IPlanWorkOrderRepository planWorkOrderRepository, IProcMaterialRepository procMaterialRepository, IManuSfcCirculationRepository circulationRepository, IExcelService excelService, IMinioService minioService, ILocalizationService localizationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;

            _procProcedureRepository = procProcedureRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procMaterialRepository = procMaterialRepository;
            _circulationRepository = circulationRepository;
            _excelService = excelService;
            _minioService = minioService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// 根据查询条件获取车间作业控制报表分页数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ComUsageReportViewDto>> GetComUsagePageListAsync(ComUsageReportPagedQueryDto param)
        {
            var pagedQuery = param.ToQuery<ComUsageReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId.Value;
            var pagedInfo = await _circulationRepository.GetReportPagedInfoAsync(pagedQuery);

            List<ComUsageReportViewDto> listDto = new List<ComUsageReportViewDto>();

            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                return new PagedInfo<ComUsageReportViewDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
            }

            var circulationProductIds = pagedInfo.Data.Select(x => x.CirculationProductId).Distinct().ToList();
            var productIds = pagedInfo.Data.Select(x => x.ProductId).Distinct().ToList();

            //合并 成 materialIds
            circulationProductIds.AddRange(productIds);
            var materialIds = circulationProductIds.ToArray();
            var materials = await _procMaterialRepository.GetByIdsAsync(materialIds);

            var workOrderIds = pagedInfo.Data.Select(x => x.WorkOrderId).Distinct().ToArray();
            var workOrders = await _planWorkOrderRepository.GetByIdsAsync(workOrderIds);

            foreach (var item in pagedInfo.Data)
            {
                var product = materials != null && materials.Any() ? materials.Where(x => x.Id == item.ProductId).FirstOrDefault() : null;
                var circulationProduct= materials != null && materials.Any() ? materials.Where(x => x.Id == item.CirculationProductId).FirstOrDefault() : null;

                var workOrder = materials != null && materials.Any() ? workOrders.Where(x => x.Id == item.WorkOrderId).FirstOrDefault() : null;

                listDto.Add(new ComUsageReportViewDto()
                {
                    SFC = item.SFC,
                    ProductCodeVersion = product != null ? product.MaterialCode + "/" + product.Version : "",
                    OrderCode= workOrder!=null? workOrder.OrderCode : "",
                    CirculationBarCode=item.CirculationBarCode,
                    CirculationProductCodeVersion= circulationProduct != null ? circulationProduct.MaterialCode + "/" + circulationProduct.Version : "",
                });
            }

            return new PagedInfo<ComUsageReportViewDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }


        /// <summary>
        /// 根据查询条件导出车间作业控制报表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ComUsageExportResultDto> ExprotComUsagePageListAsync(ComUsageReportPagedQueryDto param)
        {
            var pagedQuery = param.ToQuery<ComUsageReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId.Value;
            pagedQuery.PageSize = 1000;
            var pagedInfo = await _circulationRepository.GetReportPagedInfoAsync(pagedQuery);

            List<ComUsageReportExcelExportDto> listDto = new List<ComUsageReportExcelExportDto>();

            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                var filePathN = await _excelService.ExportAsync(listDto, _localizationService.GetResource("ComUsageReport"), _localizationService.GetResource("ComUsageReport"));
                //上传到文件服务器
                var uploadResultN = await _minioService.PutObjectAsync(filePathN);
                return new ComUsageExportResultDto
                {
                    FileName = _localizationService.GetResource("ComUsageReport"),
                    Path = uploadResultN.AbsoluteUrl,
                };
            }

            var circulationProductIds = pagedInfo.Data.Select(x => x.CirculationProductId).Distinct().ToList();
            var productIds = pagedInfo.Data.Select(x => x.ProductId).Distinct().ToList();

            //合并 成 materialIds
            circulationProductIds.AddRange(productIds);
            var materialIds = circulationProductIds.ToArray();
            var materials = await _procMaterialRepository.GetByIdsAsync(materialIds);

            var workOrderIds = pagedInfo.Data.Select(x => x.WorkOrderId).Distinct().ToArray();
            var workOrders = await _planWorkOrderRepository.GetByIdsAsync(workOrderIds);

            foreach (var item in pagedInfo.Data)
            {
                var product = materials != null && materials.Any() ? materials.Where(x => x.Id == item.ProductId).FirstOrDefault() : null;
                var circulationProduct = materials != null && materials.Any() ? materials.Where(x => x.Id == item.CirculationProductId).FirstOrDefault() : null;

                var workOrder = materials != null && materials.Any() ? workOrders.Where(x => x.Id == item.WorkOrderId).FirstOrDefault() : null;

                listDto.Add(new ComUsageReportExcelExportDto()
                {
                    SFC = item.SFC,
                    ProductCodeVersion = product != null ? product.MaterialCode + "/" + product.Version : "",
                    OrderCode = workOrder != null ? workOrder.OrderCode : "",
                    CirculationBarCode = item.CirculationBarCode,
                    CirculationProductCodeVersion = circulationProduct != null ? circulationProduct.MaterialCode + "/" + circulationProduct.Version : "",
                });
            }

            var filePath = await _excelService.ExportAsync(listDto, _localizationService.GetResource("ComUsageReport") , _localizationService.GetResource("ComUsageReport"));
            //上传到文件服务器
            var uploadResult = await _minioService.PutObjectAsync(filePath);
            return new ComUsageExportResultDto
            {
                FileName = _localizationService.GetResource("ComUsageReport"),
                Path = uploadResult.AbsoluteUrl,
            };
        }

    }
}
