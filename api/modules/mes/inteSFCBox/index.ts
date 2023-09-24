import type { SfcBoxDataType } from './types.d'

import request from '@/api/libs/modules/mes-request'

/**
 * 箱码校验
 */
export function sfcboxValidateScanAsync(data : SfcBoxDataType) {
	return request.post({ url: `/InteSFCBox/pda/sfcboxvalidate`, data, version: 'v1' })
}