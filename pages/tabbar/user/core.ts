import type { userInfoType } from '@/api/modules/user-center/user-types'

import { computed, reactive } from "vue";

import { useUserStore } from '@/store/user'

import { delCache } from '@/utils/cache-utils'
import { to, reLaunch } from '@/utils/route-utils'

import { getUserInfoAsync } from '@/api/modules/user-center/user'

export function init() {
	const page = reactive<{
		windowInfo ?: UniNamespace.GetWindowInfoResult,
		userInfo : userInfoType,
		warehouse : {
			show : boolean
			list : Array<any>
			data ?: {
				defaultWarehouseId : string
				warehouseName : string
			}
		}
		receivingRecords : number,
		onShelvesRecords : number
	}>({
		userInfo: {},
		warehouse: {
			show: false,
			list: []
		},
		receivingRecords: 3842,
		onShelvesRecords: 0
	})
	
	/**
	 * 重载用户信息
	 */
	async function reloadUserInfoAsync() {
		page.userInfo = await getUserInfoAsync()
	}

	/**
	 * 注销
	 */
	function loginOut() {
		const userStore = useUserStore()

		uni.showModal({
			title: '确认退出',
			success: function ({ confirm }) {
				if (confirm) {
					userStore.delToken()
					delCache('token')
					reLaunch('/pages/login/index')
				}
			}
		})
	}

	/**
	 * 到版本更新页面
	 */
	function toVersionUpdate() {
		to('/pages/user/version/index')
	}

	/**
	 * 到修改密码
	 */
	function toUpdatePassword() {
		to('/pages/user/password/index')
	}

	/**
	 * 到更新用户信息
	 */
	function toUpdateUserInfo() {
		to('/pages/user/userinfo/index')
	}
	
	/**
	 * 到版本更新
	 */
	function toUpdateVersion(){
		to('/pages/version/index')
	}

	return {
		page,
		loginOut,
		reloadUserInfoAsync,
		toVersionUpdate,
		toUpdatePassword,
		toUpdateUserInfo,
		toUpdateVersion
	}
}