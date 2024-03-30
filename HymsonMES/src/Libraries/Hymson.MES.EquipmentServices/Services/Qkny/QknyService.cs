﻿using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.AgvTaskRecord;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.Qkny;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuCreateBarcode;
using Hymson.MES.CoreServices.Bos.Parameter;
using Hymson.MES.CoreServices.Dtos.Qkny;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.CoreServices.Services.Manufacture;
using Hymson.MES.CoreServices.Services.Manufacture.ManuCreateBarcode;
using Hymson.MES.CoreServices.Services.Qkny;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.View;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.LoadPoint.View;
using Hymson.MES.Data.Repositories.Process.LoadPointLink.Query;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.EquipmentServices.Dtos;
using Hymson.MES.EquipmentServices.Dtos.Manufacture.ProductionProcess;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Common;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using Hymson.MES.EquipmentServices.Dtos.Qkny.ProcSortingRule;
using Hymson.MES.EquipmentServices.Services.Manufacture;
using Hymson.MES.EquipmentServices.Services.Qkny.EquEquipment;
using Hymson.MES.EquipmentServices.Services.Qkny.Formula;
using Hymson.MES.EquipmentServices.Services.Qkny.InteVehicle;
using Hymson.MES.EquipmentServices.Services.Qkny.LoadPoint;
using Hymson.MES.EquipmentServices.Services.Qkny.PlanWorkOrder;
using Hymson.MES.EquipmentServices.Services.Qkny.PowerOnParam;
using Hymson.MES.EquipmentServices.Services.Qkny.ProcSortingRule;
using Hymson.MES.EquipmentServices.Services.Qkny.WhMaterialInventory;
using Hymson.MES.EquipmentServices.Validators.Manufacture.Qkny;
using Hymson.MES.Services.Dtos.AgvTaskRecord;
using Hymson.MES.Services.Dtos.CcdFileUploadCompleteRecord;
using Hymson.MES.Services.Dtos.EquEquipmentAlarm;
using Hymson.MES.Services.Dtos.EquEquipmentHeartRecord;
using Hymson.MES.Services.Dtos.EquEquipmentLoginRecord;
using Hymson.MES.Services.Dtos.EquProcessParamRecord;
using Hymson.MES.Services.Dtos.EquProductParamRecord;
using Hymson.MES.Services.Dtos.EquToolLifeRecord;
using Hymson.MES.Services.Dtos.ManuEquipmentStatusTime;
using Hymson.MES.Services.Dtos.ManuEuqipmentNewestInfo;
using Hymson.MES.Services.Dtos.ManuFeedingCompletedZjyjRecord;
using Hymson.MES.Services.Dtos.ManuFeedingNoProductionRecord;
using Hymson.MES.Services.Dtos.ManuFeedingTransferRecord;
using Hymson.MES.Services.Dtos.ManuFillingDataRecord;
using Hymson.MES.Services.Services.AgvTaskRecord;
using Hymson.MES.Services.Services.CcdFileUploadCompleteRecord;
using Hymson.MES.Services.Services.EquEquipmentAlarm;
using Hymson.MES.Services.Services.EquEquipmentHeartRecord;
using Hymson.MES.Services.Services.EquEquipmentLoginRecord;
using Hymson.MES.Services.Services.EquProcessParamRecord;
using Hymson.MES.Services.Services.EquProductParamRecord;
using Hymson.MES.Services.Services.EquToolLifeRecord;
using Hymson.MES.Services.Services.ManuEquipmentStatusTime;
using Hymson.MES.Services.Services.ManuEuqipmentNewestInfo;
using Hymson.MES.Services.Services.ManuFeedingCompletedZjyjRecord;
using Hymson.MES.Services.Services.ManuFeedingNoProductionRecord;
using Hymson.MES.Services.Services.ManuFeedingTransferRecord;
using Hymson.MES.Services.Services.ManuFillingDataRecord;
using Hymson.MessagePush.Helper;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.Web.Framework.Attributes;
using MailKit.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static Dapper.SqlMapper;

namespace Hymson.MES.EquipmentServices.Services.Qkny
{
    /// <summary>
    /// 顷刻设备服务
    /// </summary>
    public class QknyService : IQknyService
    {
        #region 验证器
        private readonly AbstractValidator<OperationLoginDto> _validationOperationLoginDto;
        #endregion

        /// <summary>
        /// 仓储接口（设备注册）
        /// </summary>
        private readonly IEquEquipmentRepository _equEquipmentRepository;

        /// <summary>
        /// 设备验证
        /// </summary>
        private readonly IEquEquipmentVerifyRepository _equEquipmentVerifyRepository;

        /// <summary>
        /// 登录记录
        /// </summary>
        private readonly IEquEquipmentLoginRecordService _equEquipmentLoginRecordService;

        /// <summary>
        /// 最新信息
        /// </summary>
        private readonly IManuEuqipmentNewestInfoService _manuEuqipmentNewestInfoService;

        /// <summary>
        /// 心跳记录
        /// </summary>
        private readonly IEquEquipmentHeartRecordService _equEquipmentHeartRecordService;

        /// <summary>
        /// 状态上报
        /// </summary>
        private readonly IManuEquipmentStatusTimeService _manuEquipmentStatusTimeService;

        /// <summary>
        /// 报警
        /// </summary>
        private readonly IEquEquipmentAlarmService _equEquipmentAlarmService;

        /// <summary>
        /// CCD文件上传
        /// </summary>
        private readonly ICcdFileUploadCompleteRecordService _ccdFileUploadCompleteRecordService;

        /// <summary>
        /// 开机参数
        /// </summary>
        private readonly IProcEquipmentGroupParamService _procEquipmentGroupParamService;

        /// <summary>
        /// 工单
        /// </summary>
        private readonly IPlanWorkOrderService _planWorkOrderService;

        /// <summary>
        /// 库存条码接收
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        /// 上料
        /// </summary>
        private readonly IManuFeedingService _manuFeedingService;

        /// <summary>
        /// 上料点关联资源
        /// </summary>
        private readonly IProcLoadPointLinkResourceRepository _procLoadPointLinkResourceRepository;

        /// <summary>
        /// AGV任务记录
        /// </summary>
        private readonly IAgvTaskRecordService _agvTaskRecordService;

        /// <summary>
        /// 配方
        /// </summary>
        private readonly IProcFormulaService _procFormulaService;

        /// <summary>
        /// 设备过程参数
        /// </summary>
        private readonly IEquProcessParamRecordService _equProcessParamRecordService;

        /// <summary>
        /// 业务接口（创建条码服务）
        /// </summary>
        private readonly IManuCreateBarcodeService _manuCreateBarcodeService;

        /// <summary>
        /// 在制品服务
        /// </summary>
        private readonly IManuSfcProduceService _manuSfcProduceService;

        /// <summary>
        /// 条码信息
        /// </summary>
        private readonly IManuSfcService _manuSfcServicecs;

        /// <summary>
        /// 产品参数
        /// </summary>
        private readonly IEquProductParamRecordService _equProductParamRecordService;

        /// <summary>
        /// 服务接口（过站）
        /// </summary>
        private readonly IManuPassStationService _manuPassStationService;

        /// <summary>
        /// 上料点
        /// </summary>
        private readonly IProcLoadPointService _procLoadPointService;

        /// <summary>
        /// 物料库存
        /// </summary>
        private readonly IWhMaterialInventoryService _whMaterialInventoryService;

        /// <summary>
        /// 上料完成记录(制胶匀浆)
        /// </summary>
        private readonly IManuFeedingCompletedZjyjRecordService _manuFeedingCompletedZjyjRecordService;

        /// <summary>
        /// 批次转移
        /// </summary>
        private readonly IManuFeedingTransferRecordService _manuFeedingTransferRecordService;

        /// <summary>
        /// 设备投料非生产投料
        /// </summary>
        private readonly IManuFeedingNoProductionRecordService _manuFeedingNoProductionRecordService;

        /// <summary>
        /// 补液数据上报
        /// </summary>
        private readonly IManuFillingDataRecordService _manuFillingDataRecordService;

        /// <summary>
        /// 载具
        /// </summary>
        private readonly IInteVehicleService _inteVehicleService;

        /// <summary>
        /// 通用接口
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// NG记录
        /// </summary>
        private readonly IManuProductNgRecordRepository _manuProductNgRecordRepository;

        /// <summary>
        /// 生产服务
        /// </summary>
        private readonly IManufactureService _manufactureService;

        /// <summary>
        /// 工装夹具寿命
        /// </summary>
        private readonly IEquToolLifeRecordService _equToolLifeRecordService;

        /// <summary>
        /// 分选规则
        /// </summary>
        private readonly IProcSortingRuleService _procSortingRuleService;

        /// <summary>
        /// 条码
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 条码步骤
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 工地那
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 设备服务
        /// </summary>
        private readonly IEquEquipmentService _equEquipmentService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public QknyService(IEquEquipmentRepository equEquipmentRepository,
            IEquEquipmentVerifyRepository equEquipmentVerifyRepository,
            IEquEquipmentLoginRecordService equEquipmentLoginRecordService,
            IManuEuqipmentNewestInfoService manuEuqipmentNewestInfoService,
            IEquEquipmentHeartRecordService equEquipmentHeartRecordService,
            IManuEquipmentStatusTimeService manuEquipmentStatusTimeService,
            IEquEquipmentAlarmService equEquipmentAlarmService,
            ICcdFileUploadCompleteRecordService ccdFileUploadCompleteRecordService,
            IProcEquipmentGroupParamService procEquipmentGroupParamService,
            IPlanWorkOrderService planWorkOrderService,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IManuFeedingService manuFeedingService,
            IProcLoadPointLinkResourceRepository procLoadPointLinkResourceRepository,
            IAgvTaskRecordService agvTaskRecordService,
            IEquProcessParamRecordService equProcessParamRecordService,
            IProcFormulaService procFormulaService,
            IManuCreateBarcodeService manuCreateBarcodeService,
            IManuSfcProduceService manuSfcProduceService,
            IManuSfcService manuSfcServicecs,
            IEquProductParamRecordService equProductParamRecordService,
            IManuPassStationService manuPassStationService,
            IProcLoadPointService procLoadPointService,
            IWhMaterialInventoryService whMaterialInventoryService,
            IManuFeedingCompletedZjyjRecordService manuFeedingCompletedZjyjRecordService,
            IManuFeedingTransferRecordService manuFeedingTransferRecordService,
            IManuFeedingNoProductionRecordService manuFeedingNoProductionRecordService,
            IManuFillingDataRecordService manuFillingDataRecordService,
            IInteVehicleService inteVehicleService,
            IManuCommonService manuCommonService,
            IManuProductNgRecordRepository manuProductNgRecordRepository,
            IManufactureService manufactureService,
            IEquToolLifeRecordService equToolLifeRecordService,
            IProcSortingRuleService procSortingRuleService,
            IManuSfcRepository manuSfcRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IEquEquipmentService equEquipmentService,
            AbstractValidator<OperationLoginDto> validationOperationLoginDto)
        {
            _equEquipmentRepository = equEquipmentRepository;
            _equEquipmentVerifyRepository = equEquipmentVerifyRepository;
            _equEquipmentLoginRecordService = equEquipmentLoginRecordService;
            _manuEuqipmentNewestInfoService = manuEuqipmentNewestInfoService;
            _equEquipmentHeartRecordService = equEquipmentHeartRecordService;
            _manuEquipmentStatusTimeService = manuEquipmentStatusTimeService;
            _equEquipmentAlarmService = equEquipmentAlarmService;
            _ccdFileUploadCompleteRecordService = ccdFileUploadCompleteRecordService;
            _procEquipmentGroupParamService = procEquipmentGroupParamService;
            _planWorkOrderService = planWorkOrderService;
            _whMaterialInventoryRepository = whMaterialInventoryRepository; 
            _manuFeedingService = manuFeedingService;
            _procLoadPointLinkResourceRepository = procLoadPointLinkResourceRepository;
            _agvTaskRecordService = agvTaskRecordService;
            _equProcessParamRecordService = equProcessParamRecordService;
            _procFormulaService = procFormulaService;
            _manuCreateBarcodeService = manuCreateBarcodeService;
            _manuSfcProduceService = manuSfcProduceService;
            _manuSfcServicecs = manuSfcServicecs;
            _equProductParamRecordService = equProductParamRecordService;
            _manuPassStationService = manuPassStationService;
            _procLoadPointService = procLoadPointService;
            _whMaterialInventoryService = whMaterialInventoryService;
            _manuFeedingCompletedZjyjRecordService = manuFeedingCompletedZjyjRecordService;
            _manuFeedingTransferRecordService = manuFeedingTransferRecordService;
            _manuFeedingNoProductionRecordService = manuFeedingNoProductionRecordService;
            _manuFillingDataRecordService = manuFillingDataRecordService;
            _inteVehicleService = inteVehicleService;
            _manuCommonService = manuCommonService;
            _manuProductNgRecordRepository = manuProductNgRecordRepository;
            _manufactureService = manufactureService;
            _equToolLifeRecordService = equToolLifeRecordService;
            _procSortingRuleService = procSortingRuleService;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _equEquipmentService = equEquipmentService;
            //校验器
            _validationOperationLoginDto = validationOperationLoginDto;
        }


        /// <summary>
        /// 原材料上料
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task FeedingAsync(FeedingDto dto)
        {
            ManuFeedingMaterialSaveDto saveDto = new ManuFeedingMaterialSaveDto();
            saveDto.BarCode = dto.Sfc;
            if (dto.IsFeedingPoint == false)
            {
                //1. 获取设备基础信息
                //EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
                EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResAsync(dto);
                //PlanWorkOrderEntity planEntity = await _planWorkOrderService.GetByWorkLineIdAsync(equResModel.LineId);

                saveDto.Source = ManuSFCFeedingSourceEnum.BOM;
                saveDto.SiteId = equResModel.SiteId;
                saveDto.ResourceId = equResModel.ResId;
                saveDto.UserName = dto.EquipmentCode;
            }
            else
            {
                //根据
                //ProcLoadPointCodeLinkResourceQuery query = new ProcLoadPointCodeLinkResourceQuery();
                //query.LoadPoint = dto.EquipmentCode;
                //var res = await _procLoadPointLinkResourceRepository.GetByCodeAsync(query);

                ProcLoadPointQuery query = new ProcLoadPointQuery();
                query.LoadPoint = dto.EquipmentCode;
                var loadPoint = await _procLoadPointService.GetProcLoadPointAsync(query);
                //saveDto.ResourceId = res.FirstOrDefault()!.ResourceId!;
                saveDto.Source = ManuSFCFeedingSourceEnum.FeedingPoint;
                saveDto.FeedingPointId = loadPoint.Id;
                saveDto.SiteId = loadPoint.SiteId;
                saveDto.UserName = dto.EquipmentCode;
            }
            //3. 上料
            var feedResult = await _manuFeedingService.CreateAsync(saveDto);

            //TODO
            //1. 校验物料是否在lims系统发过来的条码表lims_material(wh_material_inventory)，验证是否存在及合格，以及生成日期
            //2. 添加上料表信息 manu_feeding
            //3. 添加上料记录表信息 manu_feeding_record
            //4. 参考物料加载逻辑 ManuFeedingService.CreateAsync
        }

        /// <summary>
        /// 半成品上料
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task HalfFeedingAsync(HalfFeedingDto dto)
        {
            //TODO 和上料保持一致，使用BOM上料

            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await _equEquipmentService.GetEquResLineAsync(dto);
            PlanWorkOrderEntity planEntity = await _planWorkOrderService.GetByWorkLineIdAsync(equResModel.LineId);
            //2. 构造数据
            ManuFeedingMaterialSaveDto saveDto = new ManuFeedingMaterialSaveDto();
            saveDto.BarCode = dto.Sfc;
            saveDto.Source = ManuSFCFeedingSourceEnum.BOM;
            saveDto.SiteId = equResModel.SiteId;
            saveDto.ResourceId = equResModel.ResId;
            //3. 上料
            var feedResult = await _manuFeedingService.CreateAsync(saveDto);
        }

        /// <summary>
        /// AGV叫料
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task AgvMaterialAsync(AgvMaterialDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 校验设备是否激活工单
            PlanWorkOrderEntity planEntity = await _planWorkOrderService.GetByWorkLineIdAsync(equResModel.LineId);
            //3. 调用AGV接口
            var result = ""; //await HttpHelper.HttpPostAsync("", dto.Content, "");
            //4. 存储数据
            AgvTaskRecordSaveDto saveDto = new AgvTaskRecordSaveDto();
            saveDto.Id = IdGenProvider.Instance.CreateId();
            saveDto.SiteId = equResModel.SiteId;
            saveDto.EquipmentId = equResModel.EquipmentId;
            saveDto.SendContent = dto.Content;
            saveDto.TaskType = dto.Type;
            saveDto.ReceiveContent = result;
            saveDto.CreatedOn = HymsonClock.Now();
            saveDto.CreatedBy = dto.EquipmentCode;
            saveDto.UpdatedOn = saveDto.CreatedOn;
            saveDto.UpdatedBy = saveDto.CreatedBy;
            await _agvTaskRecordService.AddAsync(saveDto);
        }

        /// <summary>
        /// 请求产出极卷码023
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<List<string>> GenerateSfcAsync(GenerateSfcDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 构造数据
            //2.1 条码数据
            PlanWorkOrderEntity planEntity = await _planWorkOrderService.GetByWorkLineIdAsync(equResModel.LineId);
            CreateBarcodeByWorkOrderBo query = new CreateBarcodeByWorkOrderBo();
            query.WorkOrderId = planEntity.Id;
            query.ResourceId = equResModel.ResId;
            query.SiteId = equResModel.SiteId;
            query.Qty = dto.Qty;
            query.ProcedureId = equResModel.ProcedureId;
            query.UserName = equResModel.EquipmentCode;
            //2.2 进站数据
            SFCInStationBo inBo = new SFCInStationBo();
            inBo.SiteId = equResModel.SiteId;
            inBo.UserName = equResModel.EquipmentCode;
            inBo.ProcedureId = equResModel.ProcedureId;
            inBo.ResourceId = equResModel.ResId;
            inBo.EquipmentId = equResModel.EquipmentId;
            //3. 数据库操作
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            var sfcObjList = await _manuCreateBarcodeService.CreateBarcodeByWorkOrderIdAsync(query, null);
            List<string> sfcList = sfcObjList.Select(m => m.SFC).ToList();
            inBo.SFCs = sfcList.ToArray();
            //var inResult = await _manuPassStationService.InStationRangeBySFCAsync(inBo, RequestSourceEnum.EquipmentApi);
            trans.Complete();

            return sfcList;
        }

        /// <summary>
        /// 产出数量上报
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task OutboundMetersReportAsync(OutboundMetersReportDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 工单信息
            PlanWorkOrderEntity planEntity = await _planWorkOrderService.GetByWorkLineIdAsync(equResModel.LineId);
            //3. 查询条码信息
            var manuSfcInfo = await _manuSfcRepository.GetManSfcAboutInfoBySfcAsync(new ManuSfcAboutInfoBySfcQuery()
            {
                SiteId = equResModel.SiteId,
                Sfc = dto.Sfc,
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES12802)).WithData("sfc", dto.Sfc);
            //4. 校验工单数量
            //暂不校验

            //构造数据
            UpdateQtyBySfcCommand command = new UpdateQtyBySfcCommand();
            command.SFC = dto.Sfc;
            command.Qty = dto.OkQty;
            command.SiteId = equResModel.SiteId;
            command.UpdatedBy = dto.EquipmentCode;
            command.UpdatedOn = HymsonClock.Now();
            //产品参数
            List<EquProductParamRecordSaveDto> saveDtoList = new List<EquProductParamRecordSaveDto>();
            foreach (var item in dto.ParamList)
            {
                EquProductParamRecordSaveDto saveDto = new EquProductParamRecordSaveDto();
                saveDto.ParamCode = item.ParamCode;
                saveDto.ParamValue = item.ParamValue;
                saveDto.CollectionTime = item.CollectionTime;
                saveDtoList.Add(saveDto);
            }
            saveDtoList.ForEach(m =>
            {
                m.SiteId = equResModel.SiteId;
                m.Sfc = dto.Sfc;
                m.EquipmentId = equResModel.EquipmentId;
                m.CreatedOn = HymsonClock.Now();
                m.CreatedBy = dto.EquipmentCode;
                m.UpdatedOn = m.CreatedOn;
                m.UpdatedBy = m.CreatedBy;
            });
            //上料条码
            List<OutStationConsumeBo> consumeSfcList = new List<OutStationConsumeBo>();
            if(dto.OutputType == "1" || dto.OutputType == "2")
            {
                //查询当前设备的上料
                EntityByResourceIdQuery loadQuery = new EntityByResourceIdQuery();
                loadQuery.SiteId = equResModel.SiteId;
                loadQuery.Resourceid = equResModel.ResId;
                var loadList = await _manuFeedingService.GetAllByResourceIdAsync(loadQuery);
                foreach(var item in loadList)
                {
                    OutStationConsumeBo consumeSfc  = new OutStationConsumeBo();
                    consumeSfc.BarCode = item.BarCode;
                    consumeSfc.MaterialId = item.MaterialId;
                    consumeSfc.ConsumeQty = item.Qty;
                    consumeSfcList.Add(consumeSfc);
                }
            }
            //进出站数据
            SFCOutStationBo outBo = new SFCOutStationBo();
            outBo.SiteId = equResModel.SiteId;
            outBo.EquipmentId = equResModel.EquipmentId;
            outBo.ResourceId = equResModel.ResId;
            outBo.ProcedureId = equResModel.ProcedureId;
            outBo.UserName = equResModel.EquipmentCode;
            OutStationRequestBo outReqBo = new OutStationRequestBo();
            outReqBo.SFC = dto.Sfc;
            outReqBo.IsQualified = true;
            outReqBo.ConsumeList = consumeSfcList;
            outBo.OutStationRequestBos = new List<OutStationRequestBo>() { outReqBo };
            //步骤记录
            ManuSfcStepEntity sfcStep = new ManuSfcStepEntity()
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = equResModel.SiteId,
                SFC = dto.Sfc,
                ProductId = manuSfcInfo.ProductId,
                WorkOrderId = manuSfcInfo.WorkOrderId,
                ProductBOMId = planEntity.ProductBOMId,
                WorkCenterId = equResModel.LineId,
                Qty = dto.OkQty,
                ResourceId = equResModel.ResId,
                ProcedureId = equResModel.ProcedureId,
                Operatetype = ManuSfcStepTypeEnum.SfcQtyAdjust,
                CurrentStatus = manuSfcInfo.Status,
                CreatedBy = dto.EquipmentCode,
                UpdatedBy = dto.EquipmentCode,
                Remark = "设备上报产出更改"
            };
            //扣减/增可下达数量
            UpdatePassDownQuantityCommand updateQtyCommand = new UpdatePassDownQuantityCommand
            {
                WorkOrderId = planEntity.Id,
                PlanQuantity = planEntity.Qty * (1 + planEntity.OverScale / 100),
                PassDownQuantity = dto.TotalQty - manuSfcInfo.Qty,//再次下达的数量
                UserName = equResModel.EquipmentCode,
                UpdateDate = HymsonClock.Now()
            };

            // 数据库操作
            // 参考条码数量更改
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            await _manuSfcProduceService.UpdateQtyBySfcAsync(command);
            await _manuSfcServicecs.UpdateQtyBySfcAsync(command);
            await _equProductParamRecordService.AddMultAsync(saveDtoList);
            int updateNum = await _planWorkOrderRepository.UpdatePassDownQuantityByWorkOrderId(updateQtyCommand);
            if (updateNum == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16503)).WithData("workorder", planEntity.OrderCode);
            }
            await _manuPassStationService.OutStationRangeBySFCAsync(outBo, RequestSourceEnum.EquipmentApi);
            await _manuSfcStepRepository.InsertAsync(sfcStep);
            trans.Complete();
        }

        /// <summary>
        /// 获取下发条码(用于CCD面密度)025
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<CcdGetBarcodeReturnDto> CcdGetBarcodeAsync(CCDFileUploadCompleteDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 查询数据
            ManuSfcEquipmentNewestQuery query = new ManuSfcEquipmentNewestQuery();
            query.EquipmentId = equResModel.EquipmentId;
            var dbModel = await _manuSfcProduceService.GetEquipmentNewestSfc(query);
            //3. 构造数据
            CcdGetBarcodeReturnDto result = new CcdGetBarcodeReturnDto();
            result.Id = dbModel.Id.ToString();
            result.Sfc = dbModel.SFC;
            result.Qty = dbModel.Qty;
            result.ProductCode = dbModel.MaterialCode;
            return result;
        }

        /// <summary>
        /// 设备过程参数026
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task EquipmentProcessParamAsync(EquipmentProcessParamDto dto)
        {
            if (dto.ParamList == null || dto.ParamList.Count == 0)
            {
                return;
            }
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 添加数据
            List<EquProcessParamRecordSaveDto> saveDtoList = new List<EquProcessParamRecordSaveDto>();
            foreach (var item in dto.ParamList)
            {
                EquProcessParamRecordSaveDto saveDto = new EquProcessParamRecordSaveDto();
                saveDto.ParamCode = item.ParamCode;
                saveDto.ParamValue = item.ParamValue;
                saveDto.CollectionTime = item.CollectionTime;
                saveDtoList.Add(saveDto);
            }
            saveDtoList.ForEach(m =>
            {
                m.SiteId = equResModel.SiteId;
                m.EquipmentId = equResModel.EquipmentId;
                m.CreatedOn = HymsonClock.Now();
                m.CreatedBy = dto.EquipmentCode;
                m.UpdatedOn = m.CreatedOn;
                m.UpdatedBy = m.CreatedBy;
            });
            //3. 数据操作
            await _equProcessParamRecordService.AddMultAsync(saveDtoList);
        }

        /// <summary>
        /// 出站027
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task InboundAsync(InboundDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 构造数据
            SFCInStationBo inBo = new SFCInStationBo();
            inBo.SiteId = equResModel.SiteId;
            inBo.UserName = equResModel.EquipmentCode;
            inBo.ProcedureId = equResModel.ProcedureId;
            inBo.ResourceId = equResModel.ResId;
            inBo.EquipmentId = equResModel.EquipmentId;
            List<string> sfcList = new List<string>() { dto.Sfc };
            inBo.SFCs = sfcList.ToArray();
            //3. 进站
            var inResult = await _manuPassStationService.InStationRangeBySFCAsync(inBo, RequestSourceEnum.EquipmentApi);
        }

        /// <summary>
        /// 出站028
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task OutboundAsync(OutboundDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 构造数据
            //2.1 出站数据
            SFCOutStationBo outBo = new SFCOutStationBo();
            outBo.SiteId = equResModel.SiteId;
            outBo.EquipmentId = equResModel.EquipmentId;
            outBo.ResourceId = equResModel.ResId;
            outBo.ProcedureId = equResModel.ProcedureId;
            outBo.UserName = equResModel.EquipmentCode;
            OutStationRequestBo outReqBo = new OutStationRequestBo();
            outReqBo.SFC = dto.Sfc;
            outReqBo.IsQualified = dto.Passed == 1;
            if(dto.BindFeedingCodeList.IsNullOrEmpty() == false)
            {
                List<OutStationConsumeBo> conList = new List<OutStationConsumeBo>();
                foreach(var item in  dto.BindFeedingCodeList)
                {
                    conList.Add(new OutStationConsumeBo() { BarCode = item });
                }
                outReqBo.ConsumeList = conList;
            }
            if(dto.NgList.IsNullOrEmpty() == false)
            {
                List<OutStationUnqualifiedBo> unCodeList = new List<OutStationUnqualifiedBo>();
                foreach(var item in dto.NgList)
                {
                    unCodeList.Add(new OutStationUnqualifiedBo() { UnqualifiedCode = item });
                }
                outReqBo.OutStationUnqualifiedList = unCodeList;
            }
            outBo.OutStationRequestBos = new List<OutStationRequestBo>() { outReqBo };
            //2.2 出站参数
            List<EquProductParamRecordSaveDto> saveDtoList = new List<EquProductParamRecordSaveDto>();
            foreach (var item in dto.ParamList)
            {
                EquProductParamRecordSaveDto saveDto = new EquProductParamRecordSaveDto();
                saveDto.ParamCode = item.ParamCode;
                saveDto.ParamValue = item.ParamValue;
                saveDto.CollectionTime = item.CollectionTime;
                saveDtoList.Add(saveDto);
            }
            saveDtoList.ForEach(m =>
            {
                m.SiteId = equResModel.SiteId;
                m.Sfc = dto.Sfc;
                m.EquipmentId = equResModel.EquipmentId;
                m.CreatedOn = HymsonClock.Now();
                m.CreatedBy = dto.EquipmentCode;
                m.UpdatedOn = m.CreatedOn;
                m.UpdatedBy = m.CreatedBy;
            });
            //3. 出站
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            await _manuPassStationService.OutStationRangeBySFCAsync(outBo, RequestSourceEnum.EquipmentApi);
            await _equProductParamRecordService.AddMultAsync(saveDtoList);
            trans.Complete();
        }

        /// <summary>
        /// 进站多个029
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<List<InboundMoreReturnDto>> InboundMoreAsync(InboundMoreDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 构造数据
            SFCInStationBo inBo = new SFCInStationBo();
            inBo.SiteId = equResModel.SiteId;
            inBo.UserName = equResModel.EquipmentCode;
            inBo.ProcedureId = equResModel.ProcedureId;
            inBo.ResourceId = equResModel.ResId;
            inBo.EquipmentId = equResModel.EquipmentId;
            inBo.SFCs = dto.SfcList.ToArray();
            //3. 进站
            var inResult = await _manuPassStationService.InStationRangeBySFCAsync(inBo, RequestSourceEnum.EquipmentApi);
            //4. 返回
            List<InboundMoreReturnDto> resultList = new List<InboundMoreReturnDto>();
            foreach(var item in dto.SfcList)
            {
                InboundMoreReturnDto model = new InboundMoreReturnDto();
                model.Sfc = item;
                resultList.Add(model);
            }

            return resultList;
        }

        /// <summary>
        /// 出站多个
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<List<OutboundMoreReturnDto>> OutboundMoreAsync(OutboundMoreDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 构造数据
            SFCOutStationBo outBo = new SFCOutStationBo();
            outBo.SiteId = equResModel.SiteId;
            outBo.EquipmentId = equResModel.EquipmentId;
            outBo.ResourceId = equResModel.ResId;
            outBo.ProcedureId = equResModel.ProcedureId;
            outBo.UserName = equResModel.EquipmentCode;
            List<OutStationRequestBo> outStationRequestBos = new();
            List<EquProductParamRecordSaveDto> saveDtoList = new List<EquProductParamRecordSaveDto>();
            foreach (var item in dto.SfcList)
            {
                //出站数据
                var outStationRequestBo = new OutStationRequestBo
                {
                    SFC = item.Sfc,
                    IsQualified = item.Passed == 1
                };
                // 消耗条码
                if (item.BindFeedingCodeList != null && item.BindFeedingCodeList.Any())
                {
                    outStationRequestBo.ConsumeList = item.BindFeedingCodeList.Select(s => new OutStationConsumeBo { BarCode = s });
                }
                // 不合格代码
                if (item.NgList != null && item.NgList.Any())
                {
                    outStationRequestBo.OutStationUnqualifiedList = item.NgList.Select(s => new OutStationUnqualifiedBo { UnqualifiedCode = s });
                }
                outStationRequestBos.Add(outStationRequestBo);

                //出站参数
                List<EquProductParamRecordSaveDto> curSfcParamList = new List<EquProductParamRecordSaveDto>();
                foreach (var paramItem in item.ParamList)
                {
                    EquProductParamRecordSaveDto saveDto = new EquProductParamRecordSaveDto();
                    saveDto.ParamCode = paramItem.ParamCode;
                    saveDto.ParamValue = paramItem.ParamValue;
                    saveDto.CollectionTime = paramItem.CollectionTime;
                    curSfcParamList.Add(saveDto);
                }
                curSfcParamList.ForEach(m =>
                {
                    m.SiteId = equResModel.SiteId;
                    m.Sfc = item.Sfc;
                    m.EquipmentId = equResModel.EquipmentId;
                    m.CreatedOn = HymsonClock.Now();
                    m.CreatedBy = dto.EquipmentCode;
                    m.UpdatedOn = m.CreatedOn;
                    m.UpdatedBy = m.CreatedBy;
                });
                saveDtoList.AddRange(curSfcParamList);
            }
            outBo.OutStationRequestBos = outStationRequestBos;
            //3. 出站
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            var outResult = await _manuPassStationService.OutStationRangeBySFCAsync(outBo, RequestSourceEnum.EquipmentApi);
            await _equProductParamRecordService.AddMultAsync(saveDtoList);
            trans.Complete();
            //4. 返回
            List<OutboundMoreReturnDto> resultList = new List<OutboundMoreReturnDto>();
            foreach (var item in dto.SfcList)
            {
                OutboundMoreReturnDto model = new OutboundMoreReturnDto();
                model.Sfc = item.Sfc;
                resultList.Add(model);
            }

            return resultList;
        }

        /// <summary>
        /// 补液数据上报034
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task FillingDataAsync(FillingDataDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 构造数据
            ManuFillingDataRecordSaveDto saveDto = new ManuFillingDataRecordSaveDto();
            saveDto.Sfc = dto.Sfc;
            saveDto.EquipmentId = equResModel.EquipmentId;
            saveDto.InTime = dto.InTime;
            saveDto.OutTime = dto.OutTime;
            saveDto.BeforeWeight = dto.BeforeWeight;
            saveDto.AfterWeight = dto.AfterWeight;
            saveDto.ElWeight = dto.ElWeight;
            saveDto.AddEl = dto.AddEl;
            saveDto.TotalEl = dto.TotalEl;
            saveDto.ManualEl = dto.ManualEl;
            saveDto.FinalEl = dto.FinalEl;
            saveDto.IsOk = dto.IsOk;
            saveDto.CreatedBy = equResModel.EquipmentCode;
            saveDto.CreatedOn = HymsonClock.Now();
            saveDto.UpdatedBy = saveDto.CreatedBy;
            saveDto.UpdatedOn = saveDto.CreatedOn;
            await _manuFillingDataRecordService.AddAsync(saveDto);

            //TODO
            //1. 新增表进行记录
        }

        /// <summary>
        /// 空托盘校验035
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task EmptyContainerCheckAsync(EmptyContainerCheckDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 托盘校验
            //InteVehicleCodeQuery query = new InteVehicleCodeQuery();
            //query.Code = dto.ContainerCode;
            //query.SiteId = equResModel.SiteId;
            //await _inteVehicleService.GetByCodeAsync(query);
            //3. 校验托盘是否为空
            VehicleSFCRequestBo vehicleQuery = new VehicleSFCRequestBo();
            vehicleQuery.SiteId = equResModel.SiteId;
            vehicleQuery.VehicleCodes = new List<string>() { dto.ContainerCode };
            var vehicleList = await _manuCommonService.GetSfcListByVehicleCodesAsync(vehicleQuery);
            if(vehicleList.IsNullOrEmpty() == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45111));
            }

            //TODO
            //2. 校验托盘是否存在系统中（待确认）
            //3. 托盘(载具)表 inte_vehicle_freight_stack 是否存在数据
        }

        /// <summary>
        /// 单电芯校验036
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task ContainerSfcCheckAsync(ContainerSfcCheckDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 校验电芯是否存在
            ManuSfcProduceBySfcQuery query = new ManuSfcProduceBySfcQuery();
            query.SiteId = equResModel.SiteId;
            query.Sfc = dto.Sfc;
            var sfcEntity = await _manuSfcProduceService.GetBySFCAsync(query);
            //校验电芯是否合格
            if(sfcEntity.Status == SfcStatusEnum.Scrapping) 
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45091));
            }
        }

        /// <summary>
        /// 托盘电芯绑定(在制品容器)037
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task BindContainerAsync(BindContainerDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 托盘电芯绑定
            InteVehicleBindDto bindDto = new InteVehicleBindDto();
            bindDto.ContainerCode = dto.ContainerCode;
            foreach(var item in dto.ContainerSfcList)
            {
                bindDto.SfcList.Add(new InteVehicleSfcDto { Sfc = item.Sfc, Location = item.Location });
            }
            bindDto.SiteId = equResModel.SiteId;
            bindDto.UserName = dto.EquipmentCode;
            await _inteVehicleService.VehicleBindOperationAsync(bindDto);
            //TODO 添加进出站逻辑
            //拿完整的工艺路线进行测试
        }

        /// <summary>
        /// 托盘电芯解绑(在制品容器)038
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task UnBindContainerAsync(UnBindContainerDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 托盘电芯绑定
            InteVehicleUnBindDto bindDto = new InteVehicleUnBindDto();
            bindDto.ContainerCode = dto.ContainCode;
            bindDto.SfcList = dto.SfcList;
            bindDto.SiteId = equResModel.SiteId;
            bindDto.UserName = dto.EquipmentCode;
            await _inteVehicleService.VehicleUnBindOperationAsync(bindDto);
            //TODO 添加进出站逻辑
            //拿完整的工艺路线进行测试

            //TODO
            //1. 校验电芯是否在托盘中
            //2. inte_vehicle_freight_stack 删除绑定数据
            //3. 添加 inte_vehicle_freight_record 解绑记录
        }

        /// <summary>
        /// 托盘NG电芯上报039
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task ContainerNgReportAsync(ContainerNgReportDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 获取实际NG设备信息
            MultEquResAllQuery factQuery = new MultEquResAllQuery();
            factQuery.EquipmentCodeList = dto.NgSfcList.Select(m => m.NgEquipmentCode).ToList();
            factQuery.ResCodeList = dto.NgSfcList.Select(m => m.NgResourceCode).ToList();
            var factList = await GetMultEquResAllAsync(factQuery);
            if(factList.IsNullOrEmpty() == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45001));
            }
            //3. 组装数据
            InteVehicleNgSfcDto ngDto = new InteVehicleNgSfcDto();
            ngDto.ContainerCode = dto.ContainerCode;
            foreach(var item in dto.NgSfcList)
            {
                var sfcEquResModel = factList.Where(m => m.EquipmentCode == item.NgEquipmentCode && m.ResCode == item.NgResourceCode).FirstOrDefault();
                if(sfcEquResModel == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES45001));
                }

                InteVehicleSfcDetailDto ngModel = new InteVehicleSfcDetailDto();
                ngModel.NgCode = item.NgCode;
                ngModel.Sfc = item.Sfc;
                ngModel.OperationId = sfcEquResModel.ProcedureId;
                ngModel.ResourceId = sfcEquResModel.ResId;
                ngDto.NgSfcList.Add(ngModel);
            }
            ngDto.SiteId = equResModel.SiteId;
            ngDto.UserName = equResModel.EquipmentName;
            ngDto.OperationId = equResModel.ProcedureId;
            //数据库
            await _inteVehicleService.ContainerNgReportAsync(ngDto);
        }

        /// <summary>
        /// 托盘进站(容器进站)040
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task InboundInContainerAsync(InboundInContainerDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 托盘进站
            VehicleInStationBo inDto = new VehicleInStationBo();
            inDto.EquipmentId = equResModel.EquipmentId;
            inDto.ResourceId = equResModel.ResId;
            inDto.SiteId = equResModel.SiteId;
            inDto.ProcedureId = equResModel.ProcedureId;
            inDto.UserName = dto.EquipmentCode;
            inDto.VehicleCodes = new List<string> { dto.ContainerCode };
            await _manuPassStationService.InStationRangeByVehicleAsync(inDto, RequestSourceEnum.EquipmentApi);
        }

        /// <summary>
        /// 托盘出站(容器出站)041
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task OutboundInContainerAsync(OutboundInContainerDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 构造数据
            //2.1 托盘出站时护具
            VehicleOutStationBo outDto = new VehicleOutStationBo();
            outDto.EquipmentId = equResModel.EquipmentId;
            outDto.ResourceId = equResModel.ResId;
            outDto.SiteId = equResModel.SiteId;
            outDto.ProcedureId = equResModel.ProcedureId;
            outDto.UserName = dto.EquipmentCode;
            OutStationRequestBo outBo = new OutStationRequestBo();
            outBo.VehicleCode = dto.ContainerCode;
            outBo.IsQualified = true;
            outDto.OutStationRequestBos = new List<OutStationRequestBo>() { outBo };
            //2.2 托盘参数
            List<EquProductParamRecordSaveDto> saveDtoList = new List<EquProductParamRecordSaveDto>();
            foreach (var item in dto.ParamList)
            {
                EquProductParamRecordSaveDto saveDto = new EquProductParamRecordSaveDto();
                saveDto.ParamCode = item.ParamCode;
                saveDto.ParamValue = item.ParamValue;
                saveDto.CollectionTime = item.CollectionTime;
                saveDtoList.Add(saveDto);
            }
            saveDtoList.ForEach(m =>
            {
                m.SiteId = equResModel.SiteId;
                m.Sfc = dto.ContainerCode;
                m.EquipmentId = equResModel.EquipmentId;
                m.CreatedOn = HymsonClock.Now();
                m.CreatedBy = dto.EquipmentCode;
                m.UpdatedOn = m.CreatedOn;
                m.UpdatedBy = m.CreatedBy;
            });
            //3. 数据库操作
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            await _manuPassStationService.OutStationRangeByVehicleAsync(outDto, RequestSourceEnum.EquipmentApi);
            await _equProductParamRecordService.AddMultAsync(saveDtoList);
            trans.Complete();
        }

        /// <summary>
        /// 产品参数上传043
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task ProductParamAsync(ProductParamDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 出站参数
            List<EquProductParamRecordSaveDto> saveDtoList = new List<EquProductParamRecordSaveDto>();
            foreach (var item in dto.SfcList)
            {
                List<EquProductParamRecordSaveDto> sfcParamList = new List<EquProductParamRecordSaveDto>();
                foreach (var sfcItem in item.ParamList)
                {
                    EquProductParamRecordSaveDto saveDto = new EquProductParamRecordSaveDto();
                    saveDto.ParamCode = sfcItem.ParamCode;
                    saveDto.ParamValue = sfcItem.ParamValue;
                    saveDto.CollectionTime = sfcItem.CollectionTime;
                    sfcParamList.Add(saveDto);
                }
                sfcParamList.ForEach(m =>
                {
                    m.SiteId = equResModel.SiteId;
                    m.Sfc = item.Sfc;
                    m.EquipmentId = equResModel.EquipmentId;
                    m.CreatedOn = HymsonClock.Now();
                    m.CreatedBy = dto.EquipmentCode;
                    m.UpdatedOn = m.CreatedOn;
                    m.UpdatedBy = m.CreatedBy;
                });
                saveDtoList.AddRange(sfcParamList);
            }
            //3. 出站
            await _equProductParamRecordService.AddMultAsync(saveDtoList);
        }

        /// <summary>
        /// 卷绕极组产出上报
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task CollingPolarAsync(CollingPolarDto dto)
        {

        }

        /// <summary>
        /// 分选规则
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<List<ProcSortRuleDetailEquDto>> SortingRuleAsync(SortingRuleDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 查询激活的工单
            PlanWorkOrderEntity planEntity = await _planWorkOrderService.GetByWorkLineIdAsync(equResModel.LineId);
            //3. 查询分选规则
            ProcSortRuleDetailEquQuery query = new ProcSortRuleDetailEquQuery();
            query.MaterialId = planEntity.ProductId;
            var resultList = (await _procSortingRuleService.GetSortRuleDetailAsync(query)).ToList();
            //
            return resultList;
        }

        /// <summary>
        /// 获取设备资源对应的基础信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private async Task<EquEquipmentResAllView> GetEquResAllAsync(QknyBaseDto param)
        {
            EquResAllQuery query = new EquResAllQuery();
            query.EquipmentCode = param.EquipmentCode;
            query.ResCode = param.ResourceCode;
            EquEquipmentResAllView equResAllModel = await _equEquipmentRepository.GetEquResAllAsync(query);
            if (equResAllModel == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45001));
            }
            return equResAllModel;
        }

        /// <summary>
        /// 获取设备资源对应的基础信息
        /// 用于化成NG电芯上报，此时实际发生不良的工序是可能是上面的多个工序出现的
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private async Task<List<EquEquipmentResAllView>> GetMultEquResAllAsync(MultEquResAllQuery query)
        {
            var list = await _equEquipmentRepository.GetMultEquResAllAsync(query);
            if (list == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45001));
            }
            return list.ToList(); ;
        }
    }
}
