import { ref, reactive, unref } from "vue";
import type { PdaListItem } from '@/components/pda/pda-list/pda-list-types'
import type { WithdrawPageData } from './withdraw-detail-types'
import { getPdaGroupApi } from '@/api/modules/wms/withdraw/withdraw-detail'

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
			//通知总数
			noticeTotal : number,
			//已下架总数
			withdrawOrderQuantity : number,
			//待下架总数
			withdrawOrderAssignedTotal : number,
			withdrawOrderCode : string
		},
		list : {
			items : PdaListItem[],
			data : WithdrawPageData[]
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
			noticeTotal: 0,
			withdrawOrderQuantity: 0,
			withdrawOrderAssignedTotal: 0,
			withdrawOrderCode: ''
		},
		list: {
			items: [
				{
					label: '通知数',
					field: 'deliveryNoticeQuantity'
				}, {
					label: '物料名称',
					field: 'materialName'
				},
				{
					label: '单位',
					field: 'materialUnit'
				},
				{
					label: '实际库位',
					field: 'warehouseBinCodeReal'
				},
				{
					label: '建议库位',
					field: 'warehouseBinCode'
				}
			],
			data: []
		}
	});

	const tabs = ref(null)
	async function getPdaGroup() {
		const list = await getPdaGroupApi({ withdrawOrderId: page.info.id });
		page.info.noticeTotal = list?.noticeTotal ?? 0
		page.info.withdrawOrderAssignedTotal = (list?.noticeTotal ?? 0) - (list?.withdrawOrderQuantity ?? 0)
		page.info.withdrawOrderQuantity = list?.withdrawOrderQuantity ?? 0
		page.info.withdrawOrderCode = list?.withdrawOrderCode ?? ''
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