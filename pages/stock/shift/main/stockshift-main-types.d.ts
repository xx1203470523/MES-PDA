export type StockShiftDetail={
	/**
	 * 
	 */
	id?:string
	
	/**
	 * 直接移位单号
	 */
	shiftCode?:string
	
	/**
	 * 数量
	 */
	shiftTotal?:number
	
	/**
	 * 仓库
	 */
	wareHouseName?:string
	
	/**
	 * 单据日期
	 */
	createOn?:string
	
	/**
	 * 制单人
	 */
	createBy?:string
	
	/**
	 * 备注
	 */
	remark?:string
	
	/**
	 * 移位类型
	 */
	shiftType?:string
}