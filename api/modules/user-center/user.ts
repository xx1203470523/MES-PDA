import type { userInfoType, resetPasswordType, updateuserinfotype } from './user-types'

import request from '@/api/libs/modules/user-center-request'

/**
 * 获取用户信息
 */
export function getUserInfoAsync() : Promise<userInfoType> {
	return request.get({ url: '/getInfo' })
}

/**
 * 修改密码
 */
export function resetPassword(data : resetPasswordType) : Promise<any> {
	return request.put({
		url: '/system/user/profile/updatePwd',
		data: data
	})
}

/**	
 * 更新用户信息
 */
export function updateuserinfo(data : updateuserinfotype) : Promise<any> {
	return request.put({
		url: '/system/user/profile',
		data: data
	})
}

