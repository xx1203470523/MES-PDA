import type { PdaListItemType } from '@/components/pda/pda-list/pda-list-types'

export type PageType = {
	windowInfo ?: UniNamespace.GetWindowInfoResult
	
	element : {
		list : {
			height : number
		}
	}
	
	input : {
		boxCode : string		
		orderCode : string
	}
	
	result : {
		items : PdaListItemType[]		
		data : any[]
	}
	
	timeout : {
		orderCodeInput : any
	}
	
	rules : any
}