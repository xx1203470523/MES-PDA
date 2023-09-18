/**
 * 移位清单
 */
export type StockShiftMaterialDetail={
	/**
	 * 
	 */
	id?:string
	
	/**
	 * 移位单Id
	 */
	stockShiftId?:string
	
	/**
	 * 移动数量
	 */
	shiftQuantity?:number
	
	/**
	 * 物料库位库存总量
	 */
	stockBinMaterialQuanltity?:number
	
	/**
	 * 移出仓库id
	 */
	fromWarehouseBinId?:string
	
	/**
	 * 移入仓库id
	 */
	toWarehouseBinId?:string
	
	/**
	 * 单据日期
	 */
	shiftDate?:string
	
	/**
	 * 制单人
	 */
	createBy?:string
	
	/**
	 * 备注
	 */
	remark?:string
	
	/**
	 * 移入仓库信息
	 */
	toWarehouseBin?:toWarehouseBin
	
	/**
	 * 物料信息
	 */
	material?:material
	
	/**
	 * 移出仓库信息
	 */
	fromWarehouseBin?:fromWarehouseBin
}

/**
 * 移出仓库
 */
export type fromWarehouseBin={
	/**
	 * 
	 */
	id?:string
	
	/**
	 * 仓库编码
	 */
	binCode?:string
	
	/**
	 * 仓库名称
	 */
	binName?:string
	
	/**
	 * 移出仓库
	 */
	fromBinCode?:string
}

/**
 * 移入仓库
 */
export type toWarehouseBin={
	/**
	 * 
	 */
	id?:string
	
	/**
	 * 仓库编码
	 */
	binCode?:string
	
	/**
	 * 仓库名称
	 */
	binName?:string
	
	/**
	 * 移入仓库
	 */
	toBinCode?:string
}

/**
 * 物料
 */
export type material={
	/**
	 * 
	 */
	id?:string
	
	/**
	 * 物料编码
	 */
	materialCode?:string
	
	/**
	 * 物料名称
	 */
	materialName?:string
	
	/**
     * 物料单位 
	 */
	materialUnit?:string
}