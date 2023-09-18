import { reactive } from "vue";
import type { PdaListItem } from '@/components/pda/pda-list/pda-list-types'
import type { StockShiftMaterialDetail } from '@/pages/stock/shift/details/stockshift-details-types.d';

export function init() {
	const page = reactive<{
			scroll : {
				top : 0,
				oldTop : 0
			},
			list : {
				items : PdaListItem[],
				data : StockShiftMaterialDetail[]
			},
			info : {
				stockBinMaterialQuantity : number,
				stockShiftMaterialQuantity : number
			},
			firstLoaded : boolean,
		}>({
			scroll: {
				top: 0,
				oldTop: 0
			},
			list: {
				items: [
					{
						label: '物料名称',
						field: 'material.materialName'
					},
					{
						label: '单位',
						field: 'material.materialUnit'
					},
					{
						label: '移出库位',
						field: 'fromWarehouseBin.fromBinCode'
					},
					{
						label: '移入库位',
						field: 'toWarehouseBin.toBinCode'
					}
				],
				data: [
		
				]
			},
			info: {
				//materialTotalQuantity:0
				stockBinMaterialQuantity: 0,
				stockShiftMaterialQuantity: 0
			},
			firstLoaded : false,
		})
		
		
		return {
			page
		};
}

