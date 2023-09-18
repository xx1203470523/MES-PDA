import request from '@/api/libs/modules/wms-request'

/**
 * 获取下架单,单条数据查询
 */
export const getDetailApi = (data : any) : Promise<any> => {
	return request.get({ version: `v1`, url: `/WithdrawOrder/pda/detail/getPdaDetail`, data })
}

/**
 * 获取下架单,单条数据查询汇总
 */
export const getPdaGroupApi = (data : any) : Promise<any> => {
	return request.get({ version: `v1`, url: `/WithdrawOrder/pda/detail/getPdaGroup`, data })
}