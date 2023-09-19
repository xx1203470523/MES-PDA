import { reactive } from "vue";

import { to } from '@/utils/route-utils'

// import type { PdaListItem } from '@/components/pda/pda-list/pda-list-types'


export function init() {
	const page = reactive<{
		windowInfo ?: UniNamespace.GetWindowInfoResult,
		search : {
			key : string,
			focus : boolean
		},
		info : {
			waitToReceivedOrderTotal : number,
			waitToReceivedMaterialTotal : number
		},
		list : {
			// items : PdaListItem[],
			// data : ReceiptData[]
		}
		// timeout : {
		// 	searchTime ?: any
		// }
	}>({
		search: {
			key: '',
			focus: true
		},
		info: {
			waitToReceivedOrderTotal: 0,
			waitToReceivedMaterialTotal: 0
		},
		list: {
			items: [
				// {
				// 	label: '仓库',
				// 	field: 'warehouseName'
				// },
				// {
				// 	label: '通知单号',
				// 	field: 'receiptNotice.receiptNoticeCode'
				// },
				// {
				// 	label: '同步单号',
				// 	field: 'receiptNotice.receiptNoticeSyncCode'
				// },
				// {
				// 	label: '备注',
				// 	field: 'remark'
				// }
			],
			data: []
		}
		// timeout: {
		// 	searchTime: null
		// }
	});

	/**
	 * 列表项点击
	 */
	function itemClick(item : any) {
		to('/pages/received/order/operate/received-operate?id=' + item.id)
	}

	return {
		page,
		itemClick
	}
}