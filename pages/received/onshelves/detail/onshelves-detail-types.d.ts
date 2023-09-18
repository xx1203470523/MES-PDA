export type ReceiptDetailData = {
	/**
	 * 明细id
	 */
	id : string,
	
	/**
	 * 收货通知数量
	 */
	receiptNoticeQuantity : number,
	
	/**
	 * 实收数量
	 */
	receiptOrderQuantity : number,
	
	/**
	 * 待收数量
	 */
	receiptOrderRemainingQuantity : number,	
	
	/**
	 * 物料编码
	 */
	materialCode : string,
	
	/**
	 * 物料名称
	 */
	materialName : string,
	
	/**
	 * 单位
	 */
	unit : string
}