export type WithdrawPageData = {
	/**
	 * id
	 */
	id: string,
	
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
	materialUnit : string
	
	/**
	 * 库位编码
	 */
	warehouseBinCode : string
	
	/**
	 * 下架建议单数量（物料+库位）
	 */
	withdrawSuggestionBinQuantity : number
	
	/**
	 * 下架单号
	 */
	withdrawOrderCode : string
	
	/**
	 * 下架建议单号
	 */
	withdrawSuggestionCode : string
}