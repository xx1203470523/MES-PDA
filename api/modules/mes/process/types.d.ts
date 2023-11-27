/**
 * 查询SFC在在制信息，以及工艺路线的工序
 */
export type queryProcessOutputType = {
	processName: string,
	procedureName: string
}

/**
 * 更新在制品状态
 */
export type updateSFCprocessType = {
	/**
	 * 条码
	 */
	sfc : string

	/**
	 * 在制工序
	 */
	procedureId : number

	/**
	 * 在制品状态
	 */
	processStatus : number
}