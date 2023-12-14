import request from '@/api/libs/modules/mes-request'

import type { bindSfcType } from './types'


export function getBindSfcAsync(sfc : string) : Promise<Array<bindSfcType>> {
	return request.get({ url: `/ManuSFCBind/pda/bindsfc/get/` + sfc, version: 'v1' })
}

export function delBindSfcAsync(data : any) {
	return request.post({ url: `/ManuSFCBind/pda/bindsfc/del`, version: 'v1', data })
}

export function bindSfcAsync(data : any) {
	return request.post({ url: `/ManuSFCBind/pda/bindsfc`, version: 'v1', data })
}