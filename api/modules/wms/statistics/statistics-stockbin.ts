import request from '@/api/libs/modules/wms-request'

/**
 * 物料库存查询接口
 */
export const getListAsync = (data : any) : Promise<any> => {
	return request.get({ version: `v1`, url: `/StockBin/pda/page`, data })
}
/**
 * 获取物料库存汇总数据
 */
export const getPdaGroupAsync = (data : any) : Promise<any> => {
	return request.get({ version: `v1`, url: `/StockBin/pda/getPdaGroup`, data })
}

/**
 * 物料移动库位
 */
export const moveStockBindAsync = (data : any) : Promise<any> => {
	return request.post({ version: `v1`, url: `/StockBin/pda/move`, data })
}