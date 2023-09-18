import request from '@/api/libs/modules/wms-request'

/**
 * 获取下架建议单列表
 */
export const getListApi = (data : any) : Promise<any> => {
	return request.get({ version: `v1`, url: `/WithdrawOrder/pda/page`, data })
}
/**
 * 获取下架建议单列表汇总数据
 */
export const getPdaGroupApi = (data : any) : Promise<any> => {
	return request.get({ version: `v1`, url: `/WithdrawOrder/pda/getPdaGroup`, data })
}