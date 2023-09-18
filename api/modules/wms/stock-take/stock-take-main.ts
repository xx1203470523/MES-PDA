import request from '@/api/libs/modules/wms-request'
import type { MaterialScanType } from './stock-take-types.d'

/**
 * 分页查询盘点采集单
 */
export function pageAsync(params : any) : Promise<any> {
	return request.get({ url: '/StockTake/page/pda', version: 'v1', params })
}

/**
 * 获取盘点采集单,单条数据查询
 */
export const getOneApi = ({ id }) : Promise<any> => {
	return request.get({ version: `v1`, url: `/StockTake/detail/${id}` })
}

/**
 * 获取盘点采集单明细
 */
export function getDetailList(id : string) : Promise<any> {
	return request.get({ version: `v1`, url: `/StockTake/detail/${id}` })
}

/**
 * 获取盘点采集单,单条数据查询
 */
export const getDetailApi = (params : any) : Promise<any> => {
	return request.get({ version: `v1`, url: `/StockTake/pda/detail/`, params })
}

/**
 * 获取盘点采集单,单条数据查询汇总
 */
export function getById(id : string) : Promise<any> {
	return request.get({ version: `v1`, url: `/StockTake/pda/${id}` })
}

/**
 * 获取盘点采集单,单条数据查询汇总
 */
export function getSum(id : string) : Promise<any> {
	return request.get({ version: `v1`, url: `/StockTake/pda/detail/sum/${id}` })
}

/**
 * 物料扫描
 */
export function materialScanAsync(data : MaterialScanType) {
	return request.post({ url: `/StockTake/pda/scan`, version: 'v1', data })
}
/**
 * 物料扫描
 */
export function getmaterial(data : MaterialScanType) {
	return request.post({ url: `/StockTake/pda/scanMaterial`, version: 'v1', data })
}


/**
 * 确认上架
 */
export function confirmAsync(data : any) {
	return request.put({ url: '/StockTake/takeComplete', data, version: 'v1' })
}