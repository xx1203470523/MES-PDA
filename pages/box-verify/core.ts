import type { PdaListItemType } from '@/components/pda/pda-list/pda-list-types'

import { reactive } from "vue";

import clipboard from '@/components/common/tui-clipboard/tui-clipboard.js'
import { sfcboxValidateScanAsync } from '@/api/modules/mes/inteSFCBox/index'
import { sfcboxFuzzyPageAsync } from '@/api/modules/mes/planWorkOrder/index'

import { debounce } from '@/utils/fn-utils'

export function init({
	boxCodeInputFocus,
	orderCodeInputFocus
}) {
	const page = reactive<{
		windowInfo ?: UniNamespace.GetWindowInfoResult,
		input : {
			boxCode : string
			orderCode : string
		},
		result : {
			items : PdaListItemType[],
			data : any[]
		},
		timeout : {
			orderCodeInput : any
		},
		rules : any
	}>({
		input: {
			boxCode: '',
			orderCode: ''
		},
		result: {
			items: [
				{
					label: '工单号',
					field: 'orderCode'
				},
				{
					label: '数量',
					field: 'qty'
				},
				{
					label: '规模过大',
					field: 'overScale'
				},
				{
					label: '状态',
					field: 'status'
				}
			],
			data: []
		},
		timeout: {
			orderCodeInput: null
		},
		rules: {
			boxCode: {
				rules: [
					{
						required: true,
						errorMessage: '请输入箱码',
					}
				]
			},
			orderCode: {
				rules: [
					{
						required: true,
						errorMessage: '请输入工单号',
					}
				]
			}
		}
	});

	/**
	 * 分页查询处理器
	 */
	async function getPageHandlerAsync() {
		if (!page.input.orderCode) {
			page.result.data = []

			return
		}

		if (page.input.orderCode === '') {
			page.result.data = []

			return
		}

		try {
			page.result.data = await sfcboxFuzzyPageAsync(page.input.orderCode)
		} catch { }
	}

	/**
	 * 分页查询
	 */
	function getPageAsync() {
		page.timeout.orderCodeInput = debounce(getPageHandlerAsync, page.timeout.orderCodeInput, 300)()
	}

	/**
	 * 点击验证按钮
	 */
	async function verifyClickAsync() {
		try {
			const { msg } = await sfcboxValidateScanAsync({
				BoxCode: page.input.boxCode,
				WorkOrderCode: page.input.orderCode
			})

			uni.showModal({
				title: msg,
				showCancel: false,
				success() {
					boxCodeInputFocus()
				}
			})
		} catch {
			orderCodeInputFocus()
		}
	}

	/**
	 * 箱码确认
	 */
	function boxCodeConfirm() {
		orderCodeInputFocus()
	}

	/**
	 * 工单确认
	 */
	async function orderCodeConfirm() {
		orderCodeInputFocus()
	}

	/**
	 * 工单号复制
	 * @param {Object} data
	 */
	function orderCodeCopy(data : any) {
		page.input.orderCode = data.orderCode

		clipboard.getClipboardData(page.input.orderCode, () => {
			uni.showToast({
				title: '工单号复制成功',
				icon: 'none'
			})
		})
	}

	return {
		page,
		getPageAsync,
		boxCodeConfirm,
		orderCodeConfirm,
		verifyClickAsync,
		orderCodeCopy
	}
}