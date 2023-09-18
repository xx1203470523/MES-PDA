import { reactive,watch } from "vue";
import type { PdaListItem } from '@/components/pda/pda-list/pda-list-types'
import type { StockShiftDetailData } from './stockshift-detail-add-types'

export function init({materialCodeInputFocus}) {
	const page = reactive<{
		scroll : {
			top : 0,
			oldTop : 0
		},
		windowInfo ?: UniNamespace.GetWindowInfoResult,
		//fromData:StockShiftDetailData,

		fromOperate : {
			id : string,
			shiftCode : string,
			wareHouseId : string,
			shiftType:string
		},

		list : {
			items : PdaListItem[],
			data : StockShiftDetailData[]
		},

		shiftType : any[],

		ishow : boolean,

		input : {
			fromBinCode : string,
			toBinCode : string,
			materialCode : string,
			shiftQuantity : number,
			shiftType : string,
			unit:string
		},

		isFous : {
			isToBinCodeFocus : boolean,
			isNumberBoxFocus : boolean,
			isMaterialFocus:boolean,
			isFromBinCodeFocus:boolean
		},

		isdisable : {
			isdisableNumberBox : boolean
		},

		ischeck : boolean,

		totalNumber : {
			scanQuanltity:number,
			stockQuanltity : number,
			totalScansQuanltity : number
		},
		rules:any,
		maxNumber:number,
		handle:{
			needConfirmQuantity:boolean
		}
	}>({
		scroll: {
			top: 0,
			oldTop: 0
		},
		// fromData:{
		// 	materialCode:'',
		// 	materialName:'',
		// 	shiftType:'',
		// 	materialUnit:'',
		// 	shiftCode:''
		// },

		fromOperate: {
			id: '',
			shiftCode: '',
			wareHouseId: '',
			shiftType:''
		},

		list: {
			items: [
				{
					label: '物料编码',
					field: 'materialCode'
				},
				{
					label: '物料名称',
					field: 'materialName'
				},
				{
					label: '移位类型',
					field: 'shiftType'
				},
				{
					label: '单位',
					field: 'materialUnit'
				},
				{
					label: '移位单号',
					field: 'shiftCode'
				}
			],

			data: [
			]
		},

		shiftType: [
			{
				text: '散件移出',
				value: '1'
			},
			{
				text: '整位移出',
				value: '2'
			}
		],

		ishow: true,

		input: {
			fromBinCode: '',
			toBinCode: '',
			materialCode: '',
			shiftQuantity: 1,
			shiftType: '',
			unit:''
		},

		isFous: {
			isToBinCodeFocus: false,
			isNumberBoxFocus: false,
			isMaterialFocus:false,
			isFromBinCodeFocus:false
		},

		isdisable: {
			isdisableNumberBox: true
		},

		ischeck: false,

		totalNumber: {
			scanQuanltity:0,
			stockQuanltity: 0,
			totalScansQuanltity: 0
		},
		
		rules:{
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
			}
		},
		maxNumber:100,
		handle:{
			needConfirmQuantity:false
		}
	});
	
	watch(() => page.handle.needConfirmQuantity, function (n) {
		materialCodeInputFocus(n)
	})

	function upper(e) {
		console.log(e)
	}

	function lower(e) {
		console.log(e)
	}

	function scroll(e) {
		page.scroll.oldTop = e.detail.scrollTop
	}

	function shiftChange(e : any) {
		
		page.input.shiftType = e

		if (e == '2') {
			page.ishow = false
		}
		else {
			page.ishow = true
		}
		
		page.input.fromBinCode=''
		page.input.toBinCode=''
		page.input.materialCode=''
		page.input.shiftQuantity=1
		page.input.unit=''

	}
	return {
		page,
		upper,
		lower,
		scroll,
		shiftChange
	};
}