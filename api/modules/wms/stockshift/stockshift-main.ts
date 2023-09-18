import request from '@/api/libs/modules/wms-request'

/**
 * 获取直接移位单列表
 */
export const getListApiAsync = (data : any) : Promise<any> => {
	return request.get({ version: `v1`, url: `/StockShift/page`, data })
}

/**
 * 获取待移动单据数和待移动物料总数
 */
export const getTotalQuanApiAsync = (data : any) : Promise<any> => {
	return request.get({ version: `v1`, url: `/StockShift/pda/getTotalQuantity`, data })
}