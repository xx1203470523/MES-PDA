export type MaterialScanType = {
	putawayOrderId: string,
	putawayOrderDetailAssignmentId: string,
	warehouseBinCode: string,
	materialCode: string,
	quantity: number
}

export type MaterialCheckType = {
	putawayOrderId: string
	putawayOrderDetailAssignmentId: string
	materialCode: string,
	warehouseBinId:string,
	quantity:number
}