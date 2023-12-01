<template>
	<view class="sfcbox" :style="{minHeight: pageHeight}">
		<uni-forms ref="formRef" class="p-2 bg-white border-bottom" errShowType="undertext" :modelValue="page.input">
			<uni-forms-item label="条码" name="sfc" required>
				<uni-easyinput ref="sfcInputRef" v-model="page.input.sfc" required trim placeholder="请扫描条码"
					@input="scanSfcAsync" @confirm="scanSfcAsync"></uni-easyinput>
			</uni-forms-item>
			<view class="text-box" scroll-y="true">
				<tui-text>
					工序：<tui-text style="color: orange;" :size="28">{{page.info.procedureName}}</tui-text>
					&ensp;&ensp;&ensp;&ensp;
					状态：<tui-text style="color: orange;" :size="28">{{page.info.processStatus}}</tui-text>
				</tui-text>
			</view>
		</uni-forms>

		<pda-list>
			<template #header>
				<view class="flex-row flex-justify-between">
					<view class="flex-row flex-1">
						<view class="flex-row">
							<tui-text text="更该信息:" :size="28"></tui-text>
						</view>
					</view>
				</view>

				<view>
					<uni-section title="更新工序">
						<uni-data-select placeholder="请选择工序" required :localdata="page.info.procedureListData"
							v-model="page.input.procedureId">
						</uni-data-select>
					</uni-section>
					<uni-section title="更新状态">
						<uni-data-select placeholder="请选择状态" required :localdata="page.info.processStatusListData"
							v-model="page.input.processStatus">
						</uni-data-select>
					</uni-section>

				</view>
			</template>
		</pda-list>
	</view>
</template>

<script lang="ts" name="sfcbox" setup>
	import { ref, computed, unref } from 'vue'

	import { init } from './core'

	import { to, back } from '@/utils/route-utils'

	import { onLoad, onReady, onNavigationBarButtonTap } from '@dcloudio/uni-app'

	const formRef = ref()
	const sfcInputRef = ref()

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
	 * 初始化
	 */
	const { page, scanSfcAsync, confirmAsync } = init({
		initFocus 
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

	onNavigationBarButtonTap((opt : any) => {
		if (opt.type == 'menu') {
			uni.showActionSheet({
				itemList: ['保存'],
				success: (res) => {
					switch (res.tapIndex) {
						case 0:
							confirmAsync()
							break
					}
				},
				fail: () => {
					initFocus()
				}
			})
		}
	})
</script>

<style lang="scss" scoped>
	.sfcbox {
		display: flex;
		flex-direction: column;
	}
</style>