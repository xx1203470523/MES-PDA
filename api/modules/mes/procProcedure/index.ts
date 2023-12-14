import type { ProcProcedureOutputType, ProcProcedureSelectType } from './types.d'

import request from '@/api/libs/modules/mes-request'

/**
 * 分页获取工序
 */
export function listAsync() : Promise<Array<ProcProcedureOutputType>> {
	return request.get({
		url: `/ProcProcedure/pda/list`, version: 'v1'
	})
}


/**
 * 分页获取工序
 */
export function getAllAsync() : Promise<Array<ProcProcedureSelectType>> {
	return request.get({
		url: `/ProcProcedure/pda/procedure/list`, version: 'v1'
	})
}

/**
 * 根据编号获取工序信息
 * @param {Array<string>} params 
 * @return 
 */ 
export function getByCodesAsync(params) : Promise<Array<ProcProcedureSelectType>> {
	return request.get({
		url: `/ProcProcedure/pda/procedure/get`, version: 'v1', params
	})
}