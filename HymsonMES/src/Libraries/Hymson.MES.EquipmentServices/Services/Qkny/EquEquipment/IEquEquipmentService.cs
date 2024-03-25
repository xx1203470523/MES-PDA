﻿using Hymson.MES.Data.Repositories.Equipment.EquEquipment.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.Qkny.EquEquipment
{
    /// <summary>
    /// 设备接口
    /// </summary>
    public interface IEquEquipmentService
    {
        /// <summary>
        /// 获取设备资源对应的基础信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<EquEquipmentResAllView> GetEquResAllAsync(QknyBaseDto param);
    }
}
