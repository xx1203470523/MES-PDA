import { reactive, watch } from "vue";
import type { PdaListItem } from '@/components/pda/pda-list/pda-list-types'
import type { TakeData } from './stocktake-operate-types'
import { startPlay } from '@/utils/voice-utils'
import { materialScanAsync, getById, getSum ,getmaterial} from '@/api/modules/wms/stock-take/stock-take-main'
import { warehouseBinScanAsync } from '@/api/modules/wms/basic/basic-warehouseBin'
export function init({ vaild, binCodeInputFocus, quantityInputFocus, materialCodeInputFocus }) {
	const page = reactive<{
		windowInfo ?: UniNamespace.GetWindowInfoResult,
		search : {
			materialCode : string
		}, input : {
			id : string,
			code : string,
			warehouseId : string
			binCode : string,
			materialCode : string,
			quantity : number,
		},
		handle : {
			needConfirmQuantity : boolean
		}
		info : {
			quantity : number,
			bookQuantity : number,
			takeTotal : number,
			bookTotal : number
		},
		scan : {
			unit : string
		},
		list : {
			items : PdaListItem[],
			data : TakeData[]
		}
	}>({
		search: {
			materialCode: ''
		},
		input: {
			id: '',
			code: '',
			warehouseId: '',
			binCode: '',
			materialCode: '',
			quantity: 1
		},
		handle: {
			needConfirmQuantity: false
		},
		info: {
			quantity: 0,
			bookQuantity: 0,
			takeTotal: 0,
			bookTotal: 0
		},
		scan: {
			unit: ''
		},
		list: {
			items: [
				{
					label: '盘点单号',
					field: 'checkCode'
				},
				{
					label: '采集单号',
					field: 'takeCode'
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
					label: '参考数量',
					field: 'bookQuantity'
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
		binCode: {
			rules: [
				{
					required: true,
					errorMessage: '请扫描或输入库位编码',
				}
			],
			validateTrigger: 'submit'
		},
		materialCode: {
			rules: [
				{
					required: true,
					errorMessage: '请扫描或输入物料编码',
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
		const data = await getById(page.input.id)
		page.input.warehouseId = data.warehouseId
	}

	/**
	 * 搜索
	 */
	async function reloadDetailQuantitySum() {
		const { takeTotal, bookTotal } = await getSum(page.input.id);
		page.info.takeTotal = takeTotal
		page.info.bookTotal = bookTotal
	}

	/**
	 * 物料扫描
	 */
	async function materialScan() {
		if (!await vaild()) { return }
		uni.showLoading({
			title: '物料扫描中...',
			mask: true
		})

		try {
			const data = await materialScanAsync({
				id: page.input.id,
				materialCode: page.input.materialCode,
				binCode: page.input.binCode,
				quantity: page.input.quantity
			})
             console.log(data)
			// if (data[0].quantity == 0) {
			// 	page.list.data = null
			// } else {
				page.list.data = data
			// }
			page.scan.unit = data[0].material.materialUnit
			page.info.quantity = data[0].quantity
			page.info.bookQuantity = data[0].bookQuantity
			page.info.takeTotal = data[0].takeTotal
			page.info.bookTotal = data[0].bookTotal
			uni.showToast({
				icon: 'success',
				title: '执行成功'
			})
			startPlay('successVoicePDA')

			if (page.handle.needConfirmQuantity) {
				binCodeInputFocus()
			} else {
				materialCodeInputFocus()
			}
		} catch (err) {
			const error = err as any
			if (error.errors.WMS10039) {
				binCodeInputFocus()
			}
			else {
				materialCodeInputFocus()
			}

			startPlay('errorVoicePDA')
		}

		uni.hideLoading()
	}

	/**
	  * 库位确认
	  */
	async function binConfirm() {
		try {
			await warehouseBinScanAsync({
				warehouseBinCode: page.input.binCode,
				warehouseId: page.input.warehouseId
			})

			startPlay('successVoicePDA')
			materialCodeInputFocus()
		} catch (e) {
			binCodeInputFocus()
			startPlay('errorVoicePDA')
		}
	}

	/**
	 * 物料扫描
	 */
	async function materialConfirm() {
		if (page.handle.needConfirmQuantity) {
			const data = await getmaterial({
				id: page.input.id,
				materialCode: page.input.materialCode,
				binCode: page.input.binCode,
				quantity: 0
			})		 
			page.list.data = data 
			page.scan.unit = data[0].material.materialUnit
			page.info.quantity = data[0].quantity
			page.info.bookQuantity = data[0].bookQuantity
			page.info.takeTotal = data[0].takeTotal
			page.info.bookTotal = data[0].bookTotal
			quantityInputFocus()
			return
		}
		await materialScan()
	}

	/**
	 * 数量确认
	 */
	async function quantityConfirm() {
		await materialScan()
	}

	watch(() => page.list.data, async function () {
		await reloadDetailQuantitySum()
	})

	watch(() => page.handle.needConfirmQuantity, function (n) {
		materialCodeInputFocus()
	})

	return {
		page,
		reloadDetailInfo,
		reloadDetailQuantitySum,
		rules, 
		binConfirm,
		materialConfirm,
		quantityConfirm
	}
}