/**
 * 缓存工具
 */

import type { CacheOptionType, CacheResultType, CacheKeyType } from './cache-utils-types'

/**
 * 设置缓存
 */
export function setCache(opt : CacheOptionType) {
	let { key, value, expired = { time: 0, unit: 'day' } } = opt

	const now = new Date()
	const nowTime = now.getTime()

	// 过期时间
	let expirationTime = 0

	if (key && value) {
		let storageInfo = uni.getStorageInfoSync()
		if (storageInfo.limitSize - storageInfo.currentSize < 5) {
			uni.clearStorageSync()
		}

		if (expired.time === 0) {
			// 缓存不会失效
			uni.setStorageSync(key, {
				data: value,
				expired: null,
				msg: null
			})
			return
		}

		if (expired.unit === 'day') {
			// 缓存失效时间以天为单位
			let seconds = 3600 * 24 * expired.time
			expirationTime = nowTime + seconds;
		} else if (expired.unit === 'hours') {
			// 缓存失效时间以小时为单位
			let seconds = 3600 * expired.time
			expirationTime = nowTime + seconds
		} else if (expired.unit === 'minutes') {
			// 缓存失效时间以分钟为单位
			let seconds = 60 * expired.time
			expirationTime = nowTime + seconds
		}

		uni.setStorageSync(key, {
			data: value,
			expired: expirationTime,
			msg: null
		})
	} else {
		console.error('存储缓存失败')
	}
}

/**
 * 获取缓存
 */
export function getCache(key : CacheKeyType) : CacheResultType {
	const now = new Date()
	const nowTime = now.getTime()

	if (key) {
		let data = uni.getStorageSync(key) as CacheResultType;
		if (data) {
			if (data.expired) {
				if (nowTime > data.expired) {
					return {
						data: null,
						expired: data.expired,
						msg: '缓存已过期'
					}
				} else {
					return data
				}
			} else {
				return data
			}
		}
	}

	return null
}

/**
 * 删除缓存
 */
export function delCache(key : CacheKeyType) {
	uni.removeStorageSync(key)
}