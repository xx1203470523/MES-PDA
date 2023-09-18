import { ref, reactive, unref } from "vue";
import type { PdaListItem } from '@/components/pda/pda-list/pda-list-types'
import type { DeliveryDetailData } from './delivery-detail-types'
import { detailSumAsync } from '@/api/modules/wms/delivery/delivery-order'

export function init() {
	const page = reactive<{
		modeIndex : number,
		windowInfo ?: UniNamespace.GetWindowInfoResult,
		tab : {
			current : number,
			items : Array<string>
		},
		info : {
			id : string,
			materialCode : string,
			//出库总数
			assignedTotal : number,
			//已扫描总数
			deliveryTotal : number,
			deliveryOrderCode : string,
			isCheckMaterial : boolean
		},
		list : {
			items : PdaListItem[],
			data : DeliveryDetailData[]
		}
	}>({
		modeIndex: 0,
		tab: {
			current: 0,
			items: ['全部数', '差异数', '无差异数']
		},
		info: {
			id: '',
			materialCode: '',
			assignedTotal: 0,
			deliveryTotal: 0,
			deliveryOrderCode: '',
			isCheckMaterial: false
		},
		list: {
			items: [
				{
					label: '物料名称',
					field: 'materialName'
				},
				{
					label: '单位',
					field: 'materialUnit'
				}
			],
			data: []
		}
	});

	const tabs = ref(null)
	async function getPdaGroup() {
		const data = await detailSumAsync(page.info.id)
		page.info.assignedTotal = data?.assignedTotal ?? 0
		page.info.deliveryTotal = data?.deliveryTotal ?? 0
		page.info.deliveryOrderCode = data?.deliveryOrderCode ?? ''
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
	function swiperTransition(e : any) {
		const _tabs = unref(tabs)
		_tabs.setDx(e.detail.dx)
	}
	function swiperAnimationfinish(e : any) {
		page.tab.current = e.detail.current

		const _tabs = unref(tabs)
		_tabs.unlockDx()
	}
	return {
		page,
		getPdaGroup,
		tabsChange,
		swiperChange,
		swiperTransition,
		swiperAnimationfinish,
		tabs
	};
}