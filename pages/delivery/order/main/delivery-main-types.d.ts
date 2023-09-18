export type DeliveryData = {
	/**
	 * id
	 */
	id: string,
	
	/**
	 * 同步单号
	 */
	 syncCode : string,
	
	/**
	 * 通知单号
	 */
	deliveryNoticeCode : string,
	
	/**
	 * 通知数量
	 */
	deliveryNoticeAssignedTotal : number,
	
	/**
	 * 出库单号
	 */
	deliveryOrderCode : string,
	
	/**
	 * 仓库名称
	 */
	warehouseName : string,
	
	/**
	 * 往来单位
	 */
	contactName: string
	
	/**
	 * 单据日期
	 */
	createdOn : string
}