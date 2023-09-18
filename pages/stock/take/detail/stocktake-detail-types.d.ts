export type TakeDetailData = {
	/**
	 * id
	 */
	id : string,

	/**
	 * 物料编码
	 */
	materialCode : string,

	/**
	 * 物料名称
	 */
	materialName : string,

	/**
	 * 物料单位
	 */
	materialUnit : string

	/**
	 * 账面总数
	 */
	bookQuantity : number | 0,

	/**
	 * 采集总数（实盘数）
	 */
	takeQuantity : number | 0,

	/**
	 * 账面总数
	 */
	bookTotal : number | 0,

	/**
	 * 采集总数（实盘数）
	 */
	takeTotal : number | 0,

	/**
	 * 库位
	 */
	binCode : string
}