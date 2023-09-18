export type ReceiptDetailType = {
	/**
	 * id
	 */
	id : string

	/**
	 * 入库单号
	 */
	receiptOrderCode : string

	/**
	 * 建议数量
	 */
	suggestionQuantity ?: number

	/**
	 * 物料信息
	 */
	material : ReceiptDetailMaterialType

	/**
	 * 建议库位
	 */
	suggestionWarehouseBin : ReceiptDetailMaterialSuggestionType
}

export type ReceiptDetailMaterialType = {
	materialCode : string
	materialName : string
	materialUnit : string
}

export type ReceiptDetailMaterialSuggestionType = {
	binCode : string
}