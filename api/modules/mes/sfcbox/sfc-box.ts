import request from '@/api/libs/modules/mes-request'
import type { SfcBoxDataType } from './sfc-box-types.d'

/**
 * 箱码校验
 */
export function sfcboxValidateScanAsync(data : SfcBoxDataType) {
	return request.post({ url: `/InteSFCBox/pda/sfcboxvalidate`, data, version: 'v1' })
}

/**
 * 根据工单模糊查询
 */
export function sfcboxFuzzyPageAsync(workOrderCode : string) {
	return request.get({ url: `/PlanWorkOrder/pda/fuzzy/${workOrderCode}`, version: 'v1' })
}