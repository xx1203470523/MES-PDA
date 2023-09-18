import type { PdaListItem } from '@/components/pda/pda-list/pda-list-types'
import type { DeliveryDetailData } from './delivery-operate-types'

import { reactive, watch } from "vue"

import { startPlay } from '@/utils/voice-utils'
import { back } from '@/utils/route-utils'

import { detailSumAsync, materialScanAsync, confirmAsync } from '@/api/modules/wms/delivery/delivery-order'

export function init({ vaild, reloadList, quantityInputFocus, materialCodeInputFocus }) {
	const page = reactive<{
		windowInfo ?: UniNamespace.GetWindowInfoResult,
		input : {
			warehouseId : string
			deliveryNoticeId : string,
			deliveryOrderId : string,
			warehouseBinCode : string,
			materialCode : string,
			quantity : number
		},
		handle : {
			needConfirmQuantity : boolean
		},
		info : {
			deliveryOrderCode : string,
			assignedTotal : number,
			deliveryTotal : number
		},
		scan : {
			noticeQuantity : number,
			deliveryQuantity : number,
			unit : string
		},
		list : {
			items : PdaListItem[],
			data : DeliveryDetailData[]
		}
	}>({
		input: {
			warehouseId: '',
			deliveryNoticeId: '',
			deliveryOrderId: '',
			warehouseBinCode: '',
			materialCode: '',
			quantity: 1
		},
		handle: {
			needConfirmQuantity: false
		},
		info: {
			deliveryOrderCode: '',
			assignedTotal: 0,
			deliveryTotal: 0
		},
		scan: {
			noticeQuantity: 0,
			deliveryQuantity: 0,
			unit: ''
		},
		list: {
			items: [
				{
					label: '物料编码',
					field: 'material.materialCode'
				},
				{
					label: '物料名称',
					field: 'material.materialName'
				},
				{
					label: '单位',
					field: 'material.materialUnit'
				}
			],
			data: []
		}
	});

	const rules = {
		materialCode: {
			rules: [
				{
					required: true,
					errorMessage: '请扫描物料编码',
				}
			],
			validateTrigger: 'submit'
		},
		quantity: {
			rules: [
				{
					required: true,
					errorMessage: '必须填入数量',
				},
				{
					maximum: 9999999999,
					errorMessage: '数量超出最大值，请重试',
				}
			],
			validateTrigger: 'submit'
		}
	}

	/**
   * 重载明细统计数量
   */
	async function reloadDetailQuantitySum() {

		const { assignedTotal, deliveryTotal } = await detailSumAsync(page.input.deliveryOrderId)

		page.info.assignedTotal = assignedTotal
		page.info.deliveryTotal = deliveryTotal

		if (page.info.assignedTotal == page.info.deliveryTotal) {
			//完成操作时，直接出库数等于通知数
			page.scan.deliveryQuantity = page.info.deliveryTotal
			page.scan.noticeQuantity = page.info.assignedTotal
			uni.showModal({
				title: '出库验证完成，是否确认？',
				success: ({ confirm }) => {
					if (confirm) {
						confirmAsync([page.input.deliveryOrderId])
							.then(_ => {
								uni.showToast({
									icon: 'success',
									title: '出库完成',
									success: function () {
										back({
											redirectUrl: '/pages/delivery/order/main/delivery-main'
										})
									},
									fail: () => {
										materialCodeInputFocus()
									}
								})

							})
					}
				}
			})
		}
	}

	/**
	 * 物料扫描处理
	 */
	async function materialScanHandle() {
		if (page.info.deliveryTotal + page.input.quantity < 0) {
			uni.showToast({
				title: '扫描数量不可小于0',
				icon: 'none'
			})
			materialCodeInputFocus()
			return
		}

		if (!await vaild()) {
			return
		}
	}

	/**
	 * 物料确认
	 */
	async function materialConfirm() {

		if (!await vaild()) {
			return
		}

		materialScanHandle()

		uni.showLoading({
			title: '物料扫描中...',
			mask: true
		})

		if (page.handle.needConfirmQuantity) {
			const { material } = await materialScanAsync({
				materialCode: page.input.materialCode,
				deliveryNoticeId: page.input.deliveryNoticeId,
				deliveryOrderId: page.input.deliveryOrderId,
				quantity: 0
			})

			page.scan.unit = material.materialUnit;

			quantityInputFocus()

			uni.hideLoading()

		} else {

			try {
				const { material } = await materialScanAsync({
					materialCode: page.input.materialCode,
					deliveryNoticeId: page.input.deliveryNoticeId,
					deliveryOrderId: page.input.deliveryOrderId,
					quantity: page.input.quantity
				})

				page.scan.unit = material.materialUnit;

				reloadDetailQuantitySum()

				startPlay('successVoicePDA')

				uni.showToast({
					icon: 'success',
					title: '执行成功'
				})

				reloadList()
				uni.hideLoading()
			} catch {
				materialCodeInputFocus()
				startPlay('errorVoicePDA')
			}
		}

		uni.hideLoading()
	}

	/**
	 * 数量确认
	 */
	async function quantityConfirm() {

		if (!await vaild()) {
			return
		}

		materialScanHandle()
		if (page.handle.needConfirmQuantity) {
			uni.showLoading({
				title: '物料扫描中...',
				mask: true
			})

			try {
				await materialScanAsync({
					materialCode: page.input.materialCode,
					deliveryNoticeId: page.input.deliveryNoticeId,
					deliveryOrderId: page.input.deliveryOrderId,
					quantity: page.input.quantity
				})

				uni.showToast({
					icon: 'success',
					title: '执行成功'
				})
				startPlay('successVoicePDA')

				reloadDetailQuantitySum()

				reloadList()

			} catch {
				materialCodeInputFocus()
				startPlay('errorVoicePDA')
			}

			uni.hideLoading()
		}
	}

	watch(() => page.handle.needConfirmQuantity, function (_) {
		materialCodeInputFocus()
	})

	return {
		page,
		rules,
		reloadDetailQuantitySum,
		materialConfirm,
		quantityConfirm
	};
}