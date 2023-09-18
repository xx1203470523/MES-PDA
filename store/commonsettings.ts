import { defineStore } from 'pinia'

import { getCommonSetting, getUserDefaultWarehouseAsync } from '@/api/modules/wms/common/common-settings'

export const useCommonSettingsStore = defineStore('commonsettings', {
	state: () => {
		return {
			commonSettingsInfo: null
		}
	},
	getters: {
	},
	actions: {
		/**
		 * 设置系统配置
		 * @param 
		 * @returns 1 扫描 2 输入
		*/
		async getCommonSettingsAsync() {
			//获取默认仓库
			const defaultWarehouse = await getUserDefaultWarehouseAsync()

			//获取PDA系统配置
			if(defaultWarehouse.defaultWarehouseId){
				const commonsettings = await getCommonSetting(
					{
						warehouseId: defaultWarehouse.defaultWarehouseId
					});
					
					this.commonSettingsInfo = commonsettings
					
					return commonsettings
			}
		},

	}
})