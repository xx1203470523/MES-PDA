/**
 * 工序
 */
export type ProcProcedureOutputType = {
	/**
	 * id
	 */
	id: string
	
	/**
	 * 工序名
	 */
	name: string
	
	/**
	 * 资源类型
	 */
	resType : string

	/**
	 * 资源类型名称
	 */
	resTypeName : string
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

/**
 * 分页结果
 */
export type PageOutputType = {
	data : Array<ProcProcedureOutputType>
}