import type { PdaListItemType } from '@/components/pda/pda-list/pda-list-types'
import type { ManuSfcBindOutputType } from '@/api/modules/mes/manuSFCBind/types'

import { NgStateEnum } from '@/api/modules/mes/manuSFCBind/enum'

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
			disable?: boolean
		}>
	}

	input : {
		code : string
		status : NgStateEnum
		newBindCode : string
	}

	chose : {
		detail : {
			manuSfcCirculationEntity: ManuSfcBindOutputType
			ngState: number
		}
	}
	
	timeout : {
		codeInput?: number
	}

	result : {
		items : PdaListItemType[]
		data : ManuSfcBindOutputType[]
		status : NgStateEnum
		isBindCount : number
	}
}