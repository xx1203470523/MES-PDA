import { reactive } from "vue";
import { to } from '@/utils/route-utils'
import type { PdaListItem } from '@/components/pda/pda-list/pda-list-types'
import type { TakeData } from './stocktake-main-types'

export function init() {
	const page = reactive<{
		scroll : {
			top : 0,
			oldTop : 0
		},
		windowInfo ?: UniNamespace.GetWindowInfoResult,
		search : {
			key : string
		},
		list : {
			items : PdaListItem[],
			data : TakeData[]
		}
	}>({
		scroll: {
			top: 0,
			oldTop: 0
		},
		search: {
			key: ''
		},
		list: {
			items: [
				{
					label: '盘点方式',
					field: 'checkMethod'
				},
				{
					label: '账面库存',
					field: 'bookTotal'
				},
				{
					label: '实盘数量',
					field: 'takeTotal'
				},
				{
					label: '盘点状态',
					field: 'countingStatus'
				},
				{
					label: '备注',
					field: 'remark'
				}
			],
			data: []
		}
	});
 	 
	/**取消查询 */
	function cancel(e : any) {
		console.log(e)
	}

	function scroll(e : any) {
		page.scroll.oldTop = e.detail.scrollTop
	}

	/**
	 * 列表项点击
	 */
	function itemClick(item : any) {
		to('/pages/stock/take/operate/stocktake-operate?id=' + item.id + '&code=' + item.takeCode)
	}

	return {
		page,
		itemClick,
		cancel, 
		scroll
	};
}