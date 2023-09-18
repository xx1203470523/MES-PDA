export type DeliveryDetailData = {
	/**
	 * id
	 */
	 id: string, 
	
	/**
	 * 物料信息
	 */
	material: DeliveryDetailMaterialType
}

export type DeliveryDetailMaterialType = {
	materialCode : string
	materialName : string
	materialUnit : string
}