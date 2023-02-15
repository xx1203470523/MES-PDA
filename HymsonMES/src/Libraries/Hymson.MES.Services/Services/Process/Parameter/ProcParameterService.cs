/*
 *creator: Karl
 *
 *describe: 标准参数表    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-13 02:50:20
 */
using FluentValidation;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Extensions;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 标准参数表 服务
    /// </summary>
    public class ProcParameterService : IProcParameterService
    {
        /// <summary>
        /// 标准参数表 仓储
        /// </summary>
        private readonly IProcParameterRepository _procParameterRepository;
        private readonly AbstractValidator<ProcParameterCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcParameterModifyDto> _validationModifyRules;

        private readonly IProcParameterLinkTypeRepository _procParameterLinkTypeRepository;

        public ProcParameterService(IProcParameterRepository procParameterRepository, AbstractValidator<ProcParameterCreateDto> validationCreateRules, AbstractValidator<ProcParameterModifyDto> validationModifyRules, IProcParameterLinkTypeRepository procParameterLinkTypeRepository)
        {
            _procParameterRepository = procParameterRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _procParameterLinkTypeRepository = procParameterLinkTypeRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procParameterCreateDto"></param>
        /// <returns></returns>
        public async Task CreateProcParameterAsync(ProcParameterCreateDto procParameterCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(procParameterCreateDto);

            //DTO转换实体
            var procParameterEntity = procParameterCreateDto.ToEntity<ProcParameterEntity>();
            procParameterEntity.Id= IdGenProvider.Instance.CreateId();
            procParameterEntity.CreatedBy = "TODO";
            procParameterEntity.UpdatedBy = "TODO";
            procParameterEntity.CreatedOn = HymsonClock.Now();
            procParameterEntity.UpdatedOn = HymsonClock.Now();
            procParameterEntity.ParameterCode = procParameterEntity.ParameterCode.ToUpper();

            //判断编号是否已经存在
            var exists = await _procParameterRepository.GetProcParameterEntitiesAsync(new ProcParameterQuery()
            {
                SiteCode = procParameterEntity.SiteCode,
                ParameterCode = procParameterEntity.ParameterCode,
            });
            if (exists != null && exists.Count() > 0) 
            {
                throw new BusinessException(ErrorCode.MES10502).WithData("parameterCode", procParameterEntity.ParameterCode);
            }

            //入库
            await _procParameterRepository.InsertAsync(procParameterEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteProcParameterAsync(long id)
        {
            await _procParameterRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesProcParameterAsync(string ids)
        {
            if (string.IsNullOrEmpty(ids)) 
            {
                throw new ValidationException(ErrorCode.MES10505);
            }

            var idsArr = StringExtension.SpitLongArrary(ids);

            //查询参数是否关联产品参数和设备参数
            var lists= await _procParameterLinkTypeRepository.GetByParameterIdsAsync(idsArr);
            if (lists!=null&&lists.Count() > 0)
            {
                throw new BusinessException(ErrorCode.MES10506);
            }

            return await _procParameterRepository.DeletesAsync(idsArr);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procParameterPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcParameterDto>> GetPageListAsync(ProcParameterPagedQueryDto procParameterPagedQueryDto)
        {
            procParameterPagedQueryDto.SiteCode = "";//TODO

            var procParameterPagedQuery = procParameterPagedQueryDto.ToQuery<ProcParameterPagedQuery>();
            var pagedInfo = await _procParameterRepository.GetPagedInfoAsync(procParameterPagedQuery);

            //实体到DTO转换 装载数据
            List<ProcParameterDto> procParameterDtos = PrepareProcParameterDtos(pagedInfo);
            return new PagedInfo<ProcParameterDto>(procParameterDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcParameterDto> PrepareProcParameterDtos(PagedInfo<ProcParameterEntity>   pagedInfo)
        {
            var procParameterDtos = new List<ProcParameterDto>();
            foreach (var procParameterEntity in pagedInfo.Data)
            {
                var procParameterDto = procParameterEntity.ToModel<ProcParameterDto>();
                procParameterDtos.Add(procParameterDto);
            }

            return procParameterDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procParameterModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyProcParameterAsync(ProcParameterModifyDto procParameterModifyDto)
        {
            if (procParameterModifyDto == null)
            {
                throw new ValidationException(ErrorCode.MES10503);
            }

            //DTO转换实体
            var procParameterEntity = procParameterModifyDto.ToEntity<ProcParameterEntity>();
            procParameterEntity.UpdatedBy = "TODO";
            procParameterEntity.UpdatedOn = HymsonClock.Now();

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(procParameterModifyDto);

            var modelOrigin = await _procParameterRepository.GetByIdAsync(procParameterEntity.Id);
            if (modelOrigin == null)
            {
                throw new BusinessException(ErrorCode.MES10504);
            }
            //判断编号是否已经存在
            var exists = await _procParameterRepository.GetProcParameterEntitiesAsync(new ProcParameterQuery()
            {
                SiteCode = procParameterEntity.SiteCode,
                ParameterCode = procParameterEntity.ParameterCode,
            });
            if (exists != null && exists.Count() > 0)
            {
                throw new BusinessException(ErrorCode.MES10502).WithData("parameterCode", procParameterEntity.ParameterCode);
            }

            await _procParameterRepository.UpdateAsync(procParameterEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcParameterDto> QueryProcParameterByIdAsync(long id) 
        {
            var siteCode = "";//TODO

           var procParameterEntity = await _procParameterRepository.GetByIdAsync(id);
           if (procParameterEntity != null) 
           {
               var dto= procParameterEntity.ToModel<CustomProcParameterDto>();
               var linkTypes =  await _procParameterLinkTypeRepository.GetProcParameterLinkTypeEntitiesAsync(new ProcParameterLinkTypeQuery()
                {
                   SiteCode= siteCode,
                   ParameterID= dto.Id
               });
               dto.Type = $"{linkTypes.GroupBy(x => x.ParameterType).Select(x => x.Key).ToList().Sum()}";
           }
            return null;
        }
    }
}