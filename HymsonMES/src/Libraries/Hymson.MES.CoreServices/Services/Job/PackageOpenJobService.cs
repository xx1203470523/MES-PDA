﻿using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.CoreServices.Services.Job.JobUtility;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using MySqlX.XDevAPI.Common;
using System.Threading.Tasks.Dataflow;
using System.Linq;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Newtonsoft.Json;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Mysqlx.Resultset;

namespace Hymson.MES.CoreServices.Services.NewJob
{
    /// <summary>
    /// 包装（打开）
    /// </summary>
    [Job("包装", JobTypeEnum.Standard)]
    public class PackageOpenJobService : IJobService
    {

        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;


        /// <summary>
        /// 
        /// </summary>
        private readonly IManuContainerBarcodeRepository _manuContainerBarcodeRepository;

        /// <summary>
        /// 验证器
        /// </summary>
        private readonly AbstractValidator<PackageOpenRequestBo> _validationRepairJob;
        /// <summary>
        /// 构造函数 
        /// </summary>
        /// <param name="manuCommonService"></param>
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        /// <param name="procProcessRouteDetailLinkRepository"></param>
        public PackageOpenJobService(IManuCommonService manuCommonService,
            AbstractValidator<PackageOpenRequestBo> validationRepairJob,
            IMasterDataService masterDataService,
            IManuContainerBarcodeRepository manuContainerBarcodeRepository)
        {
            _manuCommonService = manuCommonService;
            _validationRepairJob = validationRepairJob;
            _masterDataService = masterDataService;
            _manuContainerBarcodeRepository = manuContainerBarcodeRepository;
        }


        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <param name="proxy"></param> 
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<PackageOpenRequestBo>() ?? throw new CustomerValidationException(nameof(ErrorCode.MES10103));
            await _validationRepairJob.ValidateAndThrowAsync(bo);
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<PackageOpenRequestBo>() ?? throw new CustomerValidationException(nameof(ErrorCode.MES10103));
            var defaultDto = new PackageOpenResponseBo();
            string success = "true";
            var manuContainerBarcodeEntity = await param.Proxy.GetValueAsync(_manuContainerBarcodeRepository.GetByIdAsync, bo.ContainerId);
            if (manuContainerBarcodeEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16702));
            }
            int status = 1;//1打开，2关闭
            //当前状态不等于修改状态
            if (manuContainerBarcodeEntity.Status != status)
            {
                //修改容器状态
                manuContainerBarcodeEntity.Status = status;
                manuContainerBarcodeEntity.UpdatedBy = bo.UserName;
                manuContainerBarcodeEntity.UpdatedOn = HymsonClock.Now();
                defaultDto.Message = $"打开成功！";
            }
            else
            {
                success = "false";
                defaultDto.Message = $"该容器已经打开！";
            }

            defaultDto.Content?.Add("Operation", ManuContainerPackagJobReturnTypeEnum.JobManuPackageOpenService.ParseToInt().ToString());
            defaultDto.Content?.Add("Status", $"{status}".ToString());
            defaultDto.Content?.Add("Success", success);
            return defaultDto;
        }

        /// <summary>
        /// 执行入库
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo> ExecuteAsync(object obj)
        {
            JobResponseBo responseBo = new();

            if (obj is not PackageOpenResponseBo data) return responseBo;

            var rows = await _manuContainerBarcodeRepository.UpdateStatusAsync(data.ManuContainerBarcode);

            return new JobResponseBo { Content = data.Content, Message = data.Message, Rows = rows, Time = data.Time };
        }

    }
}