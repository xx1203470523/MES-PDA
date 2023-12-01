import type { processType } from './types'

import { reactive } from "vue";

import { getSfcProcessInfoAsync, updateProduceStatusAsync } from '@/api/modules/mes/process/index'
import { getAllAsync } from '@/api/modules/mes/procProcedure/index'

export function init({ }) {
	const page = reactive<processType>({
		element: {
			list: {
				height: 0
			}
		},
		info: {
			procedureName: '无',
			procedureListData: [],
			processStatus: '无',
			processStatusListData: [
				{ text: '排队中', value: 1 },
				{ text: '活动中', value: 2 },
				{ text: '已完成', value: 3 }
			],
		},
		input: {
			sfc: '',
			procedureId: '',
			processStatus: ''
		}
	})

	async function scanSfcAsync() {
		const query = {
			sfc: page.input.sfc
		}

		if (page.input.sfc) {
			//获取在制信息
			const processInfo = await getSfcProcessInfoAsync(query)

			if (processInfo.processStatus == '1') processInfo.processStatus = '排队中'
			if (processInfo.processStatus == '2') processInfo.processStatus = '活动中'

			page.info.procedureName = processInfo.procedureName
			page.info.processStatus = processInfo.processStatus

			await loadProcedureListAsync()
		}

	}

	async function loadProcedureListAsync() {
		const list = await getAllAsync()

		page.info.procedureListData = list;

	}

	async function confirmAsync() {
		let msg = ''
		if (!page.input.sfc) msg = '条码不能为空!'
		else if (!page.input.processStatus) msg = '请选择状态!'
		else if (!page.input.procedureId) msg = '请选择工序!'

		if (msg == '') {
			const update = {
				sfc: page.input.sfc,
				procedureId: page.input.procedureId,
				procduceStatus: page.input.processStatus
			}

			try {
				await updateProduceStatusAsync(update)

				await scanSfcAsync()

				uni.showToast({
					icon: 'success',
					title: '修改成功！'
				})

				page.input.procedureId = undefined
				page.input.processStatus = undefined

			} catch (e) {
				uni.showToast({
					icon: 'fail',
					title: '修改失败！'
				})
			}
		}
		else {
			uni.showModal({
				icon: 'fail',
				title: msg
			})
		}


	}


	return {
		page,
		scanSfcAsync,
		loadProcedureListAsync,
		getAllAsync,
		confirmAsync
	}
}