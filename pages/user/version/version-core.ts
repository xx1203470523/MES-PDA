import { reactive } from "vue"

import { versionGet } from '@/api/modules/user-center/version'

import { useAppStore } from '@/store/app'

import marked from '@/components/uni/marked'

import type { versionOutputType } from '@/api/modules/user-center/version-types'

export function init() {
	const page = reactive<{
		windowInfo ?: UniNamespace.GetWindowInfoResult,
		handle : {
			isNew : boolean,
			isDownloading : boolean,
			subTitle : number,
			versionCode : string,

			task : string,
			title : string,
			content : string,
			process : string,
			progress : number,
		},
		response ?: versionOutputType
	}>({
		handle: {
			isNew: false,
			isDownloading: false,
			subTitle: 0,
			versionCode: '',

			task: '',
			title: '',
			content: '',
			process: '未执行流程',
			progress: 0,
		}
	});

	const update = reactive<{
		handle : {
			url : string,
			filename : string,
			versionCode : string,
			isRun : boolean,
			ableUpdate : boolean,
			isForceUpdate : boolean,
			updateComplete : boolean
		},
	}>({
		handle: {
			url: '',
			filename: '',
			versionCode: '',
			isRun: false,
			ableUpdate: false,
			isForceUpdate: false,
			updateComplete: false
		}
	});

	const tip = reactive<{
		handle : {
			show : boolean,
			type : string,
			title : string,
			content : string
		},
	}>({
		handle: {
			show: false,
			type: 'warn',
			title: '',
			content: ''
		}
	});

	let userStore = useAppStore()
	const wgtinfo = userStore.getVersionCode


	async function reloadVersionCode() {
		const versionget = await versionGet({ versionCode: 33, serviceType: 'wms' })
		page.handle.subTitle = versionget.versionCode
		page.handle.versionCode = wgtinfo
		// 地址、版本号、是否强制更新
		const {
			url,
			versionCode,
			content,
			isForceUpdate
		} = versionget

		if (versionCode && wgtinfo != versionCode) {
			page.handle.content = marked(content)

			update.handle.ableUpdate = true
			update.handle.isForceUpdate = (isForceUpdate == 1)
			update.handle.url = url

			update.handle.versionCode = versionCode
		} else {
			page.handle.isNew = true
			update.handle.ableUpdate = false
			tip.handle.show = true
			tip.handle.type = 'info'
			tip.handle.title = '提示'
			tip.handle.content = '您已经是最新版，无需更新'
		}
	}

async function updateHandle() {
		page.handle.isNew = true
		// #ifdef APP-PLUS
		if (!update.handle.url) {
			tip.handle.title = '更新错误'
			tip.handle.content = '更新地址获取异常，请联系开发人员'
			tip.handle.show = true
			page.handle.isNew = false
			return
		}

		// 下载任务创建
		try {
			update.handle.isRun = true
			const downloadObj = plus.downloader.createDownload(update.handle.url.replace('//', 'http://'), {}, function (d, status) {

				update.handle.filename = d.filename
				update.handle.isRun = false
				if (status == 200) {
					installHandle()
				} else {
					uni.showToast({
						icon: 'none',
						title: `下载异常：${status}`
					})

					plus.downloader.clear();
				}
			})

			// 开始下载
			downloadObj.start()

			// 监听下载对象执行进度
			downloadObj.addEventListener('statechanged', function (task, status) {
				// 给下载任务设置一个监听
				switch (task.state) {
					case 1:
						page.handle.process = '下载准备中'
						break;
					case 2:
						page.handle.process = '已连接到服务器'
						break;
					case 3:
						page.handle.process = '下载中'
						page.handle.progress = task.downloadedSize / task
							.totalSize * 100
						break;
					case 4:
						page.handle.process = '下载完成'
						page.handle.isNew = true
						break;
				}
			})
		} catch (err) {
			console.log(err)
		}

		// #endif

		// #ifndef APP-PLUS

		uni.showToast({
			icon: 'none',
			title: '目前仅支持移动端app更新'
		})

		// #endif
	}
	
	async function installHandle() {
	
		// #ifdef APP-PLUS
	
		if (update.handle.filename) {
			console.log('安装准备中')
			// 安装任务完成
			page.handle.process = '安装准备中'
			plus.runtime.install(update.handle.filename, {}, function ({
				version
			}) {
				if (version == update.handle.versionCode) {
					page.handle.process = '安装完成'
					page.handle.task = 'update-complete'
				} else {
					if (update.handle.isForceUpdate) {
						uni.showModal({
							title: '警告',
							content: '此次更新为强制更新，必须重启应用',
							showCancel: false,
							confirmText: '重启应用',
							success: function ({
								confirm
							}) {
								if (confirm) {
									restartApp()
								}
							}
						})
					} else {
						page.handle.process = '安装失败'
						page.handle.task = 'download-complete'
	
						tip.handle.title = '提示'
						tip.handle.content = '更新包已下载完成，请安装最新安装包'
						tip.handle.show = true
					}
				}
			}, function (e) {
				tip.handle.show = true
				tip.handle.title = `安装失败【${e.code}】`
				tip.handle.content = `错误信息：${e.message}`
			})
		} else {
			uni.showToast({
				title: '无法查询到更新包，请手动安装更新包'
			})
		}
		// #endif
	}
	
	async function restartApp() {
		// #ifdef APP-PLUS
	
		plus.runtime.restart()
	
		// #endif
	}
	
	async function updateCheck() {
		// #ifndef APP-PLUS
	
		uni.showToast({
			icon: 'none',
			title: '目前仅支持移动端app更新'
		})
	
		// #endif
	}
	
	return {
		page,
		reloadVersionCode,
		update,
		tip,
		updateHandle,
		updateCheck
	}
}