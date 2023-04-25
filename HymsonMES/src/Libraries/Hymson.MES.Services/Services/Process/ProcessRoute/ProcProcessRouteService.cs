/*
 *creator: Karl
 *
 *describe: 工艺路线表    服务 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-14 10:07:11
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process.ProcessRoute
{
    /// <summary>
    /// 工艺路线表 服务
    /// </summary>
    public class ProcProcessRouteService : IProcProcessRouteService
    {
        /// <summary>
        /// 节点ID（开始）
        /// </summary>
        const int StartNodeId = 0;
        /// <summary>
        /// 节点ID（结束）
        /// </summary>
        const int EndNodeId = 999999999;

        /// <summary>
        /// 当前登录用户对象
        /// </summary>
        private readonly ICurrentUser _currentUser;
        /// <summary>
        /// 当前站点
        /// </summary>
        private readonly ICurrentSite _currentSite;
        /// <summary>
        /// 工艺路线表 仓储
        /// </summary>
        private readonly IProcProcessRouteRepository _procProcessRouteRepository;
        /// <summary>
        /// 仓储（工艺路线节点）
        /// </summary>
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteNodeRepository;
        /// <summary>
        /// 仓储（工艺路线连线）
        /// </summary>
        private readonly IProcProcessRouteDetailLinkRepository _procProcessRouteLinkRepository;
        private readonly AbstractValidator<ProcProcessRouteCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcProcessRouteModifyDto> _validationModifyRules;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcProcessRouteService(ICurrentUser currentUser, ICurrentSite currentSite,
            IProcProcessRouteRepository procProcessRouteRepository,
            IProcProcessRouteDetailNodeRepository procProcessRouteNodeRepository,
            IProcProcessRouteDetailLinkRepository procProcessRouteLinkRepository,
            AbstractValidator<ProcProcessRouteCreateDto> validationCreateRules,
            AbstractValidator<ProcProcessRouteModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _procProcessRouteRepository = procProcessRouteRepository;
            _procProcessRouteNodeRepository = procProcessRouteNodeRepository;
            _procProcessRouteLinkRepository = procProcessRouteLinkRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procProcessRoutePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProcessRouteDto>> GetPageListAsync(ProcProcessRoutePagedQueryDto procProcessRoutePagedQueryDto)
        {
            var procProcessRoutePagedQuery = procProcessRoutePagedQueryDto.ToQuery<ProcProcessRoutePagedQuery>();
            procProcessRoutePagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _procProcessRouteRepository.GetPagedInfoAsync(procProcessRoutePagedQuery);

            // 实体到DTO转换 装载数据
            var procProcessRouteDtos = pagedInfo.Data.Select(s => s.ToModel<ProcProcessRouteDto>());
            return new PagedInfo<ProcProcessRouteDto>(procProcessRouteDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CustomProcessRouteDto> GetCustomProcProcessRouteAsync(long id)
        {
            CustomProcessRouteDto model = new CustomProcessRouteDto { };
            var processRoute = await _procProcessRouteRepository.GetByIdAsync(id);
            if (processRoute == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10439));
            }
            model.Info = processRoute.ToModel<ProcProcessRouteDto>();

            var nodeQuery = new ProcProcessRouteDetailNodeQuery { ProcessRouteId = id };
            var nodes = await _procProcessRouteNodeRepository.GetListAsync(nodeQuery);
            var detailNodeViewDtos = new List<ProcProcessRouteDetailNodeViewDto>();
            foreach (var node in nodes)
            {
                //实体转换
                var nodeViewDto = node.ToModel<ProcProcessRouteDetailNodeViewDto>();
                nodeViewDto.ProcessType = node.Type;
                nodeViewDto.Code = ConvertProcedureCode(nodeViewDto.ProcedureId, nodeViewDto.Code);
                nodeViewDto.Name = ConvertProcedureName(nodeViewDto.ProcedureId, nodeViewDto.Name);
                detailNodeViewDtos.Add(nodeViewDto);
            }
            model.Nodes = detailNodeViewDtos;

            var linkQuery = new ProcProcessRouteDetailLinkQuery { ProcessRouteId = id };
            var links = await _procProcessRouteLinkRepository.GetListAsync(linkQuery);
            var linkDtos = new List<ProcProcessRouteDetailLinkDto>();
            foreach (var link in links)
            {
                //实体转换
                var linkDto = link.ToModel<ProcProcessRouteDetailLinkDto>(); ;
                linkDtos.Add(linkDto);
            }
            model.Links = linkDtos;
            return model;
        }

        /// <summary>
        /// 根据ID查询工艺路线工序列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<ProcProcessRouteDetailNodeViewDto>> GetNodesByRouteId(long id)
        {
            var nodeQuery = new ProcProcessRouteDetailNodeQuery { ProcessRouteId = id };
            var nodes = await _procProcessRouteNodeRepository.GetListAsync(nodeQuery);
            var detailNodeViewDtos = new List<ProcProcessRouteDetailNodeViewDto>();
            foreach (var node in nodes)
            {
                //实体转换
                var nodeViewDto = node.ToModel<ProcProcessRouteDetailNodeViewDto>();
                nodeViewDto.ProcessType = node.Type;
                nodeViewDto.Code = ConvertProcedureCode(nodeViewDto.ProcedureId, nodeViewDto.Code);
                nodeViewDto.Name = ConvertProcedureName(nodeViewDto.ProcedureId, nodeViewDto.Name);
                if (!string.IsNullOrWhiteSpace(nodeViewDto.Code))
                {
                    detailNodeViewDtos.Add(nodeViewDto);
                }
            }

            return detailNodeViewDtos;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task AddProcProcessRouteAsync(ProcProcessRouteCreateDto parm)
        {
            #region 验证

            if (parm == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }

            //// 判断是否有获取到站点码
            //if (string.IsNullOrWhiteSpace(parm.SiteCode))
            //{
            //    throw new ValidationException(ErrorCode.MES10101);
            //}
            parm.Code = parm.Code.ToTrimSpace().ToUpperInvariant();
            parm.Name = parm.Name.Trim();
            parm.Remark = parm.Remark.Trim();
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(parm);

            //工艺路线编码和版本唯一
            var code = parm.Code;
            var query = new ProcProcessRouteQuery
            {
                Code = code,
                SiteId = _currentSite.SiteId ?? 0,
                Version = parm.Version
            };
            if (await _procProcessRouteRepository.IsExistsAsync(query))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10437)).WithData("Code", parm.Code).WithData("Version", parm.Version);
            }

            var siteId = _currentSite.SiteId ?? 0;
            //DTO转换实体
            var procProcessRouteEntity = parm.ToEntity<ProcProcessRouteEntity>();
            procProcessRouteEntity.Id = IdGenProvider.Instance.CreateId();
            procProcessRouteEntity.SiteId = siteId;
            procProcessRouteEntity.Code = code;
            procProcessRouteEntity.CreatedBy = _currentUser.UserName;
            procProcessRouteEntity.UpdatedBy = _currentUser.UserName;

            var nodes = ConvertProcessRouteNodeList(parm.DynamicData.Nodes, procProcessRouteEntity);
            var links = ConvertProcessRouteLinkList(parm.DynamicData.Links, procProcessRouteEntity);

            // 判断是否存在多个首工序
            var firstProcessCount = nodes.Where(w => w.IsFirstProcess == (int)YesOrNoEnum.Yes).Count();
            if (firstProcessCount == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10435));
            }

            if (firstProcessCount > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10436));
            }
            #endregion

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                //入库
                await _procProcessRouteRepository.InsertAsync(procProcessRouteEntity);

                if (nodes != null && nodes.Count > 0)
                {
                    await _procProcessRouteNodeRepository.InsertRangeAsync(nodes);
                }
                if (links != null && links.Count > 0)
                {
                    await _procProcessRouteLinkRepository.InsertRangeAsync(links);
                }

                ts.Complete();
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task UpdateProcProcessRouteAsync(ProcProcessRouteModifyDto parm)
        {
            #region 验证

            if (parm == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }

            //// 判断是否有获取到站点码
            //if (string.IsNullOrWhiteSpace(parm.SiteCode))
            //{
            //    throw new ValidationException(ErrorCode.MES10101);
            //}

            parm.Name = parm.Name.Trim();
            parm.Remark = parm.Remark.Trim();
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(parm);

            //判断是否存在
            var processRoute = await _procProcessRouteRepository.GetByIdAsync(parm.Id);
            if (processRoute == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10438));
            }

            //DTO转换实体
            var procProcessRouteEntity = parm.ToEntity<ProcProcessRouteEntity>();
            procProcessRouteEntity.UpdatedBy = _currentUser.UserName;

            var nodes = ConvertProcessRouteNodeList(parm.DynamicData.Nodes, procProcessRouteEntity);
            var links = ConvertProcessRouteLinkList(parm.DynamicData.Links, procProcessRouteEntity);

            // 判断是否存在多个首工序
            var firstProcessCount = nodes.Where(w => w.IsFirstProcess == (int)YesOrNoEnum.Yes).Count();
            if (firstProcessCount == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10435));
            }

            if (firstProcessCount > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10436));
            }

            // 工艺路线编码和版本唯一
            var query = new ProcProcessRouteQuery
            {
                Code = processRoute.Code,
                SiteId = _currentSite.SiteId ?? 0,
                Version = processRoute.Version,
                Id = processRoute.Id,
            };
            if (await _procProcessRouteRepository.IsExistsAsync(query))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10437)).WithData("Code", processRoute.Code).WithData("Version", processRoute.Version);
            }
            #endregion

            //TODO 现在关联表批量删除批量新增，后面再修改
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                //入库
                await _procProcessRouteRepository.UpdateAsync(procProcessRouteEntity);

                await _procProcessRouteNodeRepository.DeleteByProcessRouteIdAsync(procProcessRouteEntity.Id);
                if (nodes != null && nodes.Count > 0)
                {
                    await _procProcessRouteNodeRepository.InsertRangeAsync(nodes);
                }

                await _procProcessRouteLinkRepository.DeleteByProcessRouteIdAsync(procProcessRouteEntity.Id);
                if (links != null && links.Count > 0)
                {
                    await _procProcessRouteLinkRepository.InsertRangeAsync(links);
                }

                ts.Complete();
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeleteProcProcessRouteAsync(long[] idsArr)
        {
            if (idsArr.Length < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10102));
            }

            #region 参数校验
            //有生产中工单引用当前工艺路线，不能删除！
            // // 状态（0:新建;1:启用;2:保留;3:废除）
            var statusArr = new int[] { (int)SysDataStatusEnum.Enable, (int)SysDataStatusEnum.Retain };
            var query = new ProcProcessRouteQuery
            {
                Ids = idsArr,
                StatusArr = statusArr
            };
            var resourceList = await _procProcessRouteRepository.IsIsExistsEnabledAsync(query);
            if (resourceList != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10430));
            }
            #endregion

            var command = new DeleteCommand
            {
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now(),
                Ids = idsArr
            };
            return await _procProcessRouteRepository.DeleteRangeAsync(command);
        }

        /// <summary>
        /// 根据不合个工艺路线Id查询不合格工艺路线列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcessRouteDto>> GetListByIdsAsync(long[] ids)
        {
            var list = await _procProcessRouteRepository.GetByIdsAsync(ids);

            //实体到DTO转换 装载数据
            var processRouteDtos = new List<ProcProcessRouteDto>();
            foreach (var entity in list)
            {
                var processRouteDto = entity.ToModel<ProcProcessRouteDto>();
                processRouteDtos.Add(processRouteDto);
            }
            return processRouteDtos;
        }

        #region 业务扩展方法

        /// <summary>
        /// 转换节点工序（编码）
        /// </summary>
        /// <param name="procedureId"></param>
        /// <param name="defaultCode"></param>
        /// <returns></returns>
        public static string ConvertProcedureCode(long procedureId, string defaultCode)
        {
            return procedureId switch
            {
                StartNodeId => "",
                EndNodeId => "",
                _ => defaultCode,
            };
        }

        /// <summary>
        /// 转换节点工序（名称）
        /// </summary>
        /// <param name="procedureId"></param>
        /// <param name="defaultName"></param>
        /// <returns></returns>
        public static string ConvertProcedureName(long procedureId, string defaultName)
        {
            return procedureId switch
            {
                StartNodeId => "",    // 开始
                EndNodeId => "",  // 结束
                _ => defaultName,
            };
        }

        /// <summary>
        /// 转换集合（工艺路线-节点）
        /// </summary>
        /// <param name="nodeList"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<ProcProcessRouteDetailNodeEntity> ConvertProcessRouteNodeList(IEnumerable<FlowDynamicNodeDto> nodeList, ProcProcessRouteEntity model)
        {
            if (nodeList == null || nodeList.Any() == false)
            {
                return new List<ProcProcessRouteDetailNodeEntity> { };
            }

            return nodeList.Select(s => new ProcProcessRouteDetailNodeEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = model.SiteId,
                ProcessRouteId = model.Id,
                SerialNo = s.SerialNo,
                ProcedureId = s.ProcedureId,
                CheckType = s.CheckType,
                CheckRate = s.CheckRate,
                IsWorkReport = s.IsWorkReport,
                IsFirstProcess = s.IsFirstProcess,
                Extra1 = s.Extra1,
                CreatedBy = model?.UpdatedBy ?? "",
                UpdatedBy = model?.UpdatedBy ?? ""
            }).ToList();
        }

        /// <summary>
        /// 转换集合（工艺路线-连线）
        /// </summary>
        /// <param name="linkList"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<ProcProcessRouteDetailLinkEntity> ConvertProcessRouteLinkList(IEnumerable<FlowDynamicLinkDto> linkList, ProcProcessRouteEntity model)
        {
            if (linkList == null || linkList.Any() == false) return new List<ProcProcessRouteDetailLinkEntity> { };

            return linkList.Select(s => new ProcProcessRouteDetailLinkEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = model.SiteId,
                SerialNo = s.SerialNo,
                ProcessRouteId = model.Id,
                PreProcessRouteDetailId = s.PreProcessRouteDetailId,
                ProcessRouteDetailId = s.ProcessRouteDetailId,
                Extra1 = s.Extra1,
                CreatedBy = model?.UpdatedBy ?? "",
                UpdatedBy = model?.UpdatedBy ?? ""
            }).ToList();
        }
        #endregion
    }
}
