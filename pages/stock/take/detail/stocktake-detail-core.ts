import { getDetailList, getSum } from '@/api/modules/wms/stock-take/stock-take-main'

export function init({ page }) {  
	 
	async function reloadDetailInfo() {
		const { data } = await getDetailList(page.info.id) 
	}

	/**
	 * 重载明细统计数量
	 */
	async function reloadDetailQuantitySum() { 
		const { takeTotal, bookTotal } = await getSum(page.info.id)

		page.info.takeTotal = takeTotal
		page.info.bookTotal = bookTotal
	}
	
	/**
	 * 持续重载统计数量
	 */
	async function reloadDetailQuantitySumOngoing() {
		await reloadDetailQuantitySum()
		const timeout = setTimeout(async () => {
			await reloadDetailQuantitySumOngoing()
		}, 3000)

		page.setTimeout.push(timeout)
	}
	
	/**
	 * 页签变更
	 */
	function tabsChange(index : number) {
		page.tab.current = index
	}
	
	/**
	 * swiper滑动
	 */
	function swiperChange(e : any) {
		// #ifndef APP-PLUS || H5 || MP-WEIXIN || MP-QQ
		let current = e.target.current || e.detail.current;
		tabsChange(current);
		// #endif
	}
	
	/**
	 * 释放页面
	 */
	function unload() {
		page.setTimeout.forEach(m => {
			clearTimeout(m)
		})
	}

	return {
		page,
		reloadDetailInfo,
		tabsChange,
		swiperChange,
		reloadDetailQuantitySumOngoing,
		unload
	} 
}