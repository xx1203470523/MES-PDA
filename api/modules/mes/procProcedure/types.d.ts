/**
 * 工序
 */
export type ProcProcedureOutputType = {
	id: string
	code: string
	name: string
}

/**
 * 工序
 */
export type ProcProcedureSelectType = {
	text: string
	value: string
}

/**
 * 分页输入
 */
export type PageInputType = {
	/**
	 * 页码
	 */
	pageIndex : number

	/**
	 * 页面大小
	 */
	pageSize : number
}