import type { PdaListItem } from '@/components/pda/pda-list/pda-list-types'
import type { ReceiptDetailData } from './received-operate-types'

import { reactive, watch } from "vue"

import { startPlay } from '@/utils/voice-utils'
import { back } from '@/utils/route-utils'

import { getTemporaryStorageBinAsync } from '@/api/modules/wms/common/common-config'
import { getDetailAsync, detailSumAsync, materialCheckAsync, materialScanAsync, confirmAsync } from '@/api/modules/wms/receive/receive-order'
import { warehouseBinScanAsync } from '@/api/modules/wms/basic/basic-warehouseBin'

export function init({ vaild, quantityInputFocus, materialCodeInputFocus, warehouseBinCodeInputFocus }) {
	const page = reactive<{
		windowInfo ?: UniNamespace.GetWindowInfoResult,
		input : {
			warehouseId : string
			receiptOrderId : string,
			warehouseBinCode : string,
			materialCode : string,
			quantity : number
		},
		handle : {
			needConfirmQuantity : boolean
		},
		info : {
			receiptOrderCode : string,
			assignedTotal : number,
			receiptTotal : number
		},
		scan : {
			assignedTotal : number,
			receiptTotal : number,
			unit : string
		},
		result : {
			items : PdaListItem[],
			data : ReceiptDetailData
		}
	}>({
		input: {
			warehouseId: '',
			receiptOrderId: '',
			warehouseBinCode: '',
			materialCode: '',
			quantity: 1
		},
		handle: {
			needConfirmQuantity: false
		},
		info: {
			receiptOrderCode: '',
			assignedTotal: 0,
			receiptTotal: 0
		},
		scan: {
			assignedTotal: 0,
			receiptTotal: 0,
			unit: ''
		},
		result: {
			items: [
				{
					label: '入库单号',
					field: 'receiptOrderCode'
				},
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
			data: null
		}
	});

	const rules = {
		warehouseBinCode: {
			rules: [
				{
					required: true,
					errorMessage: '请扫描库位编码',
				}
			],
			validateTrigger: 'submit'
		},
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
				}
			],
			validateTrigger: 'submit'
		}
	}

	/**
	 * 重载明细信息
	 */
	async function reloadDetailInfo() {
		const { warehouseId, receiptOrderCode } = await getDetailAsync(page.input.receiptOrderId)
		const warehouseBin = await getTemporaryStorageBinAsync({ warehouseId })

		page.info.receiptOrderCode = receiptOrderCode
		page.input.warehouseBinCode = warehouseBin.binCode
		page.input.warehouseId = warehouseId

		if (page.input.warehouseBinCode) {
			materialCodeInputFocus()
		}
	}

	/**
	 * 重载明细统计数量
	 */
	async function reloadDetailQuantitySum() {
		const { assignedTotal, receiptTotal } = await detailSumAsync(page.input.receiptOrderId)

		page.info.assignedTotal = assignedTotal
		page.info.receiptTotal = receiptTotal

		if (page.info.assignedTotal == page.info.receiptTotal) {
			uni.showModal({
				title: '收货完成，是否确认？',
				success: ({ confirm }) => {
					if (confirm) {
						confirmAsync([
							{
								id: page.input.receiptOrderId
							}
						]).then(_ => {
							uni.showToast({
								icon: 'success',
								title: '收货完成',
								success: function () {
									back({
										redirectUrl: '/pages/received/order/main/received-main'
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
		if (!await vaild()) {
			return
		}

		uni.showLoading({
			title: '物料扫描中...',
			mask: true
		})

		try {
			const result = await materialScanAsync({
				receiptOrderId: page.input.receiptOrderId,
				materialCode: page.input.materialCode,
				warehouseBinCode: page.input.warehouseBinCode,
				quantity: page.input.quantity
			})

			page.result.data = { ...result, receiptOrderCode: page.info.receiptOrderCode }

			page.scan.unit = result.material.materialUnit
			page.scan.assignedTotal = result.assignedTotal
			page.scan.receiptTotal = result.receiptTotal

			uni.showToast({
				icon: 'success',
				title: '执行成功'
			})
			startPlay('successVoicePDA')

			materialCodeInputFocus()

		} catch (err) {
			const error = err as any

			if (error.errors.WMS10619) {
				warehouseBinCodeInputFocus()
			}

			if (error.errors.WMS10618) {
				materialCodeInputFocus()
			}

			if (error.errors.WMS10625) {
				materialCodeInputFocus()
			}

			if (error.errors.WMS10627) {
				materialCodeInputFocus()
			}

			if (error.errors.WMS10628) {
				materialCodeInputFocus()
			}

			startPlay('errorVoicePDA')
		}

		uni.hideLoading()
	}

	/**
	 * 库位确认
	 */
	async function warehouseBinConfirm() {
		try {
			await warehouseBinScanAsync({
				warehouseBinCode: page.input.warehouseBinCode,
				warehouseId: page.input.warehouseId
			})

			materialCodeInputFocus()

			startPlay('successVoicePDA')
		} catch (e) {
			warehouseBinCodeInputFocus()

			startPlay('errorVoicePDA')
		}
	}

	/**
	 * 物料确认
	 */
	async function materialConfirm() {
		if (page.handle.needConfirmQuantity) {
			try {
				const { material, noticeQuantity, receiptQuantity } = await materialCheckAsync({
					receiptOrderId: page.input.receiptOrderId,
					materialCode: page.input.materialCode
				})

				page.scan.assignedTotal = noticeQuantity
				page.scan.receiptTotal = receiptQuantity
				page.scan.unit = material.materialUnit

				page.result.data = {
					material,
					receiptOrderCode: page.info.receiptOrderCode
				}


				quantityInputFocus()
			} catch {
				materialCodeInputFocus()
			}
		} else {
			await materialScanHandle()
		}
	}

	/**
	 * 数量确认
	 */
	async function quantityConfirm() {
		await materialScanHandle()
	}

	watch(() => page.result.data, async function () {
		await reloadDetailQuantitySum()
	})

	watch(() => page.handle.needConfirmQuantity, function (n) {
		materialCodeInputFocus()
	})

	return {
		page,
		rules,
		reloadDetailInfo,
		reloadDetailQuantitySum,
		warehouseBinConfirm,
		materialConfirm,
		quantityConfirm
	};
}