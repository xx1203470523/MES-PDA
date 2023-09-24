import type { AjaxRequestConfig, AjaxResponse } from '@/uni_modules/u-ajax/js_sdk/index.js'

import { getCache } from '@/utils/cache-utils'
import { useUserStore } from '@/store/user'

/**
 * 请求成功
 */
export function requestSuccess(config : AjaxRequestConfig) {
	const userStore = useUserStore()

	if (config.version) {
		config.url = `/${config.version}${config.url}`
	}

	let token = userStore.getToken
	if (!token) {
		const tokenCache = getCache('token')
		if (tokenCache && tokenCache.data) {
			token = tokenCache.data
		}
	}
	
	if (token) {
		config.header = {
			...config.header,
			Authorization: `Bearer ${token}`
		}
	}

	return config;
}

/**
 * 请求失败
 */
export function requestFail(error : any) {
	return Promise.reject(error);
}

/**
 * 响应成功
 */
export function responseSuccess(response : AjaxResponse<any>) {
	return response?.data;
}

/**
 * 响应失败
 */
export function responseFail(error : any) {
	if (process.env.NODE_ENV === 'development') {
		console.log(error)
	}

	const { data, statusCode } = error

	switch (statusCode) {
		case 404:
			uni.showToast({
				icon: 'none',
				title: '请求方法找不到'
			})
			break

		case 405:
			uni.showToast({
				icon: 'none',
				title: '请求方法不存在'
			})
			break

		case 401:
			const pages = getCurrentPages()
			if (pages.length > 0) {
				const currentPage = pages[pages.length - 1]
				if (currentPage.route != '/pages/login/login') {
					uni.showModal({
						title: '登录超时',
						content: '登录已超时，请跳转至登录页登录',
						confirmText: '前往登录',
						success: ({ confirm }) => {
							if (confirm) {
								uni.navigateTo({
									url: `/pages/login/index?redirect=${encodeURIComponent(currentPage.route)}`
								})
							}
						}
					})
				}
			}
			break

		default:
			if (data?.errors) {
				let errorKeys = Object.keys(data.errors)

				if (errorKeys.length > 1) {
					const errStr = errorKeys.map(m => {
						return data.errors[m]
					}).join('\n')

					uni.showModal({
						title: '多个错误项',
						content: errStr,
						showCancel: false
					})
				} else if (errorKeys.length == 1) {
					uni.showToast({
						icon: 'none',
						title: data.errors[errorKeys[0]][0],
						duration: 2000
					});
				}
			}
			break
	}

	return Promise.reject(data)
}