import { reactive } from "vue";

export function init() {

	const page = reactive<{
		scroll : {
			top : 0,
			oldTop : 0
		},
		windowInfo ?: UniNamespace.GetWindowInfoResult,
		fromDetail : {
			id : string,
			shiftCode : string
		},
		search : {
			materialCode : string,
			componentMaterialCode : string
		},
		info : {
			stockBinMaterialQuantity : number,
			stockShiftMaterialQuantity : number
		},
		tab : {
			current : number,
			items : Array<string>
		}
	}>({
		scroll: {
			top: 0,
			oldTop: 0
		},
		fromDetail: {
			id: '',
			shiftCode: ''
		},
		search: {
			materialCode: '',
			componentMaterialCode: ''
		},
		info: {
			//materialTotalQuantity:0
			stockBinMaterialQuantity: 0,
			stockShiftMaterialQuantity: 0
		},
		tab: {
			current: 0,
			items: ['全部数', '差异数', '无差异数']
		},
	})

	function upper(e) {
		console.log(e)
	}

	function lower(e) {
		console.log(e)
	}

	//滚动时触发事件
	function scroll(e) {
		page.scroll.top = e.detail.scrollTop
	}

	//列表点击事件
	function itemClick(data : any) {
		console.log(data)
	}

	/**
	 * 页签变更
	 */
	function tabsChange(index : number) {
		page.tab.current = index
	}

	/**
	 * swiper滑动
	 */
	function swiperChange(e : any) {
		// #ifndef APP-PLUS || H5 || MP-WEIXIN || MP-QQ
		let current = e.target.current || e.detail.current;
		tabsChange(current);
		// #endif
	}

	return {
		page,
		upper,
		lower,
		scroll,
		itemClick,
		swiperChange,
		tabsChange
	};
}