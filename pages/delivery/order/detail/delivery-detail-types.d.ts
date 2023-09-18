export type DeliveryDetailData = {
	/**
	 * 明细id
	 */
	id : string,

	/**
	 * 出库通知数量
	 */
	noticeQuantity : number,

	/**
	 * 实收数量
	 */
	deliveryQuantity : number, 

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