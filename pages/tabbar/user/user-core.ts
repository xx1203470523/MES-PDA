import { computed, reactive } from "vue";

import { useUserStore } from '@/store/user'

import { delCache } from '@/utils/cache-utils'
import { to, reLaunch } from '@/utils/route-utils'

import { getUserInfoAsync } from '@/api/modules/user-center/user'
// import { getWarehouseListApiAsync, getDefaultWarehouseAsync } from '@/api/modules/wms/common/common-selector'
// import { saveWarehouseDataApi } from '@/api/modules/wms/config/config-user'

import type { userInfoType } from '@/api/modules/user-center/user-types'

export function init() {
	const page = reactive<{
		windowInfo ?: UniNamespace.GetWindowInfoResult,
		userInfo : userInfoType,
		warehouse : {
			show : boolean
			list : Array<any>
			data ?: {
				defaultWarehouseId: string
				warehouseName: string
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
	 * 页面高度
	 */
	const pageHeight = computed(() => page.windowInfo.windowHeight - 50)

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
					reLaunch('/pages/login/login')
				}
			}
		})
	}

	/**
	 * 到版本更新页面
	 */
	function toVersionUpdate() {
		to('/pages/user/version/version')
	}

	/**
	 * 到修改密码
	 */
	function toUpdatePassword() {
		to('/pages/user/operate/password/password-operate')
	}

	/**
	 * 到更新用户信息
	 */
	function toUpdateUserInfo() {
		to('/pages/user/operate/userinfo/userinfo-operate')
	}

	/**
	 * 重新加载默认仓库
	 */
	async function reloadUserDefaultWarehouse() {
		if (page.userInfo.user) {
			// page.warehouse.data = await getDefaultWarehouseAsync()
		}
	}

	/**
	 * 打开仓库选择器
	 */
	async function openWarehouseSetting() {
		// page.warehouse.list = await getWarehouseListApiAsync()

		if (page.warehouse.data && page.warehouse.data.defaultWarehouseId) {
			const _index = page.warehouse.list.findIndex(m => m.value == page.warehouse.data.defaultWarehouseId)
			if (_index > -1) {
				page.warehouse.list[_index].checked = true
			}
		}

		page.warehouse.show = true
	}

	/**
	 * 关闭仓库选择器
	 */
	function closeWarehouseSetting() {
		page.warehouse.show = false
		page.warehouse.list = []
	}

	/**
	 * 保存仓库配置
	 */
	async function saveWarehouseSetting({ options }) {
		if (options && options.value) {
			const { value } = options

			try {
				// await saveWarehouseDataApi({
				// 	defaultWarehouseId: value
				// })
				
				closeWarehouseSetting()
				
				await reloadUserDefaultWarehouse()

				uni.showToast({
					icon: 'success',
					title: '设置成功'
				})
			} catch { }
		}
	}


	return {
		page,
		pageHeight,
		loginOut,
		reloadUserInfoAsync,
		toVersionUpdate,
		toUpdatePassword,
		toUpdateUserInfo,
		reloadUserDefaultWarehouse,
		openWarehouseSetting,
		closeWarehouseSetting,
		saveWarehouseSetting
	}
}