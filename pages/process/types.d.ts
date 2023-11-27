
export type processType = {
	windowInfo ?: UniNamespace.GetWindowInfoResult
	
	element : {
		list : {
			height : number
		}
	},
	info : {
		procedureName:string,
		processStatus:string,
		procedureListData:any,
		processStatusListData:any,
	},
	input : {
		sfc : string		
		procedureId : string ,
		processStatus : string
	}
	
}