import request from '@/api/libs/modules/wms-request'
import type { warehouse } from './config-user-types'


/**
 * 保存默认仓库
 */
export function saveWarehouseDataApi(data :warehouse) {
	return request.post({ url: `/ConfigUser/create`, version: 'v1', data })
}