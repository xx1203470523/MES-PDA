export type PdaListItemType = {
	label : string
	field : string
	valuePreprocessing?(value : any) : void
	color ?: string
}