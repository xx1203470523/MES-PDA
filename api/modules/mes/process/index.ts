import type { queryProcessOutputType, updateSFCprocessType } from './types.d'

import request from '@/api/libs/modules/mes-request'

/**
 * 获取条码在制信息
 */
export function getSfcProcessInfoAsync(params:any) : Promise<queryProcessOutputType> {
	return request.get({
		url: `/manuSfcProduce/pda/getProcessInfo`, version: 'v1', params
	})
}


/**
 * 获取条码在制信息
 */
export function updateProduceStatusAsync(data:any) : Promise<any> {
	return request.post({	
		url: `/manuSfcProduce/pda/produce/updatestatus`, data, version: 'v1'
	})
}
