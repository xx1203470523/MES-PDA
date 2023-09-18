import { reactive } from "vue";
import type { baseFormData } from './stockshift-add-types.d';

export function init() {

	const baseFormData = reactive<{
		formData : baseFormData
		selectData_shiftType : any[],
		selectData_wareHouses : any[],
		rules : any
	}>({
		formData: {
			wareHouseId: '',
			shiftType: '',
			remark: ''
		},
		selectData_shiftType: [
			{ value: '1', text: "直接移位" }
		],
		selectData_wareHouses: [
		],
		rules: {
			wareHouseId: {
				rules: [{
					required: true,
					errorMessage: '仓库不能为空'
				}]
			},
			shiftType: {
				rules: [{
					required: true,
					errorMessage: '移位类型不能为空'
				}]
			},
			remark: {
				rules: [
					{required: false}
					// {
					// 	pattern:/^[\u4e00-\u9fa5-a-zA-Z0-9-_]*$/,
					// 	errorMessage:'包含非法字符'
					// }

				]
			}
		}
	})



	// function upper(e) {
	// 	console.log(e)
	// }

	// function lower(e) {
	// 	console.log(e)
	// }

	//滚动时触发事件
	function scroll(e) {
	}

	return {
		baseFormData,
		//upper,
		//lower,
		scroll
	};
}