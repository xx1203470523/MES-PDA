import { reactive } from "vue";
import type { StockShiftDetail } from './stockshift-main-types.d';
import type { PdaListItem } from '@/components/pda/pda-list/pda-list-types'
import {  to } from '@/utils/route-utils'
//import { useCommonSettingsStore } from '@/store/commonsettings'

export function init() {
	// const CommonSettingsStore=useCommonSettingsStore()
	// console.log(CommonSettingsStore.getCommonSettingsAsync()) 

	const page = reactive<{
		scroll : {
			top : 0,
			oldTop : 0
		},
		windowInfo ?: UniNamespace.GetWindowInfoResult,
		operateON : {
			id : ''
		},
		list : {
			items : PdaListItem[],
			data : StockShiftDetail[]
		},
		search : {
			shiftCode : string
		},
		info : {
			billTotalQuantity : number,
			materialTotalQuantity : number
		},
		isShow:{
			addBtnIsShow:boolean
		}
	}>({
		scroll: {
			top: 0,
			oldTop: 0
		},
		operateON: {
			id: ''
		},
		list: {
			items: [
				{
					label: '仓库',
					field: 'warehouseName'
				},
				{
					label: '单据日期',
					field: 'createdOn'
				},
				{
					label: '制单人',
					field: 'createdBy'
				},
				{
					label: '备注',
					field: 'remark'
				}
			],
			data: [

			]
		},
		search: {
			shiftCode: ''
		},
		info: {
			billTotalQuantity: 0,
			materialTotalQuantity: 0
		},
		isShow:{
			addBtnIsShow:false
		}
	})

	// function upper(e) {
	// 	console.log(e)
	// }

	// function lower(e) {
	// 	console.log(e)
	// }

	//滚动时触发事件
	function scroll(e) {
		page.scroll.top = e.detail.scrollTop
	}

	function navToDetail(item : any) {
		let url = '/pages/stock/shift/detail/stockshift-detail-add?id=' +
			item.id + '&shiftCode=' + item.shiftCode +
			'&wareHouseId=' + item.warehouseId + '&shiftType=' + item.shiftType+'&shiftTotal='+item.shiftTotal
			to(url)
		// if (url) {
		// 	to(url)
		// 	// uni.navigateTo({
		// 	// 	url,
		// 	// 	success: () => {

		// 	// 	}
		// 	// });
		// }
	}

	function nvoToAdd() {
		let url = '/pages/stock/shift/operate/stockshift-add'
		if (url) {
			uni.navigateTo({
				url,
				success: () => {

				}
			});
		}
	}

	//列表点击事件
	function itemClick(data : any) {
		console.log(data)
		navToDetail(data)
	}

	return {
		page,
		//upper,
		//lower,
		scroll,
		nvoToAdd,
		itemClick
	};
}