import request from '@/api/libs/modules/wms-request'
import type { MaterialScanType, MaterialCheckType } from './delivery-order-types'

/**
 * 获取出库验证列表
 */
export const pageAsync = (data : any) : Promise<any> => {
	return request.get({ version: `v1`, url: `/DeliveryOrder/pda/page`, data })
}
/**
 * 获取出库验证列表汇总数据
 */
export const getPdaGroupApi = (data : any) : Promise<any> => {
	return request.get({ version: `v1`, url: `/DeliveryOrder/pda/getPdaGroup`, data })
}

 /**
  * 获取推荐扫码物料
  */
 export function getScanMaterialAsync(id : string) {
 	return request.get({ url: `/DeliveryOrder/pda/scan/material/${id}`, version: 'v1' })
 }
 
 /**
  * 获取扫码物料信息
  */
 export function getScanMaterialInfoAsync(params : any) {
 	return request.get({ url: `/DeliveryOrder/pda/scan/getScanInfo`, version: 'v1',params })
 }
 
 
/**
 * 获取出库验证,单条数据查询
 */
export const getDetailApi = (data : any) : Promise<any> => {
	return request.get({ version: `v1`, url: `/DeliveryOrder/pda/detail/page`, data })
} 
 
 /**
  * 获取出库验证,单条数据查询
  */
 export const getOneAsync = (params : any) : Promise<any> => {
 	return request.get({ version: `v1`, url: `/DeliveryOrder/pda/scanPage`, params })
 }
 
 /**
  * 获取单条明细
  */
 export function getDetailAsync(id : string) {
 	return request.get({ url: `/DeliveryOrder/${id}`, version: 'v1' })
 }
 /**
  * 出库验证数据查询汇总
  */
 export function detailSumAsync(id : string) {
 	return request.get({ version: `v1`, url: `/DeliveryOrder/pda/detail/sum/${id}` })
 } 
 
 /**
  * 出库验证数据明细汇总
  */
 export function getSumAsync(params : any) {
 	return request.get({ version: `v1`, url: `/DeliveryOrder/pda/detail/sum`, params })
 }
 
 /**
  * 物料扫描接口
  */
 export const materialScanAsync = (data : any) : Promise<any> => {
 	return request.post({ version: `v1`, url: `/DeliveryOrder/pda/scan`, data })
 }
 
 /**
  * 出库确认完成功能
  */
 export function confirmAsync(data : any) {
 	return request.put({ version: `v1`, url: `/DeliveryOrder/update/Affirm`, data })
 }
  