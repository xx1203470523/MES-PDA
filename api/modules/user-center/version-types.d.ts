export type versionInputType = {
	/**
	 * 版本号
	 */
	versionCode : number
	
	/**
	 * 服务类型
	 */
	serviceType : 'wms'
}

export type versionOutputType = {
	/**
	 * 版本名称
	 */
	versionName : string

	/**
	 * 下载地址
	 */
	url : string

	/**
	 * 更新说明
	 */
	content : string

	/**
	 * 是否有新版本
	 */
	hasNewVersion : boolean

	/**
	 * 是否强制更新
	 */
	isForceUpdate : boolean
}