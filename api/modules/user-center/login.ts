import type { loginResultType } from './login-types.d'

import request from '@/api/libs/modules/user-center-request'

/**
 * 登录接口
 */
export function loginAsync(data : any) : Promise<loginResultType> {
	return request.post({ url: '/login', data })
}