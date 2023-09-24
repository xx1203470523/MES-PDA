import request from '@/api/libs/modules/mes-request'

/**
 * 根据工单模糊查询
 */
export function sfcboxFuzzyPageAsync(workOrderCode : string) {
	return request.get({ url: `/PlanWorkOrder/pda/fuzzy/${workOrderCode}`, version: 'v1' })
}