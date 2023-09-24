import type {
	SwitchBindInputType, 
	BindSFCInputType, 
	UnBindSFCInputType, 
	RepeatInputType, 	
	BindSFCOutputType
} from './types.d'

import request from '@/api/libs/modules/mes-request'

/**
 * 列表
 */
export function listAsync(params : BindSFCInputType) : Promise<BindSFCOutputType> {
	return request.get({ url: `/ManuSFCBind/pda/pagelist`, params, version: 'v1' })
}

/**
 * 解绑
 */
export function unbindAsync(data : UnBindSFCInputType) {
	return request.post({ url: `/ManuSFCBind/pda/unBind`, data, version: 'v1' })
}

/**
 * 换绑
 */
export function switchBindAsync(data : SwitchBindInputType) {
	return request.post({ url: `/ManuSFCBind/pda/switchBind`, data, version: 'v1' })
}

/**
 * 复投
 */
export function repeatManuSFCAsync(data : RepeatInputType) {
	return request.post({ url: `/ManuSFCBind/pda/repeatManu`, data, version: 'v1' })
}