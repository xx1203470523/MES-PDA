export type versionInputType = {
	/**
	 * 版本号
	 */
	versionCode : number
}

export type versionOutputType = {
	/**
	 * 版本名称
	 */
	versionName ?: string

	/**
	 * 版本编号
	 */
	versionCode ?: number

	/**
	 * 下载地址
	 */
	url ?: string

	/**
	 * 更新说明
	 */
	content ?: string

	/**
	 * 是否启用
	 */
	isEnabled ?: boolean

	/**
	 * 是否强制更新
	 */
	isForceUpdate ?: boolean

	/**
	 * 创建时间
	 */
	createdOn ?: Date
}