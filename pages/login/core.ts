import { computed, reactive } from "vue";

import { useUserStore } from '@/store/user'

// import { useCommonSettingsStore } from '@/store/commonsettings'

import { setCache } from '@/utils/cache-utils'
import { redirectTo } from '@/utils/route-utils'

import { loginAsync } from '@/api/modules/user-center/login'

export const rules = {
	username: {
		rules: [
			{
				required: true,
				errorMessage: '请输入工号',
			}
		]
	},
	password: {
		rules: [
			{
				required: true,
				errorMessage: '请输入密码',
			}
		]
	}
}

export function init({ vaild }) {
	const userStore = useUserStore()
	
	// const userCommonSetting = useCommonSettingsStore()
	
	const page = reactive<{
		windowInfo ?: UniNamespace.GetWindowInfoResult,
		redirect ?: string,
		input : {
			username : string,
			password : string,
			loginRemember : boolean
		}
	}>({
		input: {
			username: '',
			password: '',
			loginRemember: true
		}
	});

	const pageHeight = computed(() => page.windowInfo.windowHeight - 44)

	async function loginHandle() {
		if (!await vaild()) {
			return
		}

		uni.showLoading({
			title: '用户登录中'
		})

		try {
			const data = await loginAsync({
				username: page.input.username,
				password: page.input.password
			})
			
			userStore.setToken(data.token)
			
			//获取系统配置
			// await userCommonSetting.getCommonSettingsAsync()

			setCache({
				key: 'token',
				value: data.token
			})

			setCache({
				key: 'login-remember',
				value: page.input.loginRemember
			})

			uni.showToast({
				icon: 'success',
				title: '登录成功'
			})

			if (page.redirect) {
				redirectTo(`/${page.redirect}`)
			} else {
				redirectTo('/pages/tabbar/user/index')
			}
		} catch {

		} finally {
			uni.hideLoading()
		}
	}

	return {
		page,
		loginHandle
	};
}