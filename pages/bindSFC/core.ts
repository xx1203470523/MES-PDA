import type { scanType } from './types'

import { reactive } from "vue";

import { getBindSfcAsync, delBindSfcAsync, bindSfcAsync as bindSfcApiAsync } from '@/api/modules/mes/manuSfcCirculation/index'
import { getByCodesAsync } from '@/api/modules/mes/procProcedure/index'

export function init({
	bindSfcOnFocus
}) {
	const page = reactive<scanType>({
		element: {
			list: {
				height: 0
			}
		},
		input: {
			sfc: '',
			bindSfc: '',
			procedureId: 0,
			procedureList: []
		},
		dataList: {
			items: [
				{
					label: '条码',
					field: 'sfc'
				},
				{
					label: '绑定条码',
					field: 'bindSFC'
				},
				{
					label: '日期',
					field: 'updatedOn'
				},
				{
					label: '操作人',
					field: 'updatedBy'
				}
			],
			data: [
			]
		}
	})

	/**
	 * 查询绑定关系
	 */
	async function queryListAsync() {
		page.dataList.data = await getBindSfcAsync(page.input.sfc)
	}

	/**
	 * 条码扫描
	 */
	async function scanSfcAsync() {
		try {
			await queryListAsync()

			bindSfcOnFocus()
		} catch {

		}
	}

	/**
	 * 绑定条码扫描
	 */
	async function scanBindSfcAsync() {

		//校验数据
		let msg = ''
		if (!page.input.sfc) msg = '条码不能为空！'
		else if (!page.input.bindSfc) msg = '绑定条码不能为空！'
		else if (!page.input.procedureId) msg = '请选择工序！'

		if (msg != '') {
			uni.showToast({
				title: msg
			})
			return;
		}

		const bindData = {
			sfc: page.input.sfc,
			bindSfc: page.input.bindSfc,
			procedureId: page.input.procedureId
		}

		try {

			await bindSfcApiAsync(bindData)

			uni.showToast({
				title: '绑定成功！',
				success: async () => {
					await queryListAsync()
				}
			})

		} catch (e) {
			console.log(e.message)
		} finally {
			await queryListAsync()
			
			bindSfcOnFocus()
		}

	}

	/**
	 * 接触绑定关系
	 * @param {any} row 
	 * @return 
	 */
	async function deleteBindSfcAsync(row : any) {
		try {
			uni.showModal({
				title: "警告",
				content: '确认解绑该条码',
				success: async ({ confirm }) => {
					if (confirm) {
						await delBindSfcAsync(row.id)

						uni.showToast({
							title: '解绑成功！'
						})

						await queryListAsync()
					}
				}
			})

		} catch {
			uni.showToast({
				title: '解绑失败，请重试！'
			})
		}
	}

	return {
		page,
		queryListAsync,
		scanSfcAsync,
		scanBindSfcAsync,
		deleteBindSfcAsync,
		getByCodesAsync
	}
}