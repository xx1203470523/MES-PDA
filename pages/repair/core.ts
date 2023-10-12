import type { PageType } from './types'
import type { ManuSfcBindOutputType } from '@/api/modules/mes/manuSFCBind/types'

import { reactive } from "vue";

import { NgStateEnum } from '@/api/modules/mes/manuSFCBind/enum'
import { listAsync, unbindAsync, switchBindAsync, repairSFCAsync } from '@/api/modules/mes/manuSFCBind/index'


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
					value: 0,
					// disable: true
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
				// {
				// 	label: 'NG状态',
				// 	field: 'ngState',
				// 	valuePreprocessing(v) {
				// 		switch (v) {
				// 			case 0:
				// 				return 'NG'

				// 			case 1:
				// 				return 'OK'
				// 		}
				// 	}
				// }
			],
			data: [],
			status: NgStateEnum.NG,
			isBindCount: 0
		}
	});

	/**
	 * 条码确认
	 */
	async function codeConfirmAsync() {
		if (await formVaild()) {
			try {
				const { data, ngState } = await listAsync({
					sFC: page.input.code
				})

				page.input.status = ngState

				page.result.data = data
				page.result.status = ngState
				page.result.isBindCount = data.filter(m => m.status === 1).length
			} catch { }
		}
		codeInputFocus()
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
		ngStateChangeHandleAsync,
		switchBindModalOpen,
		switchBindModalConfirm,
		switchBindModalClose,
		unbindSFCAsync
	}
}