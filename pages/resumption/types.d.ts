import type { PdaListItemType } from '@/components/pda/pda-list/pda-list-types'
import type { ManuSfcBindOutputType } from '@/api/modules/mes/manuSFCBind/types'

export type PageType = {
	windowInfo ?: UniNamespace.GetWindowInfoResult

	formRules : {
		pageRules : any
	}

	selected : {
		options : Array<{
			text : string
			value : string
		}>
	}

	/**
	 * 输入
	 */
	input : {
		/**
		 * 条码
		 */
		sFC : string
		/**
		 * NG位置
		 */
		nGLocationId : string
		/**
		 * 复投位置
		 */
		repeatLocationId : string
	}

	/**
	 * 接口返回结果
	 */
	result : {
		items : PdaListItemType[]
		data : ManuSfcBindOutputType[]
		ngLocationId : string
	}
}