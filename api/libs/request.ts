import type { RequestConfig } from './request-types'
import type { AjaxInstance, AjaxConfigType, AjaxRequestConfig } from '@/uni_modules/u-ajax/js_sdk/index.js'

import ajax from '@/uni_modules/u-ajax/js_sdk/index.js'

class Request {
	axios : AjaxInstance<AjaxConfigType>

	/**
	 * 构造函数
	 */
	constructor(config : RequestConfig) {
		this.axios = ajax.create({
			timeout: 10000,
			...config.ajaxConfig
		})

		this.axios.interceptors.request.use(config.requestInterceptorsSuccess, config.requestInterceptorsReject)
		this.axios.interceptors.response.use(config.responseInterceptorsSuccess, config.responseInterceptorsReject)
	}

	/**
	 * get方法
	 */
	async get(config : AjaxRequestConfig) {
		return await this.axios.get(config) as any
	}

	/**
	 * post方法
	 */
	async post(config : AjaxRequestConfig) {
		return await this.axios.post(config) as any
	}

	/**
	 * put方法
	 */
	async put(config : AjaxRequestConfig) {
		return await this.axios.put(config) as any
	}

	/**
	 * delete方法
	 */
	async delete(config : AjaxRequestConfig) {
		return await this.axios.delete(config) as any
	}
}

export default Request