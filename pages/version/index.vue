<template>
	<view class="version" :style="{minHeight: pageHeight}">
		<uni-card :hide-actions="false">
			<view>
				<text>当前版本：{{ appStore.systemInfo.appVersion}}</text>
			</view>
			<view>
				<tui-text v-if="isHasNewVersion" :size="24" text="有新版本" type="danger"></tui-text>
			</view>
			<view class="mt-2 flex-row" v-if="isHasNewVersion">
				<tui-button height="70rpx" width="320rpx" type="black" :size="26" :loading="page.handle.isDownload"
					:disabled="page.handle.isDownload" @click="updateDownloadHandle">立即更新</tui-button>
			</view>
			<template #actions>
				<view>
					<text class="text-sub">{{ page.handle.progressTitle }}</text>
				</view>
				<view class="mt-2 flex-row">
					<tui-progress :percent="page.handle.progress"></tui-progress>
				</view>
			</template>
		</uni-card>

		<uni-card title="最新版本" :sub-title="`版本号：${page.result.info.versionName}`">
			<u-parse className="version-content" :content="page.result.content || ''"></u-parse>
		</uni-card>
	</view>
</template>

<script lang="ts" setup>
	import uParse from '@/components/uni/uParse/src/wxParse'

	import { onLoad, onShow } from '@dcloudio/uni-app'
	import { computed } from 'vue'

	import { init } from './core'

	const {
		page,
		appStore,
		updateCheckAsync,
		updateDownloadHandle
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

	const isHasNewVersion = computed(() => {
		return Number(appStore.systemInfo.appVersionCode) < page.result.info.versionCode
	})

	onShow(() => {
		updateCheckAsync()
	})

	onLoad(() => {
		page.windowInfo = uni.getWindowInfo()
	})
</script>

<style lang="scss" scoped>
	.version {}
</style>