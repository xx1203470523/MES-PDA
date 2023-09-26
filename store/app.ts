import { defineStore } from 'pinia'

import { versionGetAsync } from '@/api/modules/user-center/version'
import { versionOutputType } from '@/api/modules/user-center/version-types'

export const useAppStore = defineStore('app', {
	state: () : {
		systemInfo : UniNamespace.GetSystemInfoResult,
		newVersion : versionOutputType
	} => {
		return {
			systemInfo: null,
			newVersion: null
		}
	},
	getters: {},
	actions: {
		loadSystemInfo() {
			this.systemInfo = uni.getSystemInfoSync()
		},
		async versionCheckAsync() {
			if (!this.systemInfo) {
				this.loadSystemInfo()
			}
			
			this.newVersion = await versionGetAsync({ versionCode: Number(this.systemInfo.appVersionCode) })
		}
	}
})