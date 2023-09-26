/**
 * 路由工具
 */

import type { BackType } from './route-utils-types'

/**
 * 路由跳转（兼容tabbar页）
 */
export function to(url : string | HBuilderX.PageURIString) {
	uni.navigateTo({
		url: url,
		fail: ({ errMsg }) => {
			if (errMsg && errMsg.indexOf('tabbar page') != -1) {
				uni.switchTab({
					url: url,
					fail: () => {
						uni.showToast({
							icon: 'none',
							title: '页面跳转异常'
						})
					}
				})
			} else if (errMsg) {
				uni.showToast({
					icon: 'none',
					title: errMsg
				})
			} else {
				uni.showToast({
					icon: 'none',
					title: '页面跳转异常'
				})
			}
		}
	})
}

/**
 * 路由跳转（兼容tabbar页）
 */
export function redirectTo(url : string | HBuilderX.PageURIString) {
	uni.redirectTo({
		url: url,
		fail: ({ errMsg }) => {
			if (errMsg && errMsg.indexOf('tabbar page') != -1) {
				uni.switchTab({
					url: url,
					fail: () => {
						uni.showToast({
							icon: 'none',
							title: '页面跳转异常'
						})
					}
				})
			} else if (errMsg) {
				uni.showToast({
					icon: 'none',
					title: errMsg
				})
			} else {
				uni.showToast({
					icon: 'none',
					title: '页面跳转异常'
				})
			}
		}
	})
}

/**
 * 关闭所有页面，打开新的页面
 */
export function reLaunch(url : string | HBuilderX.PageURIString) {
	uni.reLaunch({
		url: url,
		fail: ({ errMsg }) => {
			if (errMsg && errMsg.indexOf('tabbar page') != -1) {
				uni.switchTab({
					url: url,
					fail: () => {
						uni.showToast({
							icon: 'none',
							title: '页面跳转异常'
						})
					}
				})
			} else if (errMsg) {
				uni.showToast({
					icon: 'none',
					title: errMsg
				})
			} else {
				uni.showToast({
					icon: 'none',
					title: '页面跳转异常'
				})
			}
		}
	})
}

/**
 * 返回首页
 */
export function toMain(){
	uni.switchTab({
		url: '/pages/tabbar/user/user',
		fail: () => {
			uni.showToast({
				icon: 'none',
				title: '页面跳转异常'
			})
		}
	})
}

/**
 * 返回上一页
 */
export function back(opt ?: BackType) {
	const pages = getCurrentPages();
	if (opt) {
		if (pages.length >= 2) {
			uni.navigateBack({
				delta: opt.delta || 1
			})
		} else if (pages.length === 1) {
			redirectTo(opt.redirectUrl)
		}
	} else {
		if (pages.length === 1) {
			toMain()
		} else {
			uni.navigateBack()
		}
	}
}