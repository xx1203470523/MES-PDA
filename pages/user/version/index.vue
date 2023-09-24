<template>
	<view class="user-version" :style="{height: pageHeight}">
		<!-- <uni-card title="版本信息">
			<view>
				<tui-text text="当前版本:" :size="30"></tui-text>
				<view>
					<tui-text type="gray" :text="page.handle.versionCode" :size="24"></tui-text>
				</view>
				<tui-text text="最新版本:" :size="30"></tui-text>
				<view>
					<tui-text type="gray" :text="page.handle.subTitle" :size="24"></tui-text>
				</view>
			</view>
		</uni-card>

		<uni-card title="新版本内容">
			<u-parse className="content" :content="page.handle.content || ''"></u-parse>
		</uni-card>
		<uni-card :title="page.handle.process">
			<tui-progress :percent="page.handle.progress" show-info></tui-progress>
		</uni-card>
		<uni-card v-if="tip.handle.show">
			<view v-if="tip.handle.show" class="mt-4">
				<tui-alerts :type="tip.handle.type" :title="tip.handle.title" :desc="tip.handle.content"></tui-alerts>
			</view>
		</uni-card> -->
		<view class="flex flex-align-center">
			<view class="p-2">
				<tui-form-button width="350rpx" height="70rpx" @click="updateHandle" :size="26" :text="btnText"
					:disabled="page.handle.isNew"></tui-form-button>
			</view>
		</view>
	</view>
</template>

<script lang="ts" name="user-version" setup>
	import { computed } from 'vue'

	import { onLoad, onHide, onShow } from '@dcloudio/uni-app'

	// import uParse from '@/components/uni/uParse/src/wxParse'

	import marked from '@/components/uni/marked'

	import { init } from './core'

	const {
		page,
		reloadVersionCode,
		// tip,
		updateHandle,
		updateCheck
	} = init()

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

	const btnText = computed(() => {
		if (page.handle.isNew) {
			return '当前已是最新版本'
		} else {
			return '点击获取最新版本'
		}
	})

	// const content = computed(() => {
	// 	if (page.response && page.response.content) {
	// 		return marked(page.response.content)
	// 	} else {
	// 		return ''
	// 	}
	// })

	onShow(async () => {
		// #ifdef APP
		await reloadVersionCode()
		// #endif

		// updateCheck()
	})

	onLoad(() => {
		page.windowInfo = uni.getWindowInfo()
	})

	// onHide(() => {
	// 	// #ifndef APP-PLUS
	// 	uni.hideKeyboard()
	// 	// #endif
	// 	// #ifdef APP-PLUS
	// 	plus.key.hideSoftKeybord()
	// 	// #endif
	// })
</script>

<style lang="scss" scoped>
	.received-main {
		display: flex;
		background-color: #fff;

		.received-main-statistics {
			border-radius: 24rpx;
		}
	}
</style>