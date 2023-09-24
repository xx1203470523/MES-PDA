import type { PdaListItemType } from '@/components/pda/pda-list/pda-list-types'
import type { ManuSfcBindOutputType } from '@/api/modules/mes/manuSFCBind/types'

export type PageType = {
	windowInfo ?: UniNamespace.GetWindowInfoResult

	formRules : {
		pageRules : any
		switchBindRules : any
	}

	modal : {
		switchBind : {
			show : boolean
			isTasking : boolean
		}
	}

	selected : {
		options : Array<{
			text : string
			value : number
		}>
	}

	input : {
		code : string
		status : number
		newBindCode : string
	}

	chose : {
		detail : ManuSfcBindOutputType
	}

	result : {
		items : PdaListItemType[]
		data : ManuSfcBindOutputType[]
		isBindCount : number
	}
}