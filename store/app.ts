import { defineStore } from 'pinia'

export const useAppStore = defineStore('app', {
	state: () => ({
		version: {
			code: '',
			hasNewVersion: false
		}
	}),
	actions: {
		setHasNewVersion(hasNewVersion : boolean) {			
			this.version.hasNewVersion = hasNewVersion
		},
		setversionCode(code: string){
			this.version.code = code
		}
	},
	getters: {
		getVersion({ version }) {
			return version.hasNewVersion
		},
		getVersionCode({ version }) {
			return version.code
		}
	}
})