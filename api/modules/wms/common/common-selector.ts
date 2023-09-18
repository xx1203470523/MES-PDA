import request from '@/api/libs/modules/wms-request'

/**
 * 仓库选择器
 */
export const getWarehouseListApiAsync = () : Promise<any> => {
	return request.get({ version: `v1`, url: `/CommonSelector/pda/warehouse` })
}

/**
 * 获取默认仓库
 */
export const getDefaultWarehouseAsync = () : Promise<any> => {
	return request.get({ version: `v1`, url: `/ConfigUser/getDefaultWarehouseId` })
}