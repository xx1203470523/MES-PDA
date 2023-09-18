export type TakeData = {
	/**
	 * id
	 */
	id : string,

	/**
	 * 采集单号
	 */
	takeCode : string,
	
	/**
	 * 盘点类型
	 */
	checkType : string,
	
	/**
	 * 盘点方式
	 */
	checkMethod : string,

	/**
	 * 账面总数
	 */
	bookTotal : number|'',

	/**
	 * 采集总数（实盘数）
	 */
	takeTotal : number|'',
	
	/**
	 * 盘点状态
	 */
	countingStatus : string,

	/**
	 * 备注
	 */
	remark : string
}