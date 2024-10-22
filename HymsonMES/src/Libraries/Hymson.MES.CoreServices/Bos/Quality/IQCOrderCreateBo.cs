﻿using Hymson.MES.Core.Domain.WHMaterialReceipt;
using Hymson.MES.Core.Domain.WHMaterialReceiptDetail;
using Hymson.MES.CoreServices.Bos.Common;

namespace Hymson.MES.CoreServices.Bos.Quality
{
    public class IQCOrderCreateBo : CoreBaseBo
    {
        /// <summary>
        /// 收货单
        /// </summary>
        public WhMaterialReceiptEntity MaterialReceiptEntity { get; set; }

        /// <summary>
        /// 收货单明细列表
        /// </summary>
        public IEnumerable<WHMaterialReceiptDetailEntity> MaterialReceiptDetailEntities { get; set; }
    }
}
