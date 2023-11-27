import type { scanType } from './types'

import { reactive } from "vue";


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
					label: '绑定条码',
					field: 'bindSFC'
				},
				{
					label: '日期',
					field: 'UpdatedOn'
				},
				{
					label: '操作',
					field: 'overScale'
				}
			],
			data: []
		}
	})
	
	/**
	 * 查询绑定关系
	 */
	async function queryListAsync(){
		
		
		
	}
	
	/**
	 * 条码扫描
	 */
	async function scanSfcAsync(){
		
		try{
			await queryListAsync()
			
			bindSfcOnFocus()
		}
		catch(e){
			uni.showModal({
				title:'查询绑定关系失败！'
			})
		}
		
	}
	
	/**
	 * 绑定条码扫描
	 */
	async function scanBindSfcAsync(){
		
		try{
			
			await queryListAsync()
			
		}catch{
			uni.showModal({
				title:'查询绑定关系失败！'
			})
		}finally{
			bindSfcOnFocus()
		}
		
	}


	return {
		page,
		queryListAsync,
		scanSfcAsync,
		scanBindSfcAsync
	}
}