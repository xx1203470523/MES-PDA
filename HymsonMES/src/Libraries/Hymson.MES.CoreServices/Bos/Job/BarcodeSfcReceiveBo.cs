﻿using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Bos.Job
{
    public class BarcodeSfcReceiveBo : JobBaseBo
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = "";
        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }
        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        public long EquipmentId { get; set; }
    }

    public class BomMaterial
    {
        /// <summary>
        /// 物料
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 数据收集方式 
        /// </summary>
        public MaterialSerialNumberEnum? DataCollectionWay { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Qty { get; set; }
    }

    public class BarcodeSfcReceiveResponseBo
    {
        /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQuantity { get; set; }

        /// <summary>
        /// 下达数量
        /// </summary>
        public decimal PassDownQuantity { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; } = "";

        /// <summary>
        /// 是否产品一致
        /// </summary>
        public bool IsProductSame { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public ManuSfcInfoUpdateIsUsedBo ManuSfcInfoUpdateIsUsed { get; set; } = new ManuSfcInfoUpdateIsUsedBo();

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcEntity> ManuSfcList { get; set; } = new List<ManuSfcEntity>();

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcEntity> UpdateManuSfcList { get; set; } = new List<ManuSfcEntity>();

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcInfoEntity> ManuSfcInfoList { get; set; } = new List<ManuSfcInfoEntity>();

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcProduceEntity> ManuSfcProduceList { get; set; } = new List<ManuSfcProduceEntity>();

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ManuSfcStepEntity> ManuSfcStepList { get; set; } = new List<ManuSfcStepEntity>();

        public IEnumerable<UpdateWhMaterialInventoryEmptyByIdCommand>  updateWhMaterialInventoryEmptyByIdCommands{ get; set; } =new  List<UpdateWhMaterialInventoryEmptyByIdCommand>();
}

    /// <summary>
    /// 更新
    /// </summary>
    public class ManuSfcInfoUpdateIsUsedBo
    {
        /// <summary>
        /// 操作人员
        /// </summary>
        public string UserId { get; set; } = "";

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 产品条码ID列表 
        /// </summary>
        public IList<long> SfcIds { get; set; } = new List<long>();
    }

}
