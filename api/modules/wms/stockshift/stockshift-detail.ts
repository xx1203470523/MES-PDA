import request from '@/api/libs/modules/wms-request'

/**
 * 获取移位单明细
 */
export const getDetailListApiAsync = (params : any) : Promise<any> => {
	return request.get({ version: `v1`, url: `/StockShift/pda/detail`,params})
}	

/**
 * 校验移出/移入库位
 */
export const getBinByQueryAsync = (params : any) : Promise<any> => {
	return request.get({ version: `v1`, url: `/BaseLocation/one`,params})
}

/**
 * 创建移位明细
 */
export const createDetailsAsync = (data : any) : Promise<any> => {
	return request.post({ version: `v1`, url: `/StockShift/pda/createdetails`,data})
}

/**
 * 校验移出/移入库位/物料
 */
export const getBinAndMaterialByQueryAsync = (data : any) : Promise<any> => {
	return request.post({ version: `v1`, url: `/StockShift/checkwarehousebin`,data})
}

/**
 * 确认单据
 */
export const confirmShiftAsync = (data : any) : Promise<any> => {
	return request.put({ version: `v1`, url: `/StockShift/confirmshift`,data})
}


/**
 * 获取移位单明细
 */
export const getTotalCountApiAsync = (params : any) : Promise<any> => {
	return request.get({ version: `v1`, url: `/StockShift/pda/detail/totalcount`,params})
}	


/**
 * 获取扫描数
 */
export const getScanQualtityApiAsync = (params : any) : Promise<any> => {
	return request.get({ version: `v1`, url: `/StockShift/pda/getsanqunaltity`,params})
}