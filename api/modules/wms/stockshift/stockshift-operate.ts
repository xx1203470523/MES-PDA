import request from '@/api/libs/modules/wms-request'

/**
 * 新增移位单
 */
export const addAsync = (data : any) : Promise<any> => {
	return request.post({ version: `v1`, url: `/StockShift/pda/create`, data })
}

/**
 * 根据Id获取移位单
 */
export const getOne = (params : any) : Promise<any> => {
	return request.get({ version: `v1`, url: `/StockShift/`+params})
}