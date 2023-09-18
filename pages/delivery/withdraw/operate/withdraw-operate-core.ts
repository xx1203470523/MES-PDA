import { ref, reactive } from "vue";
import type { PdaListItem } from '@/components/pda/pda-list/pda-list-types'
import type { WithdrawPageData } from './withdraw-operate-types'
import { getOneAsync, getDetailAsync, skipAsync } from '@/api/modules/wms/withdraw/withdraw-operate'
import { warehouseBinScanAsync } from '@/api/modules/wms/basic/basic-warehouseBin'
import { pdaMaterialScanAsync, withdrawConfirmAsync, getPdaGroupAsync, getSumAsync } from '@/api/modules/wms/withdraw/withdraw-operate'
import { startPlay } from '@/utils/voice-utils'
import { to, back } from '@/utils/route-utils'

export function init({ vaild, quantityInputFocus, materialCodeInputFocus, warehouseBinCodeInputFocus }) {
	const page = reactive<{
		windowInfo ?: UniNamespace.GetWindowInfoResult,
		search : {
			materialCode : string
		},
		input : {
			withdrawOrderId : string,
			warehouseBinCode : string,
			materialCode : string,
			quantity : number
		},
		handle : {
			needConfirmQuantity : boolean
		},
		list : {
			items : PdaListItem[],
			data : WithdrawPageData[]
		},
		scan : {
			suggestionTotal : number,
			withdrawOrderTotal : number,
			materialUnit : string
		}
		info : {
			suggestionTotal : number,
			withdrawOrderTotal : number,
			warehouseId : string,
			withdrawSuggestionId : number,
			withdrawSuggestionDetailId : number
		}
	}>({
		search: {
			materialCode: ''
		},
		input: {
			withdrawOrderId: '',
			warehouseBinCode: '',
			materialCode: '',
			quantity: 1
		},
		handle: {
			needConfirmQuantity: false
		},
		list: {
			items: [
				{
					label: '物料编码',
					field: 'materialCode'
				},
				{
					label: '物料名称',
					field: 'materialName'
				},
				{
					label: '建议库位',
					field: 'warehouseBinCode'
				},
				{
					label: '建议数量',
					field: 'withdrawSuggestionBinQuantity'
				},
				{
					label: '单位',
					field: 'materialUnit'
				},
				{
					label: '下架单号',
					field: 'withdrawOrderCode'
				},
				{
					label: '下架建议单号',
					field: 'withdrawSuggestionCode'
				}
			],
			data: []
		},
		scan: {
			suggestionTotal: 0,
			withdrawOrderTotal: 0,
			materialUnit: ''
		},
		info: {
			suggestionTotal: 0,
			withdrawOrderTotal: 0,
			warehouseId: '',
			withdrawSuggestionId: 0,
			withdrawSuggestionDetailId: 0
		}
	});

	const rules = {
		warehouseBinCode: {
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
				},
				{
					validateFunction: function (rule : any, value : number, data, callback) {
						//允许为负数
						if (value == 0) {
							callback('不能为0')
						}
						return true
					}
				}
			],
			validateTrigger: 'submit'
		}
	}

	/**
	 * 分页对象
	 */
	const paging = ref(null)

	/**
	* 查询  pageIndex : number, pageSize : number
	*/
	async function queryList() {
		try {

			const data = await getOneAsync({
				withdrawOrderId: page.input.withdrawOrderId,
				materialCode: page.search.materialCode,
			});
			
			//记录建议单Id和明细Id，以便做跳过功能
			page.info.withdrawSuggestionId = data.withdrawSuggestionId
			page.info.withdrawSuggestionDetailId = data.withdrawSuggestionDetailId
			
			if (data)
				page.list.data[0] = data
			else
				page.list.data = []

			if (page.info.suggestionTotal == page.info.withdrawOrderTotal) {
				paging.value.complete([])
			}
			else {
				paging.value.complete(page.list.data)
			}

		}
		catch (error) {
			paging.value.complete(false)
		}
	}
	/**
	 * 重载明细统计数量
	 */
	async function reloadDetailQuantitySum() {
		const { suggestionTotal, withdrawOrderTotal } = await getPdaGroupAsync(page.input.withdrawOrderId)

		page.info.suggestionTotal = suggestionTotal
		page.info.withdrawOrderTotal = withdrawOrderTotal

		//如果总扫描数等于等下架数，则弹出确认提示
		if (suggestionTotal == withdrawOrderTotal) {
			uni.showModal({
				title: '提示',
				content: '所有条码已扫描，是否完成',
				success: function (res) {
					if (res.confirm) {
						confirmAsync();
					}
				}
			})
		}
	}
	/**
	 * 重新加载扫描数量
	 */
	async function reloadDetailQuantitySumByMaterialCode() {
		const { suggestionTotal, withdrawOrderTotal, materialUnit } = await getSumAsync({
			withdrawOrderId: page.input.withdrawOrderId,
			materialCode: page.input.materialCode
		})

		page.scan.suggestionTotal = suggestionTotal
		page.scan.withdrawOrderTotal = withdrawOrderTotal
		page.scan.materialUnit = materialUnit

		//记录上次推荐库位信息
		const lastWarehousebincode = page.list.data[0]?.warehouseBinCode

		try {
			const data = await getOneAsync({
				withdrawOrderId: page.input.withdrawOrderId,
				materialCode: page.search.materialCode
			});

			//当已已扫描数等于总数时，显示已完成
			if (data.withdrawOrderAssignedTotal == data.withdrawOrderSumQuantity) {
				page.list.data = []
			}
			else if (data)
				page.list.data[0] = data
			else
				page.list.data = []


			if (page.scan.withdrawOrderTotal >= page.scan.suggestionTotal) {
				page.scan.withdrawOrderTotal = 0
				page.scan.suggestionTotal = 0
			}

			//判断下一个建议库位是否跟当前推荐的库位匹配，如果一致，则跳转到条码，不一致跳转到库位，并清空
			if (data.warehouseBinCode == lastWarehousebincode) {
				// page.input.materialCode = ''
				materialCodeInputFocus()
			}
			else {
				// page.input.warehouseBinCode = ''
				// page.input.materialCode = ''
				warehouseBinCodeInputFocus()
			}

			paging.value.complete(page.list.data)
		}
		catch (error) {
			paging.value.complete(false)
		}
	}
	/**
	 * 库位编码确认
	 */
	async function warehouseBinCodeConfirm() {
		if (page.input?.warehouseBinCode == '') {
			startPlay('errorVoicePDA') //提示音
			uni.showToast({
				icon: 'none',
				title: '库位编码不能为空',
				duration: 2000
			});
			return
		}
		uni.showLoading({
			title: "库位扫描中...",
			mask: true
		})
		try {
			await warehouseBinScanAsync({
				warehouseBinCode: page.input.warehouseBinCode,
				warehouseId: page.info.warehouseId
			})
			materialCodeInputFocus()
			startPlay('successVoicePDA') //提示音
		} catch (err) {
			warehouseBinCodeInputFocus()

			startPlay('errorVoicePDA') //提示音
		} finally {
			uni.hideLoading()
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
			title: "物料扫描中...",
			mask: true
		})
		try {
			const param = {
				withdrawOrderId: page.input.withdrawOrderId,
				warehouseBinCode: page.input.warehouseBinCode,
				materialCode: page.input.materialCode,
				quantity: page.input.quantity,
				withdrawSuggestionId: page.info.withdrawSuggestionId,
				withdrawSuggestionDetailId: page.info.withdrawSuggestionDetailId
			}
			page.search.materialCode = page.input.materialCode
			//条码扫描
			await pdaMaterialScanAsync(param)

			uni.showToast({
				icon: 'success',
				title: "执行成功"
			})
			startPlay('successVoicePDA') //成功提示音

			reloadDetailQuantitySumByMaterialCode()

			reloadDetailQuantitySum()

		} catch (err) {
			const error = err as any

			if (error.errors.WMS11024 || error.errors.WMS11027 || error.errors.WMS11030 || error.errors.WMS11032) {
				warehouseBinCodeInputFocus()
			}
			else if (error.errors.WMS11025 || error.errors.WMS11026 || error.errors.WMS11031) {
				materialCodeInputFocus()
			} else {
				materialCodeInputFocus()
			}

			startPlay('errorVoicePDA') //成功提示音

		} finally {
			uni.hideLoading()
		}
	}

	/**
	 * 物料扫描
	 */
	async function materialConfirm() {
		/**
		 * 更新已扫数量
		 */
		await reloadDetailQuantitySumByMaterialCode()

		if (!await vaild()) {
			return
		}

		if (page.handle.needConfirmQuantity) {
			quantityInputFocus()
			return
		}

		await materialScanHandle()
	}
	/**
	* 数量扫描
	*/
	async function quantityConfirm() {
		if (!await vaild()) {
			return
		}

		await materialScanHandle()
	}

	/**
	 * 重新加载
	 */
	async function reloadList() {

		await paging.value.reload()
	}
	/**
	 * 刷新明细
	 */
	async function reloadDetail() {
		const { warehouseId, withdrawSuggestionId, withdrawSuggestionDetailId } = await getDetailAsync(page.input.withdrawOrderId)

		page.info.warehouseId = warehouseId
		page.info.withdrawSuggestionId = withdrawSuggestionId
		page.info.withdrawSuggestionDetailId = withdrawSuggestionDetailId
	}
	/**
	 * 扫码完成
	 */
	async function confirmAsync() {
		uni.showLoading({
			title: "正在处理中...",
			mask: true
		})
		try {
			//扫码完成
			await withdrawConfirmAsync({ id: page.input.withdrawOrderId })

			uni.showToast({
				icon: 'success',
				title: '执行成功'
			});
			//跳回列表待处理页面
			back({
				redirectUrl: '/pages/delivery/withdraw/main/withdraw-main'
			})

		} catch {

		} finally {
			uni.hideLoading()
		}
	}


	return {
		page, queryList, reloadList, rules, skipAsync,
		materialConfirm, quantityConfirm, confirmAsync, paging,
		reloadDetailQuantitySum, warehouseBinCodeConfirm,
		reloadDetail
	};
}