/**
 * 上架收货
 */

import request from '@/api/libs/modules/wms-request'

import type { MaterialScanType, MaterialCheckType } from './receive-onshelves-types.d'

/**
 * 分页查询收货上架单据
 */
export function pageAsync(params : any) {
	return request.get({ url: `/PutawayOrder/pda/page`, version: 'v1', params })
}

/**
 * 入库查看数据查询
 */
export function pageDetailApi(data : any) {
	return request.get({ version: `v1`, url: `/PutawayOrder/pda/page/detail`, data })
}

/**
 * 分页查询上架扫描单据
 */
export function pageScanAsync(params : any) {
	return request.get({ url: `/PutawayOrder/pda/page/scan`, version: 'v1', params })
}	

/**
 * 获取单条明细
 */
export function getDetailAsync(id : string) {
	return request.get({ url: `/Putawayorder/${id}`, version: 'v1' })
}

/**
 * 入库单数据查询汇总
 */
export function getPdaGroupApi(id : string) {
	return request.get({ version: `v1`, url: `/PutawayOrder/pda/detail/sum/${id}` })
}

/**
 * 入库单数据查询汇总
 */
export function getSum(params : any) {
	return request.get({ version: `v1`, url: `/PutawayOrder/pda/detail/sum`, params })
}

/**
 * 物料合法性检查
 */
export function getMaterialCheckAsync(params : MaterialCheckType) {
	return request.get({ url: `/PutawayOrder/pda/material/check`, version: 'v1', params })
}

/**
 * 物料扫描
 */
export function materialScanAsync(data : MaterialScanType) {
	return request.post({ url: `/PutawayOrder/pda/scan`, version: 'v1', data })
}

/**
 * 确认上架
 */
export function confirmAsync(data : any) {
	return request.put({ url: '/Putawayorder/confirm', data, version: 'v1' })
}

/**
 * 跳过（将当前建议明细顺序调整至最后，优先提醒下一个建议物料库位信息）
 */
export function skipAsync(data : any) {
	return request.post({ url: `/PutawaySuggestion/pda/skip`, data, version: 'v1' })
}