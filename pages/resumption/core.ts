import type { PageType } from './types'

import { reactive } from "vue";

import { listAsync, repeatManuSFCAsync } from '@/api/modules/mes/manuSFCBind/index'
import { listAsync as procProcedureListAsync } from '@/api/modules/mes/procProcedure/index'

import { debounce } from '@/utils/fn-utils'

export function init({
	formVaild,
	codeInputFocus
}) {
	const page = reactive<PageType>({
		formRules: {
			pageRules: {
				sFC: {
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
		timeout: {},
		input: {
			sFC: '',
			nGLocationId: '',
			repeatLocationId: ''
		},
		result: {
			items: [
				{
					label: '绑定条码',
					field: 'manuSfcCirculationEntity.sfc'
				}
			],
			data: [],
			ngLocationId: ''
		}
	});

	/**
	 * 页面清理
	 * @return 
	 */
	function clear() {
		page.result.data = []

		if (page.timeout.codeInput) {
			clearTimeout(page.timeout.codeInput)
		}
	}

	/**
	 * 条码确认
	 */
	async function codeConfirmAsync() {
		try {
			if(!page.input.sFC){
				throw '条码为空'
			}
			
			const { data, nGLocationId } = await listAsync({
				sFC: page.input.sFC
			})


			page.result.data = data
			page.input.nGLocationId = nGLocationId
		} catch {
			clear()
		}

		codeInputFocus()
	}

	/**
	 * 条码输入
	 * @return 
	 */
	async function codeInputAsync() {
		page.timeout.codeInput = debounce(codeConfirmAsync, page.timeout.codeInput, 300)()
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
			const data = await procProcedureListAsync()

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
		codeInputAsync,
		repeatConfirmAsync
	}
}