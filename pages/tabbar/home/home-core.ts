import type { HomeType } from './home-types';

import { reactive } from "vue";

export function init() {
	const page = reactive<HomeType>({
		collapse: {
			items: [
				{
					title: '',
					items: [
						{
							icon: '/static/imgs/home/received.png',
							title: '箱码工单验证',							
							url: '/pages/sfcbox/main/index'
						},
						{
							icon: '/static/imgs/home/listing.png',
							title: '模组码解绑',
							describe: '按当前工序',
							url: '/pages/received/onshelves/main/onshelves-main'
						}
					]
				},
				// {
				// 	title: '出库',
				// 	items: [
				// 		{
				// 			icon: '/static/imgs/home/delisting.png',
				// 			title: '出库下架',
				// 			describe: '按建议下架单',
				// 			url: '/pages/delivery/withdraw/main/withdraw-main'
				// 		},{
				// 			icon: '/static/imgs/home/outstock-confirm.png',
				// 			title: '出库验证',
				// 			describe: '按出库单',
				// 			url: '/pages/delivery/order/main/delivery-main'
				// 		}
				// 	]
				// },
				// {
				// 	title: '库内',
				// 	items: [
				// 		{
				// 			icon: '/static/imgs/home/shift.png',
				// 			title: '直接移位',
				// 			url: '/pages/stock/shift/main/stockshift-main'
				// 		},
				// 		{
				// 			icon: '/static/imgs/home/take.png',
				// 			title: '盘点采集单',
				// 			url: "/pages/stock/take/main/stocktake-main"
				// 		}
				// 	]
				// },
				// {
				// 	title: '统计',
				// 	items: [
				// 		{
				// 			icon: '/static/imgs/home/statistics-material.png',
				// 			title: '物料库存',
				// 			url: "/pages/statistics/material-stock/statistics-material"
				// 		},
				// 		{
				// 			icon: '/static/imgs/home/statistics-bin.png',
				// 			title: '库位库存',
				// 			url: '/pages/statistics/stockbin/main/stockbin-main'
				// 		}
				// 	]
				// }
			],
			open: ['0']
		}
	})

	function navTo(url : string) {
		if (url) {
			uni.navigateTo({
				url
			})
		}
	}

	return {
		page,
		navTo
	}
}