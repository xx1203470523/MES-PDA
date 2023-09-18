/**
 * 上架收货
 */

import request from '@/api/libs/modules/wms-request'

import type { WarehouseBinScanType } from './basic-warehouseBin-types'

/**
 * 库位扫描
 */
export function warehouseBinScanAsync(params : WarehouseBinScanType) {
	return request.get({ url: `/BaseWarehouseBin/pda/scan`, version: 'v1', params })
}
