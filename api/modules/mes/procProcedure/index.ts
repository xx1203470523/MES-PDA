import type { PageInputType, PageOutputType } from './types.d'

import request from '@/api/libs/modules/mes-request'

/**
 * 分页获取工序
 */
export function pageAsync(params : PageInputType) : Promise<PageOutputType> {
	return request.get({ url: `/ProcProcedure/list`, params, version: 'v1' })
}