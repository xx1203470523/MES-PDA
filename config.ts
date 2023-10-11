const updateConfig = {
	remoteUrl: process.env.NODE_ENV === 'development' ? '' : ''
}

const appConfig = {
	// wms远程地址
	// wmsRemoteUrl: process.env.NODE_ENV === 'development' ? 'http://localhost:11701/api' : 'http://10.10.79.12:9013/wms/api',

	// mes远程地址
	
	// 正式环境
	// mesRemoteUrl: process.env.NODE_ENV === 'development' ? 'http://10.10.79.12:9018/api' : 'http://59.40.181.33:8580/api',
	
	// 测试环境
	mesRemoteUrl: process.env.NODE_ENV === 'development' ? 'http://localhost:11701/api' : 'http://10.10.79.12:9018/api',

	// 用户中心远程地址
	
	// 测试环境
	// userCenterRemoteUrl: process.env.NODE_ENV === 'development' ? 'http://10.10.79.12:9015/prod-api' : 'http://10.10.79.12:9015/prod-api'
	
	// 正式环境
	userCenterRemoteUrl: process.env.NODE_ENV === 'development' ? 'http://10.10.79.12:8015/prod-api' : 'http://59.40.181.33:8581/prod-api'

	// 用户中心

	// wms远程地址
	// 本地环境
	// wmsRemoteUrl: '/wmsService/api',
	// wmsRemoteUrl: 'http://10.9.2.14:5235/api',
	// wmsRemoteUrl: 'http://192.168.61.210:5235/api',

	// 测试环境
	// wmsRemoteUrl: 'http://10.10.79.12:9013/wms/api',

	// 正式环境
	// wmsRemoteUrl: '/wms/api',

	// 用户中心远程地址
	// 本地环境
	// userCenterRemoteUrl: '/userService'

	// 测试环境
	// userCenterRemoteUrl: 'http://10.10.79.12:9015/prod-api'
}

export { updateConfig, appConfig }