import type { AjaxConfigType, AjaxRequestConfig, AjaxResponse } from '@/uni_modules/u-ajax/js_sdk/index.js'

export type RequestConfig = {
	ajaxConfig: AjaxConfigType
	requestInterceptorsSuccess: (config : AjaxRequestConfig) => AjaxRequestConfig
	requestInterceptorsReject: <T = never>(error : any) => Promise<T>
	responseInterceptorsSuccess: (response : AjaxResponse<any>) => AjaxResponse<any>
	responseInterceptorsReject: <T = never>(error : any) => Promise<T>
}