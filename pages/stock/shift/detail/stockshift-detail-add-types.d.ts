export type StockShiftDetailData = {
	// /**
	//  * id
	//  */
	// id: string,
	
	/**
	 * 物料编码
	 */
	materialCode : string,
	
	/**
	 * 物料名称
	 */
	materialName : string,
	
	/**
	 * 移位类型
	 */
	shiftType:string
	
	/**
	 * 单位
	 */
	materialUnit : string
	
	/**
	 * 移位单号
	 */
	shiftCode:string
	
	/**
	 * 移出库位
	 */
	fromBinCode:string
	
	/**
	 * 移入库位
	 */
	toBinCode:string
}