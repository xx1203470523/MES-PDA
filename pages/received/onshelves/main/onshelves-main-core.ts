import { reactive } from "vue";

import { to } from '@/utils/route-utils'

import type { PdaListItem } from '@/components/pda/pda-list/pda-list-types'
import type { ReceiptData } from './onshelves-main-types'

export function init() {
	const page = reactive<{
		windowInfo ?: UniNamespace.GetWindowInfoResult,
		search : {
			key : string
		},
		info : {
			waitToOnshelvesOrderTotal : number,
			waitToOnshelvesMaterialTotal : number
		},
		list : {
			items : PdaListItem[],
			data : ReceiptData[]
		}
	}>({
		search: {
			key: ''
		},
		info: {
			waitToOnshelvesOrderTotal: 0,
			waitToOnshelvesMaterialTotal: 0
		},
		list: {
			items: [
				{
					label: '仓库',
					field: 'warehouse.name'
				},
				{
					label: '同步单号',
					field: 'receiptNotice.receiptNoticeSyncCode'
				},
				{
					label: '入库通知单号',
					field: 'receiptNotice.receiptNoticeCode'
				},
				{
					label: '入库单号',
					field: 'receiptOrder.receiptOrderCode'
				},
				{
					label: '上架建议单号',
					field: 'suggestion.putawaySuggestionCode'
				},
				{
					label: '往来单位',
					field: 'contact.contactName'
				},
				{
					label: '单据日期',
					field: 'suggestion.createdOn'
				},
				{
					label: '备注',
					field: 'suggestion.remark'
				}
			],
			data: []
		}
	});

	/**
	 * 列表项点击
	 */
	function itemClick(item : any) {
		to('/pages/received/onshelves/operate/onshelves-operate?id=' + item.id)
	}

	return {
		page,
		itemClick
	}
}