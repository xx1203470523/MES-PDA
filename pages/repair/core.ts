import type { PageType } from './types'
import type { ManuSfcBindOutputType } from '@/api/modules/mes/manuSFCBind/types'

import { reactive } from "vue";

import { listAsync, unbindAsync, switchBindAsync } from '@/api/modules/mes/manuSFCBind/index'


export function init({
	formVaild,
	switchBindFormVaild,
	codeInputFocus,
	newBindCodeInputFocus
}) {
	const page = reactive<PageType>({
		formRules: {
			pageRules: {
				code: {
					rules: [
						{
							required: true,
							errorMessage: '请扫描编码',
						}
					]
				}
			},
			switchBindRules: {
				newBindCode: {
					rules: [
						{
							required: true,
							errorMessage: '请扫描新条码',
						}
					]
				}
			}

		},
		modal: {
			switchBind: {
				show: false,
				isTasking: false
			}
		},
		selected: {
			options: [
				{
					text: 'OK',
					value: 0
				},
				{
					text: 'NG',
					value: 1
				}
			]
		},
		input: {
			code: '',
			status: 0,
			newBindCode: ''
		},
		chose: {
			detail: {}
		},
		result: {
			items: [
				{
					label: '绑定条码',
					field: 'bindSFC'
				}
			],
			data: [],
			isBindCount: 0
		}
	});

	/**
	 * 条码确认
	 */
	async function codeConfirmAsync() {
		if (await formVaild()) {
			try {
				const { data } = await listAsync({
					sFC: page.input.code
				})

				page.result.data = data
				page.result.isBindCount = data.filter(m => m.status === 1).length
			} catch { }
		}
		codeInputFocus()
	}

	/**
	 * 换绑窗口打开
	 */
	function switchBindModalOpen(row : ManuSfcBindOutputType) {
		page.modal.switchBind.show = true
		page.chose.detail = row


		setTimeout(() => {
			newBindCodeInputFocus()
		}, 450)
	}

	/**
	 * 确认换绑
	 */
	async function switchBindModalConfirm() {
		page.modal.switchBind.isTasking = true
		if (await switchBindFormVaild()) {
			try {
				await switchBindAsync({
					sFC: page.input.code,
					oldBindId: page.chose.detail.id,
					oldBindSFC: page.chose.detail.bindSFC,
					newBindSFC: page.input.newBindCode
				})
				uni.showToast({
					title: '换绑成功',
					icon: 'success'
				});
				await codeConfirmAsync()
				switchBindModalClose()
			} catch {
				newBindCodeInputFocus()
			}
		} else {
			newBindCodeInputFocus()
		}
		page.modal.switchBind.isTasking = false
	}

	/**
	 * 换绑庄口关闭
	 */
	function switchBindModalClose() {
		page.modal.switchBind.show = false
		page.chose.detail = {}
		codeInputFocus()
	}

	/**
	 * 全部解绑
	 */
	async function unbindSFCAsync() {
		if (await formVaild()) {
			uni.showModal({
				title: '操作警告',
				content: '确认全部解绑吗？',
				async success({ confirm }) {
					if (confirm) {
						uni.showLoading({
							title: '全部解绑中...',
							mask: true
						})
						try {
							await unbindAsync({
								sFC: page.input.code
							})
						} catch { }
						uni.hideLoading()
					}
				}
			})

		}
		codeInputFocus()
	}

	return {
		page,
		codeConfirmAsync,
		switchBindModalOpen,
		switchBindModalConfirm,
		switchBindModalClose,
		unbindSFCAsync
	}
}