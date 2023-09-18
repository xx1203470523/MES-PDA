/**
 * 版本工具
 */

import { reLaunch } from './route-utils'

import { versionCheck } from '@/api/modules/user-center/version'

import { useAppStore } from '@/store/app'

const versionUtilsStatus = {
	versionUpdateCheckedTimeout: null,
	isOpenUpdateModal: false
}

async function checkVersion() {
	try {
		let routes = getCurrentPages();

		if (routes && routes.length <= 0) {
			restartAutoUpdate()
			return
		}

		const curRoute = routes[routes.length - 1]?.route
		if (curRoute && curRoute == 'pages/user/version/version') {
			restartAutoUpdate()
			return
		}
		let userStore = useAppStore()
		const wgtinfo = userStore.getVersionCode

		if (!wgtinfo) {
			restartAutoUpdate()
			return
		}

		try {
			const { hasNewVersion, isForceUpdate } = await versionCheck({versionCode:wgtinfo,serviceType:'wms'})
			if (hasNewVersion) {
				if (isForceUpdate && !versionUtilsStatus.isOpenUpdateModal) {
					versionUtilsStatus.isOpenUpdateModal = true
					uni.showModal({
						title: '有新版本',
						content: '此版本需要强制更新，请点击确定跳转更新页面',
						showCancel: false,
						success: () => {		
							reLaunch('/pages/user/version/version')
						},
						complete: () => {
							versionUtilsStatus.isOpenUpdateModal = false
						}
					})
				} else {
					restartAutoUpdate()
				}
			} else {
				restartAutoUpdate()
			}
		} catch {
			restartAutoUpdate()
		}
	} catch (err) {
		console.log(err)
		restartAutoUpdate()
	}
}

function restartAutoUpdate() {
	autoUpdate()
}

export function autoUpdate() {
	if (versionUtilsStatus.versionUpdateCheckedTimeout) {
		clearTimeout(versionUtilsStatus.versionUpdateCheckedTimeout)
	}
	versionUtilsStatus.versionUpdateCheckedTimeout = setTimeout(async () => {
		await checkVersion()
	}, 2000)
}