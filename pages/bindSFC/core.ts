import type { scanType } from './types'

import { reactive } from "vue";

import { getBindSfcAsync, delBindSfcAsync } from '@/api/modules/mes/manuSfcCirculation/index'

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
			bindSfc: ''
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

		await queryListAsync()

		bindSfcOnFocus()
	}

	/**
	 * 绑定条码扫描
	 */
	async function scanBindSfcAsync() {

		try {

			await queryListAsync()

		} catch {
			uni.showToast({
				title: '查询绑定关系失败！'
			})
		} finally {
			bindSfcOnFocus()
		}

	}

	async function deleteBindSfcAsync(row) {
		try {
			uni.showModal({
				title: "警告",
				content:'确认解绑该条码',
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
		deleteBindSfcAsync
	}
}