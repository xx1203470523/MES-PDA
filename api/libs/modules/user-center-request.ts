import { appConfig } from '@/config'
import { requestSuccess, requestFail, responseSuccess, responseFail } from '../interceptors/hmx-interceptors'

import Request from '../request'

const request = new Request({
	ajaxConfig: {
		baseURL: appConfig.userCenterRemoteUrl
	},
	requestInterceptorsSuccess: requestSuccess,
	requestInterceptorsReject: requestFail,
	responseInterceptorsSuccess: responseSuccess,
	responseInterceptorsReject: responseFail
})

export default request