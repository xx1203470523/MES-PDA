import request from '@/api/libs/modules/wms-request'

/**
 * 获取默认仓库
 */
export const getTemporaryStorageBinAsync = (params: any) : Promise<any> => {
	return request.get({ version: `v1`, url: `/CommonConfig/temporary/storage/bin`, params})
}

/**
 * 获取交换库位
 */
export const getExchangeStorageBinAsync = (params: any) : Promise<any> => {
	return request.get({ version: `v1`, url: `/CommonConfig/exchange/storage/bin`, params})
}