﻿using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command
{
    public class UpdateProcedureCommand
    {
        /// <summary>
        ///Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }
    }
}
