import type { PageType } from './types'
import type { ManuSfcBindOutputType } from '@/api/modules/mes/manuSFCBind/types'

import { reactive } from "vue";

import { NgStateEnum } from '@/api/modules/mes/manuSFCBind/enum'
import { listAsync, unbindAsync, switchBindAsync, repairSFCAsync } from '@/api/modules/mes/manuSFCBind/index'

import { debounce } from '@/utils/fn-utils'

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
					text: 'NG',
					value: 0
				},
				{
					text: 'OK',
					value: 1
				}
			]
		},
		input: {
			code: '',
			status: NgStateEnum.NG,
			newBindCode: ''
		},
		chose: {
			detail: {
				manuSfcCirculationEntity: {},
				ngState: 0
			}
		},
		timeout: {},
		result: {
			items: [
				{
					label: '绑定条码',
					field: 'manuSfcCirculationEntity.sfc'
				},
				{
					label: '位置',
					field: 'manuSfcCirculationEntity.location'
				}
			],
			data: [],
			status: NgStateEnum.NG,
			isBindCount: 0
		}
	});

	/**
	 * 页面清理
	 * @return 
	 */
	function clear() {
		page.result.data = []

		if (page.timeout.codeInput) {
			clearTimeout(page.timeout.codeInput)
		}
	}

	/**
	 * 条码确认
	 */
	async function codeConfirmAsync() {
		try {
			if (!page.input.code) {
				throw '条码不可为空'
			}

			if (!await formVaild()) {
				throw '表单验证失败'
			}


			const { data, ngState } = await listAsync({
				sFC: page.input.code
			})

			page.input.status = ngState
			page.result.data = data
			page.result.status = ngState
			page.result.isBindCount = data.filter(m => m.status === 1).length

		}
		catch {
			clear()
		}
		codeInputFocus()
	}

	/**
	 * 条码输入
	 * @return 
	 */
	async function codeInputAsync() {
		page.timeout.codeInput = debounce(codeConfirmAsync, page.timeout.codeInput, 300)()
	}

	/**
	 * NG状态改变
	 * @param {number} e 
	 * @return 
	 */
	async function ngStateChangeHandleAsync(e : number) {
		if (!page.input.code) {
			return
		}

		if (e === NgStateEnum.OK && page.result.status !== NgStateEnum.OK) {
			uni.showModal({
				title: '确认更新所有状态为OK吗？',
				async success({ confirm }) {
					if (confirm) {
						await repairSFCAsync({
							sFC: page.input.code,
							operateType: NgStateEnum.OK
						})

						uni.showToast({
							title: '状态已更新',
							icon: 'success'
						})
					}
				}
			})
		}
	}

	/**
	 * 换绑窗口打开
	 */
	function switchBindModalOpen(row : { manuSfcCirculationEntity : ManuSfcBindOutputType, ngState : number }) {
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
					oldBindId: page.chose.detail.manuSfcCirculationEntity.id,
					oldBindSFC: page.chose.detail.manuSfcCirculationEntity.sfc,
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
		page.chose.detail = {
			manuSfcCirculationEntity: {},
			ngState: 0
		}
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
							uni.showToast({
								title: '全部解绑成功',
								icon: 'success'
							});
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
		codeInputAsync,
		ngStateChangeHandleAsync,
		switchBindModalOpen,
		switchBindModalConfirm,
		switchBindModalClose,
		unbindSFCAsync
	}
}