import type { PageType } from './types'

import marked from '@/components/uni/marked'

import { reactive } from "vue"

import { useAppStore } from '@/store/app'

import { versionGetAsync } from '@/api/modules/user-center/version'

export function init() {
	const appStore = useAppStore()

	/**
	 * 页面数据
	 */
	const page = reactive<PageType>({
		handle: {
			fileName: '',
			isDownload: false,
			progress: 0,
			progressTitle: '未开始'
		},
		result: {
			info: {},
			content: null
		}
	})

	/**
	 * 更新检查
	 */
	async function updateCheckAsync() {
		page.result.info = await versionGetAsync({ versionCode: Number(appStore.systemInfo.appVersionCode) })
		page.result.content = marked(page.result.info.content)
	}

	/**
	 * 更新处理
	 */
	function updateDownloadHandle() {
		// #ifdef APP
		if (page.handle.fileName) {
			page.handle.fileName = null
		}

		if (page.handle.isDownload) {
			uni.showToast({
				title: '正在下载中，请不要重复下载',
				icon: 'none'
			})
			return
		}

		page.handle.isDownload = true

		try {
			const download = plus.downloader.createDownload(page.result.info.url.replace('//', 'http://'), {}, function (d, status) {
				page.handle.fileName = d.filename
				page.handle.isDownload = false
				if (status == 200) {
					if (page.result.info.isForceUpdate) {
						installHandle()
					} else {
						uni.showModal({
							title: '操作提示',
							content: '下载完成，是否自动安装？',
							success({ confirm }) {
								if (confirm) {
									installHandle()
								}
							}
						})
					}
				} else {
					uni.showToast({
						icon: 'none',
						title: `下载异常：${status}`
					})

					plus.downloader.clear();
				}
			})

			download.addEventListener('statechanged', function (task) {
				switch (task.state) {
					case 1:
						page.handle.progressTitle = '下载准备中'
						break;
					case 2:
						page.handle.progressTitle = '已连接到服务器'
						break;
					case 3:
						page.handle.progressTitle = '下载中'
						page.handle.progress = task.downloadedSize / task
							.totalSize * 100
						break;
					case 4:
						page.handle.progressTitle = '下载完成'
						break;
				}
			})

			download.start()
		} catch (e) {
			console.error(e)
			page.handle.isDownload = false
		}
		// #endif

		// #ifndef APP

		uni.showToast({
			title: '只有移动端才可更新',
			icon: 'none'
		})

		// #endif
	}

	/**
	 * 安装处理
	 */
	function installHandle() {
		if (!page.handle.fileName) {
			return
		}

		page.handle.progressTitle = '安装准备中'

		uni.showModal({
			title: '更新说明',
			content: '此次更新为强制更新，会自动安装应用后重启应用',
			showCancel: false,
			confirmText: '重启应用',
			success() {
				plus.runtime.install(page.handle.fileName, {}, function () {
					plus.runtime.restart()
				}, function (e) {
					uni.showModal({
						title: '安装失败',
						content: e.message
					})
				})
			}
		})
	}

	return {
		page,
		appStore,
		updateCheckAsync,
		updateDownloadHandle
	}
}