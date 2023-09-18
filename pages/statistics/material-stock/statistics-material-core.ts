import { reactive } from "vue";
import type { StockMaterialDetail } from './statistics-material-types';
import type { pdaItem } from '@/components/pda/pda-item-checkbox/pda-item-checkbox-types'

export function init() {
	const page = reactive<{
		windowInfo ?: UniNamespace.GetWindowInfoResult,
		operateON : {
			selectIds ?: any[],
			bindCode ?: string
		},
		params : {
			pageIndex : number | 1,
			pageSize : number | 5,
			materialCode : string,
			selectId ?: string,
			hasStockQuantity : boolean
		},
		list : {
			stockQuantityTotal : number | 0,
			availableQuantity : number | 0,
			items : pdaItem[],
			data : StockMaterialDetail[]
		}
	}>({
		operateON: {
			selectIds: [],
			bindCode: ''
		},
		params: {
			pageIndex: 1,
			pageSize: 5,
			materialCode: '',
			selectId: '',
			hasStockQuantity: true
		},
		list: {
			stockQuantityTotal: 0,
			availableQuantity: 0,
			items: [
				{
					label: '库位',
					field: 'warehouseBin.binCode'
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
					label: '库存数',
					field: 'stockQuantity'
				},
				{
					label: '可用数',
					field: 'availableQuantity'
				},
				{
					label: '锁定数',
					field: 'occupyQuantity'
				},
				{
					label: '单位',
					field: 'material.materialUnit'
				}
			],
			data: []
		}
	})

	/**
	 * 复选按钮勾选和取消勾选事件
	 */
	function changeCheckbox(item : any, e : any) {
		if (e.detail.value.length > 0) {
			page.operateON.selectIds.push(item.id)
		} else {
			page.operateON.selectIds = page.operateON.selectIds.filter(x => x != item.id)
		}
	}

	return {
		page,
		changeCheckbox
	};
}