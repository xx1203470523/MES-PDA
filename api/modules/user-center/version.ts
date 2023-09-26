import type { versionInputType, versionOutputType } from './version-types'

import request from '@/api/libs/modules/user-center-request'

/**
 * 版本号检查
 */
export function versionCheckAsync(params : versionInputType) : Promise<versionOutputType> {
	return request.get({
		url: '/system/version/check',
		params: {
			versionCode: params.versionCode,
			serviceType: 'mes'
		}
	})
}

/**
 * 版本信息
 */
export function versionGetAsync(params : versionInputType) : Promise<versionOutputType> {
	return request.get({
		url: '/system/version/code',
		params: {
			versionCode: params.versionCode,
			serviceType: 'mes'
		}
	})
}