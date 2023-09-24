import type { PageType } from './types'

import { reactive } from "vue";

import { listAsync, repeatManuSFCAsync } from '@/api/modules/mes/manuSFCBind/index'
import { pageAsync } from '@/api/modules/mes/procProcedure/index'


export function init({
	formVaild,
	codeInputFocus
}) {
	const page = reactive<PageType>({
		formRules: {
			pageRules: {
				code: {
					rules: [
						{
							required: true,
							errorMessage: '请扫描编码',
						}
					]
				},
				nGLocationId: {
					rules: [
						{
							required: true,
							errorMessage: '请选择当前位置',
						}
					]
				},
				repeatLocationId: {
					rules: [
						{
							required: true,
							errorMessage: '请选择复投位置',
						}
					]
				}
			}
		},
		selected: {
			options: []
		},
		input: {
			code: '',
			nGLocationId: '',
			repeatLocationId: ''
		},
		result: {
			items: [
				{
					label: '绑定条码',
					field: 'bindSFC'
				}
			],
			data: [],
			ngLocationId: ''
		}
	});

	/**
	 * 条码确认
	 */
	async function codeConfirmAsync() {
		if (await formVaild()) {
			try {
				const { data, nGLocationId } = await listAsync({
					sFC: page.input.code
				})


				page.result.data = data
				page.input.nGLocationId = nGLocationId
			} catch { }
		}
		codeInputFocus()
	}

	/**
	 * 复投确认
	 */
	async function repeatConfirmAsync() {
		if (await formVaild()) {
			uni.showModal({
				title: '操作警告',
				content: '确认复投后将解除目前条码绑定关系，是否“确认”？',
				async success({ confirm }) {
					uni.showLoading({
						title: '复投中...'
					})
					if (confirm) {
						try {
							await repeatManuSFCAsync(page.input)
						} catch { }
					}
					uni.hideLoading()

					codeInputFocus()
				}
			})
		}
		codeInputFocus()
	}

	/**
	 * 加载所有工序
	 */
	async function reloadProcProcedureSelected() {
		try {
			const { data } = await pageAsync({
				pageIndex: 1,
				pageSize: 10000
			})

			page.selected.options = data.map(m => {
				return {
					text: m.name,
					value: m.id
				}
			})
		} catch { }
	}

	/**
	 * 初始化
	 */
	async function initAsync() {
		page.windowInfo = uni.getWindowInfo()

		uni.showLoading({
			title: '页面初始化中...',
			mask: true
		})

		await reloadProcProcedureSelected()

		uni.hideLoading()
	}

	return {
		page,
		initAsync,
		codeConfirmAsync,
		repeatConfirmAsync
	}
}