import { reactive } from "vue";

import { to } from '@/utils/route-utils'

import type { PdaListItem } from '@/components/pda/pda-list/pda-list-types'

import type { DeliveryData } from './delivery-main-types'


export function init() {
	const page = reactive<{
		windowInfo ?: UniNamespace.GetWindowInfoResult,
		search : {
			key : string,
			focus : boolean
		},
		info : {
			waitToDeliveryOrderTotal : number,
			waitToDeliveryMaterialTotal : number
		},
		list : {
			items : PdaListItem[],
			data : DeliveryData[]
		} 
	}>({
		search: {
			key: '',
			focus: true
		},
		info: {
			waitToDeliveryOrderTotal: 0,
			waitToDeliveryMaterialTotal: 0
		},
		list: {
			items: [
				{
					label: '仓库',
					field: 'warehouseName'
				},
				{
					label: '通知单号',
					field: 'deliveryNotice.deliveryNoticeCode'
				},
				{
					label: '同步单号',
					field: 'deliveryNotice.syncCode'
				},
				{
					label: '往来单位',
					field: 'deliveryNotice.contact.contactName'
				},
				{
					label: '单据日期',
					field: 'deliveryNotice.createdOn'
				},
				{
					label: '备注',
					field: 'deliveryNotice.remark'
				}
			],
			data: []
		} 
	});

	/**
	 * 列表项点击
	 */
	function itemClick(item : any) {
		to('/pages/delivery/order/operate/delivery-operate?id=' + item.id)
	}

	return {
		page,
		itemClick
	}
}