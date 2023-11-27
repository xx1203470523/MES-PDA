import type { processType } from './types'

import { reactive } from "vue";

import {  getSfcProcessInfoAsync } from '@/api/modules/mes/process/index'
import { listAsync as procProcedureListAsync } from '@/api/modules/mes/procProcedure/index'

export function init({}) {
	const page = reactive<processType>({
		element: {
			list: {
				height: 0
			}
		},
		info:{
			procedureName:'无',
			procedureListData:[],
			processStatus:'无',
			processStatusListData:[
				{ text:'排队中',value:'排队中' },
				{ text:'活动中',value:'活动中' },
				{ text:'已完成',value:'已完成' }
			],
		},
		input: {
			sfc: '',
			procedureId:'',
			processStatus:''
		}
	})
	
	async function scanSfcAsync(){
		const query = {
			sfc : page.input.sfc
		}
		
		//获取在制信息
		const processInfo = await getSfcProcessInfoAsync(query)
		
		page.info.procedureName = processInfo.procedureName
		page.info.processStatus = processInfo.processName
		
	}
	
	async function loadProcedureListAsync(){
		const list = await procProcedureListAsync()
		
		
	}


	return {
		page,
		scanSfcAsync,
		loadProcedureListAsync
	}
}