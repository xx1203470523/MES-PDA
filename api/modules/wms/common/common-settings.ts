import request from '@/api/libs/modules/wms-request'

/**
 * 获取系统配置
 * @returns
 */
export const getCommonSetting = ({ warehouseId }): any => {
  return request.get({  version: `v1`,url: `/CommonSetting/pda/getCommonSettings/${warehouseId}` })
}

/**
 * 获取用户默认仓库
 * @returns
 */
export const getUserDefaultWarehouseAsync = (): Promise<any> => {
  return request.get({ version: `v1`,url: '/ConfigUser/getDefaultWarehouseId' })
}