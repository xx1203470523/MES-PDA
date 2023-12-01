import type { HomeType } from './types';

import { reactive } from "vue";

export function init() {
	const page = reactive<HomeType>({
		collapse: {
			items: [
				{
					title: '工作列表',
					items: [
						{
							icon: '/static/imgs/home/verify.png',
							title: '箱码验证',
							url: '/pages/box-verify/index'
						},
						{
							icon: '/static/imgs/home/repair.png',
							title: '维修',
							url: '/pages/repair/index'
						},
						{
							icon: '/static/imgs/home/resumption.png',
							title: '复投',
							url: '/pages/resumption/index'
						},
						{
							icon: '/static/imgs/home/process.png',
							title: '在制',
							url: '/pages/process/index'
						},
						{
							icon: '/static/imgs/home/bind.png',
							title: '绑定解绑',
							url: '/pages/bindSFC/index'
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