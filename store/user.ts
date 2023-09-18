import { defineStore } from 'pinia'

export const useUserStore = defineStore('user', {
	state: () => {
		return {
			token: null
		}
	},
	getters: {
		getToken(state) {
			return state.token
		}
	},
	actions: {
		setToken(token : string) {
			this.token = token
		},
		delToken() {
			this.token = null
		}
	}
})