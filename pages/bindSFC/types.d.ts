import type { PdaListItemType } from '@/components/pda/pda-list/pda-list-types'

export type scanType = {
	windowInfo ?: UniNamespace.GetWindowInfoResult
	
	element : {
		list : {
			height : number
		}
	}
	
	input : {
		sfc : string		
		bindSfc : string,
		procedureId: number,
		procedureList: any
	}
	
	dataList : {
		items : PdaListItemType[]		
		data : any[]
	}
}