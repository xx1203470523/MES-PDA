export type TakeData = {
	/**
	 * id
	 */
	id : string,
	
	/**
	 * 盘点单号
	 */
	checkCode : string,
	
	/**
	 * 采集单号
	 */
	takeCode : string,
	
	/**
	 * 采集数
	 */
	quantity : number|'',
	
	/**
	 * 采集数
	 */
	bookQuantity : number|'',
	
	/**
	 * 账面总数(参考数量)
	 */
	takeTotal : number|'',
	
	/**
	 * 账面总数(参考数量)
	 */
	bookTotal : number|'',
	
	/**
	 * 物料编码
	 */
	materialCode: string,
	
	/**
	 * 物料名称
	 */
	materialName: string,
	
	/**
	 * 物料单位
	 */
	materialUnit: string
}