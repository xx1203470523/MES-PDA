import type { loginResultType } from './demo-types.d'

import request from '@/api/libs/modules/mes-request'

/**
 * 登录接口
 */
export function loginAsync(data : any) : Promise<loginResultType> {
	return request.post({ url: '/login', data })
}