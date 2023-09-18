export type StockMaterialDetail = {
	/**
	 * 库位库存id
	 */
	id ?: string

	/**
	 * 库位Id
	 */
	warehouseBinId ?: string
	
	/**
	 * 库位
	 */
	warehouseBin ?: {
		/**
		 * 库位编码
		 */
		binCode ?: string
	}
	
	/**
	 * 物料Id
	 */
	materialId ?: string

	material ?: {
		/**
		* 物料编码
		*/
		materialCode ?: string

		/**
		 * 物料名称
		 */
		materialName ?: string
		/**
		 * 单位
		 */
		materialUnit ?: string
	}

	/**
	 * 库存数
	 */
	stockQuantityTotal ?: number

	/**
	 * 可用数
	 */
	availableQuantity ?: number

	/**
	 * 锁定数
	 */
	occupyQuantity ?: number
	
	/**
	 * 移动数量
	 */
	moveQuantity ?: number
}