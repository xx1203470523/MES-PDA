export type WithdrawPageData = {
	/**
	 * id
	 */
	id: string,
	
	/**
	 * 物料编码
	 */
	materialCode : string,
	
	/**
	 * 物料名称
	 */
	materialName : string,
	
	/**
	 * 单位
	 */
	materialUnit : string
	
	/**
	 * 建议库位编码
	 */
	warehouseBinCode : string
	
	/**
	 * 下架数量（物料的下架建议单数量）
	 */
	withdrawSuggestionQuantity : number
	
	/**
	 * 下架数量（某物料扫描数）
	 */
	withdrawOrderQuantity : number
	
	/**
	 * 通知数
	 */
	deliveryNoticeQuantity : number
	
	/**
	 * 实际库位编码
	 */
	warehouseBinCodeReal : string
	
	// /**
	//  * 下架单号
	//  */
	// withdrawOrderCode : string
	
	// /**
	//  * 下架建议单号
	//  */
	// withdrawSuggestionCode : string
}


export type WithdrawDetail = {
	withdrawOrder: string;
	noticeQuantity: number|'';
	withdrawQuantity?: number|'';
	waitWithdrawQuantity?: number|'';
}
export type WithdrawItemDetail = {
	materialCode: string;
	materialName: string;
	noticeQuantity?: number|'';
	materialUnit: string;
	binCode: string;
	fromBinCode: string;
}

export type WithdrawType = {
	base: WithdrawDetail;
	items: Array<WithdrawItemDetail>;
}

export type WithdrawQuery = {
	code: string; //条码或同步号、条码编号
}