import { onMounted, reactive, ref, unref } from "vue";
import { to } from '@/utils/route-utils'
import type { PdaListItem } from '@/components/pda/pda-list/pda-list-types'
import type { WithdrawData } from './withdraw-main-types'
import { getPdaGroupApi } from '@/api/modules/wms/withdraw/withdraw-main'
import { getListApi } from '@/api/modules/wms/withdraw/withdraw-main'

export function init({ withdrawbillcodeFocus }) {
	const paging = ref(null)

	const page = reactive<{
		params : {
			pageIndex : number | 1,
			pageSize : number | 5,
			keyWords ?: string,
			deliveryNoticeId ?: string,
			WithdrawOrderStatus : number
		},
		windowInfo ?: UniNamespace.GetWindowInfoResult,
		list : {
			items : PdaListItem[],
			data : WithdrawData[],
			totalPages : number
		},
		info : {
			withdrawOrderAssignedTotal : number,
			withdrawOrderBillCount : number
		},
	}>({
		params: {
			pageIndex: 1,
			pageSize: 5,
			keyWords: '',
			WithdrawOrderStatus: 0
		},
		list: {
			items: [
				{
					label: '仓库',
					field: 'warehouseName'
				},
				{
					label: '同步单号',
					field: 'deliveryNoticeDto.syncCode'
				},
				{
					label: '出库通知单号',
					field: 'deliveryNoticeDto.deliveryNoticeCode'
				},
				{
					label: '出库单号',
					field: 'deliveryOrderCode'
				}, {
					label: '下架建议单号',
					field: 'withdrawSuggestionDto.withdrawSuggestionCode'
				}
				, {
					label: '往来单位',
					field: 'contactName'
				}, {
					label: '单据日期',
					field: 'withdrawSuggestionDto.createdOn'
				}, {
					label: '创建日期',
					field: 'createdOn'
				},
				{
					label: '备注',
					field: 'remark'
				}
			],
			data: [],
			totalPages: 0
		}
		, info: {
			withdrawOrderAssignedTotal: 0,
			withdrawOrderBillCount: 0
		},
	});

	function itemClick(item : any) {
		to('/pages/delivery/withdraw/operate/withdraw-operate?id=' + item.id)
	}



	async function getPdaGroup() {
		const pdaGroup = await getPdaGroupApi({ "withdrawOrderStatus": 0 });
		page.info = pdaGroup
	}

	/**
	 * 查询数据
	 */
	async function queryList(pageIndex : number, pageSize : number) {
		uni.showLoading({
			title: '数据加载中...',
			mask: true
		})
		try {
			page.params.pageIndex = pageIndex
			page.params.pageSize = pageSize

			const { data } = await getListApi(page.params);

			paging.value.complete(data)

			await getPdaGroup()
		}
		catch (error) {
			paging.value.complete(false)
		} finally {
			uni.hideLoading()
		}
	}
	/**
	 * 重新加载数据
	 */
	async function reloadList() {
		const _paging = unref(paging)
		if (_paging) {
			_paging.reload()
		}
	}

	return {
		page,
		itemClick,
		queryList,
		reloadList,
		getPdaGroup,
		paging
	};
}