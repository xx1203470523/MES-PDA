/**
 * 单据收货
 */

import request from '@/api/libs/modules/wms-request'

import type { MaterialScanType, MaterialCheckType } from './receive-order-types'

/**
 * 分页查询收货通知单列表
 */
export function pageAsync(params : any) : Promise<any> {
	return request.get({ url: '/ReceiptOrder/pda/page', version: 'v1', params })
}

/**
 * 分页查询单据收货收货明细
 */
export function pageDetailAsync(params : any) : Promise<any> {
	return request.get({ url: '/ReceiptOrder/pda/page/detail', version: 'v1', params })
}

/**
 * 分页查询PDA单据扫描列表
 */
export function pageScanAsync(params : any) : Promise<any> {
	return request.get({ url: '/ReceiptOrder/pda/page/scan', version: 'v1', params })
}

/**
 * 明细汇总
 */
export function detailSumAsync(id : string) : Promise<any> {
	return request.get({ url: `/ReceiptOrder/pda/detail/sum/${id}`, version: 'v1' })
}

/**
 * 获取明细
 */
export function getDetailAsync(id : string) {
	return request.get({ url: `/ReceiptOrder/${id}`, version: 'v1' })
}

/**
 * 物料检查
 */
export function materialCheckAsync(params: MaterialCheckType){
	return request.get({ url: '/ReceiptOrder/pda/scan/check', version: 'v1', params})
}

/**
 * 物料扫描
 */
export function materialScanAsync(data : MaterialScanType) {
	return request.post({ url: `/ReceiptOrder/pda/scan/material`, data, version: 'v1' })
}

/**
 * 确认收货
 */
export function confirmAsync(data : any) {
	return request.put({ url: '/ReceiptOrder/confirm', data, version: 'v1' })
}