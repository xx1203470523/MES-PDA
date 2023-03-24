/*
 *creator: Karl
 *
 *describe: 条码生产信息（物理删除）    服务 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-03-18 05:37:27
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Manufacture.ManuSfcProduce;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Linq;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 条码生产信息（物理删除） 服务
    /// </summary>
    public class ManuSfcProduceService : IManuSfcProduceService
    {
        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 条码生产信息（物理删除） 仓储
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 条码步骤表仓储 仓储
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;
        private readonly AbstractValidator<ManuSfcProduceCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuSfcProduceModifyDto> _validationModifyRules;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ManuSfcProduceService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            AbstractValidator<ManuSfcProduceCreateDto> validationCreateRules,
            AbstractValidator<ManuSfcProduceModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuSfcProducePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcProduceViewDto>> GetPageListAsync(ManuSfcProducePagedQueryDto manuSfcProducePagedQueryDto)
        {
            var manuSfcProducePagedQuery = manuSfcProducePagedQueryDto.ToQuery<ManuSfcProducePagedQuery>();
            manuSfcProducePagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _manuSfcProduceRepository.GetPagedInfoAsync(manuSfcProducePagedQuery);

            //实体到DTO转换 装载数据
            List<ManuSfcProduceViewDto> manuSfcProduceDtos = new List<ManuSfcProduceViewDto>();
            foreach (var item in pagedInfo.Data)
            {
                manuSfcProduceDtos.Add(new ManuSfcProduceViewDto
                {
                    Id = item.Id,
                    Sfc = item.Sfc,
                    Lock = item.Lock,
                    LockProductionId = item.LockProductionId,
                    Status = item.Status,
                    OrderCode = item.OrderCode,
                    Code = item.Code,
                    Name = item.Name,
                    MaterialCode = item.MaterialCode,
                    MaterialName = item.MaterialName,
                    Version = item.Version
                });
            }
            return new PagedInfo<ManuSfcProduceViewDto>(manuSfcProduceDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 质量锁定
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task QualityLockAsync(ManuSfcProduceLockDto parm)
        {
            #region 验证
            if (parm == null)
            {
                throw new ValidationException(nameof(ErrorCode.MES10100));
            }

            if (parm.Sfcs == null || parm.Sfcs.Length < 1)
            {
                throw new ValidationException(nameof(ErrorCode.MES15301));
            }

            if (parm.Sfcs.Length > 100)
            {
                throw new ValidationException(nameof(ErrorCode.MES15305));
            }

            //查询条码信息,
            //校验条码状态，如果为报废或者删除，则提示：“条码已报废 / 删除，不可再操作锁定 / 取消锁定！
            var sfcs = parm.Sfcs.Distinct().ToArray();
            var query = new ManuSfcProduceQuery { Sfcs = sfcs };
            var sfcInfo = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(query);
            var sfcList = sfcInfo.ToList();
            if (sfcList.Count < sfcs.Length)
            {
                //比较有哪些条码查询不到就不是在制品
                var sfcInfos = sfcList.Select(a => a.Sfc).ToArray();
                var nprocessedSfcs = sfcs.Except(sfcInfos).ToArray();

                if (nprocessedSfcs != null && nprocessedSfcs.Length > 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15304)).WithData("Sfcs", string.Join("','", nprocessedSfcs));
                }
            }

            //如果是将来锁，需要选择工序
            if (parm.OperationType == QualityLockEnum.FutureLock)
            {
                if (!parm.LockProductionId.HasValue)
                {
                    throw new ValidationException(nameof(ErrorCode.MES15300));
                }

                //当操作类型为“将来锁定”时，扫描的条码状态都必须不是“锁定”，且没有未关闭的将来锁定指令存在（即已指定将来锁定工序，但暂未执行锁定）
                var sfcLocks = sfcInfo.Where(a => a.Lock != (int)QualityLockEnum.Unlock);
                if (sfcLocks.Any())
                {
                    throw new ValidationException(nameof(ErrorCode.MES15306));
                }

                //将来锁的工单一致
                var workOrders = sfcInfo.Select(a => a.WorkOrderId).Distinct().ToList();
                if (workOrders.Count > 1)
                {
                    throw new ValidationException(nameof(ErrorCode.MES15308));
                }

                //判断工序,产品条码 > 工单 > 工艺路线 > 工序
                //只能选择列表中产品条码所在工序之后的工序
            }

            //当操作类型为“即时锁定”时，扫描的条码状态都必须不是锁定状态
            if (parm.OperationType == QualityLockEnum.InstantLock)
            {
                //将来锁定改为即时锁定，需要修改锁定类型，去掉将来锁定工序  
                var sfcLocks = sfcInfo.Where(a => a.Lock == (int)QualityLockEnum.InstantLock);
                if (sfcLocks.Any())
                {
                    throw new ValidationException(nameof(ErrorCode.MES15306));
                }
            }

            //当操作类型为“取消锁定”时，扫描的条码状态都必须是“锁定”或者有未关闭的将来锁定指令存在（即已指定将来锁定工序，但暂未执行锁定）
            if (parm.OperationType == QualityLockEnum.Unlock)
            {
                var sfcLocks = sfcInfo.Where(a => a.Lock != (int)QualityLockEnum.Unlock);
                if (sfcLocks.Any())
                {
                    throw new ValidationException(nameof(ErrorCode.MES15307));
                }
            }
            #endregion

            #region  组装数据

            /* 1.即时锁定：将条码更新为“锁定”状态；
               2.将来锁定：保存列表中的条码信息，及指定锁定的工序，供条码过站校验时调用；
               3.取消锁定：产品条码已经是锁定状态：将条码更新到锁定前状态
              指定将来锁定工序，且条码还没流转到指定工序：关闭将来锁定的工序指定，即取消将来锁定*/
            var lockCommand = new QualityLockCommand
            {
                Lock = (int)parm.OperationType,
                Sfcs = parm.Sfcs,
                LockProductionId = parm.LockProductionId??0,
                UserId = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now()
            };

            int type = 0;
            if (parm.OperationType == QualityLockEnum.Unlock)
            {
                type = (int)ManuSfcStepTypeEnum.Unlock;
            }
            if (parm.OperationType == QualityLockEnum.InstantLock)
            {
                type = (int)ManuSfcStepTypeEnum.InstantLock;
            }
            if (parm.OperationType == QualityLockEnum.FutureLock)
            {
                type = (int)ManuSfcStepTypeEnum.FutureLock;
            }

            var sfcStepList = new List<ManuSfcStepEntity>();
            foreach (var sfc in sfcList)
            {
                sfcStepList.Add(new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = sfc.Sfc,
                    ProductId = sfc.ProductId,
                    WorkOrdeId = sfc.WorkOrderId,
                    WorkCenterId = sfc.WorkCenterId,
                    ProductBOMId = sfc.ProductBOMId,
                    Qty = sfc.Qty,
                    EquipmentId = sfc.EquipmentId,
                    ResourceId = sfc.ResourceId,
                    ProcedureId = sfc.ProcedureId,
                    Type = type,
                    Status = sfc.Status,
                    Lock = lockCommand.Lock,
                    SiteId = _currentSite.SiteId ?? 0,
                    CreatedBy = sfc.CreatedBy,
                    UpdatedBy = sfc.UpdatedBy
                });
            }
            #endregion

            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //插入操作数据
                await _manuSfcStepRepository.InsertRangeAsync(sfcStepList);

                //修改状态
                await _manuSfcProduceRepository.QualityLockAsync(lockCommand);
                trans.Complete();
            }
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="manuSfcProduceCreateDto"></param>
        /// <returns></returns>
        public async Task CreateManuSfcProduceAsync(ManuSfcProduceCreateDto manuSfcProduceCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(manuSfcProduceCreateDto);

            //DTO转换实体
            var manuSfcProduceEntity = manuSfcProduceCreateDto.ToEntity<ManuSfcProduceEntity>();
            manuSfcProduceEntity.Id = IdGenProvider.Instance.CreateId();
            manuSfcProduceEntity.CreatedBy = _currentUser.UserName;
            manuSfcProduceEntity.UpdatedBy = _currentUser.UserName;

            //入库
            await _manuSfcProduceRepository.InsertAsync(manuSfcProduceEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteManuSfcProduceAsync(long id)
        {
            await _manuSfcProduceRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesManuSfcProduceAsync(string ids)
        {
            var idsArr = StringExtension.SpitLongArrary(ids);
            return await _manuSfcProduceRepository.DeleteRangeAsync(idsArr);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuSfcProduceModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyManuSfcProduceAsync(ManuSfcProduceModifyDto manuSfcProduceModifyDto)
        {
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuSfcProduceModifyDto);

            //DTO转换实体
            var manuSfcProduceEntity = manuSfcProduceModifyDto.ToEntity<ManuSfcProduceEntity>();
            manuSfcProduceEntity.UpdatedBy = _currentUser.UserName;

            await _manuSfcProduceRepository.UpdateAsync(manuSfcProduceEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuSfcProduceDto> QueryManuSfcProduceByIdAsync(long id)
        {
            var manuSfcProduceEntity = await _manuSfcProduceRepository.GetByIdAsync(id);
            if (manuSfcProduceEntity == null)
            {
                return null;
            }
            return manuSfcProduceEntity.ToModel<ManuSfcProduceDto>();
        }
    }
}