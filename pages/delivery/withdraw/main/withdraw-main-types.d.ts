export type WithdrawData = {
	/**
	 * id
	 */
	id : string,

	/**
	 * 同步单号
	 */
	syncCode : string,

	/**
	 * 下架单号
	 */
	withdrawOrderCode : string,

	/**
	 * 下架建议单号
	 */
	withdrawSuggestionCode : string,

	/**
	 * 出库通知单号
	 */
	deliveryNoticeCode : string,

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
	contactName : string,
	/**
	 * 单据日期（派单日期）
	 */
	syncDispatchDate : string,

	/**
	 * 下架建议数
	 */
	withdrawSuggestionTotal : number

	/**
	 * 备注
	 */
	remark : string
}