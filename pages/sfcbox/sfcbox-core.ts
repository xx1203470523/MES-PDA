import { reactive } from "vue";

export const rules = {}

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
	function verifyClick() {
		uni.showModal({
			title: '消息提示',
			showCancel: false
		})
	}

	return {
		page,
		boxCodeConfirm,
		orderCodeConfirm,
		verifyClick
	}
}