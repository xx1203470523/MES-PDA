import type { versionInputType, versionOutputType } from './version-types'

import request from '@/api/libs/modules/user-center-request'

/**
 * 版本号检查
 */
export function versionCheck(params : versionInputType) : Promise<versionOutputType> {
	return request.get({
		url: '/system/version/check',
		params
	})
}

/**
 * 版本信息
 */
export function versionGet(params : versionInputType) : Promise<any> {
	return request.get({
		url: '/system/version/code',
		params
	})
}