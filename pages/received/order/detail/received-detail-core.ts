import { getDetailAsync, detailSumAsync } from '@/api/modules/wms/receive/receive-order'

export function init({ page }) {
	async function reloadDetailInfo() {
		const { receiptOrderCode } = await getDetailAsync(page.input.receiptOrderId)
		page.info.receiptOrderCode = receiptOrderCode
	}

	/**
	 * 重载明细统计数量
	 */
	async function reloadDetailQuantitySum() {
		const { assignedTotal, receiptTotal } = await detailSumAsync(page.input.receiptOrderId)

		page.info.assignedTotal = assignedTotal
		page.info.receiptTotal = receiptTotal
	}

	/**
	 * 持续重载统计数量
	 */
	async function reloadDetailQuantitySumOngoing() {
		await reloadDetailQuantitySum()
		const timeout = setTimeout(async () => {
			await reloadDetailQuantitySumOngoing()
		}, 5000)

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