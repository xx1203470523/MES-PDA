<template>
	<view class="sfcbox" :style="{minHeight: pageHeight}">
		<uni-forms ref="formRef" class="p-2 bg-white border-bottom" errShowType="undertext" :modelValue="page.input">
			<uni-forms-item label="条码" name="sfc" required>
				<uni-easyinput ref="sfcInputRef" v-model="page.input.sfc" trim placeholder="请扫描条码"
				@input="scanSfcAsync"></uni-easyinput>
			</uni-forms-item>
			<uni-forms-item label="绑定条码" name="bindSfc" required>
				<uni-easyinput ref="bindSfcInputRef" v-model="page.input.bindSfc" trim placeholder="请扫描绑定条码"
				@input="scanBindSfcAsync"></uni-easyinput>
			</uni-forms-item>
		</uni-forms>

		<pda-list >
			<template #header>
				<view class="flex-row flex-justify-between">
					<view class="flex-row flex-1">
						<view class="flex-row">
							<tui-text text="绑定明细：" :size="28"></tui-text>
							<tui-text :text="page.dataList" type="danger" :size="28"></tui-text>
						</view>
						<view class="ml-4 flex-row">
							<tui-text text="已绑定：" :size="28"></tui-text>
							<tui-text :text="page.dataList" type="danger" :size="28"></tui-text>
						</view>
					</view>
					<view>
						<tui-text text="全部解绑" :size="28" type="primary" ></tui-text>
					</view>
				</view>
			</template>
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
	const { page, scanSfcAsync, scanBindSfcAsync } = init({
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
	onReady(() => {
		initFocus()
	})
</script>

<style lang="scss" scoped>
	.sfcbox {
		display: flex;
		flex-direction: column;
	}
</style>