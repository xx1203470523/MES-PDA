export type ReceiptData = {
	/**
	 * id
	 */
	id: string,
	
	/**
	 * 同步单号
	 */
	receiptNoticeSyncCode : string,
	
	/**
	 * 通知单号
	 */
	receiptNoticeCode : string,
	
	/**
	 * 通知数量
	 */
	receiptNoticeQuantity : number,
	
	/**
	 * 入库单号
	 */
	receiptOrderCode : string,
	
	/**
	 * 仓库名称
	 */
	name : string,
	
	/**
	 * 备注
	 */
	remark : string
}