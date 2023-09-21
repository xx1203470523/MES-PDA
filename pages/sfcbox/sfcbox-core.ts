import { reactive } from "vue";

import { sfcboxValidateScanAsync} from '@/api/modules/sfcbox/sfc-box'

export const rules = {
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
		list : {}
	}>({
		input: {
			boxCode: '',
			orderCode: ''
		},
		list: {}
	});
	
	/**
	 * 箱码确认
	 */
	function boxCodeConfirm(){
		orderCodeInputFocus()
	}
	
	/**
	 * 工单确认
	 */
	function orderCodeConfirm(){
		boxCodeInputFocus()
	}
	
	/**
	 * 点击验证按钮
	 */
	async function verifyClick() {
		const { msg } = await sfcboxValidateScanAsync({
						BoxCode: page.input.boxCode,
						WorkOrderCode: page.input.orderCode
					})
					
		uni.showModal({
			title: msg,
			showCancel: false
		})
		
		// uni.showToast({
		// 	title: '请求异常',
		// 	icon: 'none'
		// })
	}

	return {
		page,
		boxCodeConfirm,
		orderCodeConfirm,
		verifyClick
	}
}