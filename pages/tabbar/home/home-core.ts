import type { HomeType } from './home-types';

import { reactive } from "vue";

export function init() {
	const page = reactive<HomeType>({
		collapse: {
			items: [
				{
					title: '工作列表',
					items: [
						{
							icon: '/static/imgs/home/received.png',
							title: '箱码工单验证',							
							url: '/pages/sfcbox/sfcbox'
						},
						{
							icon: '/static/imgs/home/listing.png',
							title: '模组码解绑',
							describe: '按当前工序',
							url: ''
						}
					]
				}
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