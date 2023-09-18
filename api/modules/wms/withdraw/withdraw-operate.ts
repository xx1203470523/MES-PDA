import request from '@/api/libs/modules/wms-request'

/**
 * 获取下架单,单条数据查询
 */
export const getOneAsync = (params : any) : Promise<any> => {
	return request.get({ version: `v1`, url: `/WithdrawOrder/pda/scanPage`, params })
}
/**
 * 获取单条明细
 */
export function getDetailAsync(id : string) {
	return request.get({ url: `/WithdrawOrder/${id}`, version: 'v1' })
}
/**
 * 下架单数据查询汇总
 */
export function getPdaGroupAsync(id : string) {
	return request.get({ version: `v1`, url: `/WithdrawOrder/pda/detail/sum/${id}` })
}

/**
 * 下架单数据明细汇总
 */
export function getSumAsync(params : any) {
	return request.get({ version: `v1`, url: `/WithdrawOrder/pda/detail/sum`, params })
}

/**
 * 物料扫描接口
 */
export const pdaMaterialScanAsync = (data : any) : Promise<any> => {
	return request.post({ version: `v1`, url: `/WithdrawOrder/pda/scan/material`, data })
}

/**
 * 下架确认完成功能
 */
export const withdrawConfirmAsync = (data : any) : Promise<any> => {
	return request.post({ version: `v1`, url: `/WithdrawOrder/pda/confirm`, data })
}

/**
 * 跳过（将当前建议明细顺序调整至最后，优先提醒下一个建议物料库位信息）
 */
export function skipAsync(data : any) {
	return request.post({ url: `/WithdrawSuggestion/pda/skip`, data, version: 'v1' })
}