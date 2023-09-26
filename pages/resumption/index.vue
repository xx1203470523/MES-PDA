<template>
	<view class="sfcbox" :style="{minHeight: pageHeight}">
		<uni-forms ref="formRef" class="p-2 bg-white border-bottom" errShowType="toast"
			:rules="page.formRules.pageRules" :modelValue="page.input">
			<uni-forms-item label-width="160rpx" label="条码" name="code" required>
				<uni-easyinput ref="codeInputRef" v-model="page.input.code" trim placeholder="扫描或输入"
					@confirm="codeConfirmAsync"></uni-easyinput>
			</uni-forms-item>
			<uni-forms-item label-width="160rpx" label="当前位置" name="nGLocationId" required>
				<uni-data-select v-model="page.input.nGLocationId" :localdata="page.selected.options"
					:clear="false"></uni-data-select>
			</uni-forms-item>
			<uni-forms-item label-width="160rpx" label="复投位置" name="repeatLocationId" required>
				<uni-data-select v-model="page.input.repeatLocationId" :localdata="page.selected.options"
					:clear="false"></uni-data-select>
			</uni-forms-item>

			<tui-button type="black" @click="repeatConfirmAsync">复投</tui-button>
		</uni-forms>

		<pda-list :items="page.result.items" :data="page.result.data">
			<template #header>
				<view class="flex-row flex-justify-between">
					<view class="flex-row flex-1">
						<view class="flex-row">
							<tui-text text="绑定明细：" :size="28"></tui-text>
							<tui-text :text="page.result.data.length" type="danger" :size="28"></tui-text>
						</view>
					</view>
				</view>
			</template>
		</pda-list>
	</view>
</template>

<script lang="ts" name="sfcbox" setup>
	import { ref, computed, unref } from 'vue'

	import { onLoad, onReady } from '@dcloudio/uni-app'

	import { init } from './core'

	const formRef = ref()
	const codeInputRef = ref()

	/**
	 * 表单验证
	 */
	async function formVaild() {
		const _form = unref(formRef)
		if (_form) {
			try {
				await _form.validate()
				return true
			} catch (err) {
				return false
			}
		}
	}

	/**
	 * 定位到boxCode
	 */
	function codeInputFocus() {
		const _codeInputRef = unref(codeInputRef)
		if (_codeInputRef) {
			_codeInputRef.toFocus()
		}
	}

	/**
	 * 初始化
	 */
	const { page, initAsync, codeConfirmAsync, repeatConfirmAsync } = init({
		formVaild,
		codeInputFocus
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
		initAsync()
	})

	/**
	 * 首次渲染完毕后执行
	 */
	onReady(() => {
		codeInputFocus()
	})
</script>

<style lang="scss" scoped>
	.sfcbox {
		display: flex;
		flex-direction: column;
	}
</style>