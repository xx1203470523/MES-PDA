<template>
	<view class="sfcbox" :style="{minHeight: pageHeight}">
		<uni-forms ref="formRef" class="p-2 bg-white border-bottom" errShowType="undertext" :modelValue="page.input">
			<uni-forms-item label="条码" name="sfc" required>
				<uni-easyinput ref="sfcInputRef" v-model="page.input.sfc" trim placeholder="请扫描条码"
					@confirm="scanSfcAsync"></uni-easyinput>
			</uni-forms-item>
			<uni-forms-item label="绑定条码" name="bindSfc" required>
				<uni-easyinput ref="bindSfcInputRef" v-model="page.input.bindSfc" trim placeholder="请扫描绑定条码"
					@input="scanBindSfcAsync" @confirm="scanBindSfcAsync"></uni-easyinput>
			</uni-forms-item>
			<uni-forms-item label="绑定工序" name="procedureId" required>
				<uni-data-select placeholder="请选择工序" required :localdata="page.input.procedureList"
					v-model="page.input.procedureId">
				</uni-data-select>
			</uni-forms-item>
		</uni-forms>

		<pda-list :items="page.dataList.items" :data="page.dataList.data" @query="queryListAsync">
			<template #right={row}> <a href='#' @click="deleteBindSfcAsync(row)">解绑</a></template>
		</pda-list>

	</view>
</template>

<script lang="ts" name="sfcbox" setup>
	import { ref, computed, unref } from 'vue'

	import { init } from './core'

	import { onLoad, onReady } from '@dcloudio/uni-app'

	const formRef = ref()
	const sfcInputRef = ref()
	const bindSfcInputRef = ref()
	const paging = ref()


	/**
	 * 初始化焦点
	 */
	function initFocus() {
		const _sfcInputRef = unref(sfcInputRef)
		if (_sfcInputRef) {
			_sfcInputRef.toFocus()
		}
	}

	/**
	 * 焦点定位到bindSfc
	 */
	function bindSfcOnFocus() {
		const _bindSfcInputRef = unref(bindSfcInputRef)
		if (_bindSfcInputRef) {
			_bindSfcInputRef.toFocus()
		}
	}

	/**
	 * 初始化
	 */
	const { page, scanSfcAsync, scanBindSfcAsync, queryListAsync, deleteBindSfcAsync, getByCodesAsync } = init({
		bindSfcOnFocus
	})

	/**
	 * 动态计算页面高度
	 */
	const pageHeight = computed(() => {
		let height = 0

		//#ifdef APP
		height = page.windowInfo.windowHeight
		// #endif

		// #ifndef APP
		height = page.windowInfo.windowHeight - 44
		// #endif

		return height + 'px'
	})

	/**
	 * 初次加载执行
	 */
	onLoad(() => {
		page.windowInfo = uni.getWindowInfo()
	})

	/**
	 * 首次渲染完毕后执行
	 */
	onReady(async () => {
		initFocus()

		const query = { "procedureCodes": 'OP14,OP27' }
		page.input.procedureList = await getByCodesAsync(query)
	})
</script>

<style lang="scss" scoped>
	.sfcbox {
		display: flex;
		flex-direction: column;
	}
</style>