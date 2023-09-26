import type { ProcProcedureOutputType } from './types.d'

import request from '@/api/libs/modules/mes-request'

/**
 * 分页获取工序
 */
export function listAsync() : Promise<Array<ProcProcedureOutputType>> {
	return request.get({
		url: `/ProcProcedure/pda/list`, version: 'v1'
	})
}