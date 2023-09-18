import type { PdaListItem } from '@/components/pda/pda-list/pda-list-types'
import type { ReceiptDetailType } from './onshelves-operate-types'

import { reactive, watch } from "vue"
import { back } from '@/utils/route-utils'
import { startPlay } from '@/utils/voice-utils'
import { getMaterialCheckAsync, materialScanAsync, getPdaGroupApi, getSum, confirmAsync } from '@/api/modules/wms/receive/receive-onshelves'
import { warehouseBinScanAsync } from '@/api/modules/wms/basic/basic-warehouseBin'

export function init({ vaild, reloadList, warehouseBinCodeInputFocus, materialCodeInputFocus, quantityInputFocus }) {
	const page = reactive<{
		windowInfo ?: UniNamespace.GetWindowInfoResult,
		search : {
			materialCode : string
		},
		input : {
			warehouseId : string
			putawayOrderId : string,
			warehouseBinId : string,
			warehouseBinCode : string,
			materialCode : string,
			quantity : number
		},
		handle : {
			needConfirmQuantity : boolean
		},
		info : {
			putawaySuggestionId : number,
			putawaySuggestionDetailId : number,
			putawaySuggestionCode : string,
			putawayOrderCode : string,
			suggestionTotal : number,
			putawayOrderTotal : number
		},
		scan : {
			suggestionTotal : number,
			putawayOrderTotal : number,
			unit : string
		},
		list : {
			items : PdaListItem[],
			data : ReceiptDetailType[]
		}
	}>({
		search: {
			materialCode: ''
		},
		input: {
			warehouseId: '',
			putawayOrderId: '',
			warehouseBinId: '',
			warehouseBinCode: '',
			materialCode: '',
			quantity: 1
		},
		handle: {
			needConfirmQuantity: false
		},
		info: {
			putawaySuggestionId : 0,
			putawaySuggestionDetailId : 0,
			putawaySuggestionCode: '',
			putawayOrderCode: '',
			suggestionTotal: 0,
			putawayOrderTotal: 0
		},
		scan: {
			suggestionTotal: 0,
			putawayOrderTotal: 0,
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
					label: '建议数量',
					field: 'suggestionQuantity'
				},
				{
					label: '建议库位',
					field: 'suggestionWarehouseBin.binCode'
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
	 * 重载明细统计数量
	 */
	async function reloadDetailQuantitySum() {
		const { suggestionTotal, putawayOrderTotal } = await getPdaGroupApi(page.input.putawayOrderId)

		page.info.suggestionTotal = suggestionTotal
		page.info.putawayOrderTotal = putawayOrderTotal

		if (suggestionTotal == putawayOrderTotal) {
			uni.showModal({
				title: '提示',
				content: '明细操作完成，是否提交！',
				success: function (res) {
					if (res.confirm) {
						confirmAsync([
							{
								id: page.input.putawayOrderId
							}
						]).then(_ => {
							uni.showToast({
								icon: 'success',
								title: '上架完成',
								success: function () {
									back({
										redirectUrl: '/pages/received/onshelves/main/onshelves-main'
									})
								}
							})

						})
					}
				}
			});
		}
	}

	/**
	 * 重新加载扫描数量
	 */
	async function reloadDetailQuantitySumByMaterialCode() {
		const { suggestionTotal, putawayOrderTotal } = await getSum({
			putawayOrderId: page.input.putawayOrderId,
			materialCode: page.input.materialCode
		})

		page.scan.suggestionTotal = suggestionTotal
		page.scan.putawayOrderTotal = putawayOrderTotal

		if (page.scan.putawayOrderTotal == page.scan.suggestionTotal) {
			page.scan.putawayOrderTotal = 0
			page.scan.suggestionTotal = 0
		}
	}

	/**
	 * 物料扫描处理
	 */
	async function materialScanHandle() {
		if (page.info.putawayOrderTotal + page.input.quantity < 0) {
			uni.showToast({
				title: '上架数量不可小于0',
				icon: 'none'
			})
			materialCodeInputFocus()
			return
		}

		if (!await vaild()) {
			return
		}


		uni.showLoading({
			title: '物料扫描中...',
			mask: true
		})

		try {
			await materialScanAsync({
				materialCode: page.input.materialCode,
				warehouseBinCode: page.input.warehouseBinCode,
				putawayOrderId: page.input.putawayOrderId,
				putawayOrderDetailAssignmentId: page.list.data[0]?.id,
				quantity: page.input.quantity
			})

			page.search.materialCode = page.input.materialCode

			uni.showToast({
				icon: 'success',
				title: '执行成功'
			})
			startPlay('successVoicePDA')

			reloadDetailQuantitySumByMaterialCode()

			reloadDetailQuantitySum()

			reloadList()

		} catch (err) {
			const error = err as any

			if (error.errors.WMS10039) {
				warehouseBinCodeInputFocus()
			}
			else if (error.errors.WMS10701) {
				materialCodeInputFocus()
			}
			else if (error.errors.WMS10039) {
				warehouseBinCodeInputFocus()
			}
			else {
				materialCodeInputFocus()
			}

			startPlay('errorVoicePDA')
		}

		uni.hideLoading()
	}

	/**
	 * 库位编码确认
	 */
	async function warehouseBinCodeConfirm() {
		try {
			const binId = await warehouseBinScanAsync({
				warehouseId: page.input.warehouseId,
				warehouseBinCode: page.input.warehouseBinCode,
			})
			page.input.warehouseBinId = binId

			startPlay('successVoicePDA')

			materialCodeInputFocus()
		} catch {
			warehouseBinCodeInputFocus()

			startPlay('errorVoicePDA')
		}
	}

	/**
	 * 物料编码确认
	 */
	async function materialCodeConfirm() {
		try {
			if (!await vaild()) {
				return
			}

			const { material } = await getMaterialCheckAsync({
				putawayOrderId: page.input.putawayOrderId,
				putawayOrderDetailAssignmentId: page.list.data[0]?.id,
				materialCode: page.input.materialCode,
				warehouseBinId: page.input.warehouseBinId,
				quantity: page.input.quantity
			})
			page.scan.unit = material.materialUnit

			if (page.handle.needConfirmQuantity) {
				if (page.list.data && page.list.data.length > 0) {
					const _data = page.list.data[0]
					if ((_data.material.materialCode.toUpperCase()) !== (page.input.materialCode.toUpperCase())) {
						reloadList()
					}
					reloadDetailQuantitySumByMaterialCode()
				}
				quantityInputFocus()
			} else {
				await materialScanHandle()
			}
		} catch (err) {
			const error = err as any

			if (error.errors.WMS10123 && page.handle.needConfirmQuantity) {
				quantityInputFocus()
			} else {
				materialCodeInputFocus()
			}

			startPlay('errorVoicePDA')
		}
	}

	/**
	 * 数量确认
	 */
	async function quantityConfirm() {
		await materialScanHandle()
	}

	watch(() => page.handle.needConfirmQuantity, function (_) {
		materialCodeInputFocus()
	})

	return {
		page,
		rules,
		reloadDetailQuantitySum,
		warehouseBinCodeConfirm,
		materialCodeConfirm,
		quantityConfirm
	};
}