<template>
	<view class="user" :style="{'min-height' : pageHeight}">
		<tui-status-bar></tui-status-bar>

		<view class="header">
			<view class="flex">
				<text class="user-name">{{ page.userInfo.user?.userName }}</text>
				<text class="text-sub mt-2">Thanks for your work</text>
				<view class="mt-2">
					<text class="text-sub">当前版本 {{ appStore.systemInfo.appVersion }}</text>
				</view>
				<view class="mt-1" v-if="isHasNewVersion">
					<tui-tag plain>有新版本</tui-tag>					
				</view>
			</view>

			<view class="custom-avatar">
				<image class="img" :src="page.userInfo.user?.avatar || '/static/imgs/mine/default-avatar.png'"
					mode="widthFix"></image>
			</view>
		</view>

		<uni-grid class="mt-2" :column="4" :showBorder="false">
			<uni-grid-item>
				<view class="flex flex-justify-center flex-align-center p-2" @click="toUpdateUserInfo">
					<image class="grid-image" src="../../../static/imgs/mine/edit.png" mode="aspectFill"></image>
					<view class="mt-2">
						<tui-text text="编辑资料"></tui-text>
					</view>
				</view>
			</uni-grid-item>
			<uni-grid-item>
				<view class="flex flex-justify-center flex-align-center p-2" @click="toUpdatePassword">
					<image class="grid-image" src="../../../static/imgs/mine/edit-password.png" mode="aspectFill">
					</image>
					<view class="mt-2">
						<tui-text text="修改密码"></tui-text>
					</view>
				</view>
			</uni-grid-item>
			<uni-grid-item>
				<view class="flex flex-justify-center flex-align-center p-2" @click="toUpdateVersion">
					<image class="grid-image" src="../../../static/imgs/mine/update-version.png" mode="aspectFill">
					</image>
					<view class="mt-2">
						<tui-text text="版本更新"></tui-text>
					</view>
				</view>
			</uni-grid-item>
			<uni-grid-item @click="loginOut">
				<view class="flex flex-justify-center flex-align-center p-2">
					<image class="grid-image" src="../../../static/imgs/mine/logout.png" mode="aspectFill"></image>
					<view class="mt-2">
						<tui-text text="退出登录"></tui-text>
					</view>
				</view>
			</uni-grid-item>
		</uni-grid>
	</view>
</template>

<script setup name="tabbar-user" lang="ts">
	import { onLoad, onShow } from '@dcloudio/uni-app'
	import { computed } from 'vue'

	import { init } from './core'

	import { useAppStore } from '@/store/app'

	const appStore = useAppStore()

	const {
		page,
		reloadUserInfoAsync,
		toUpdatePassword,
		toUpdateUserInfo,
		toUpdateVersion,
		loginOut
	} = init()

	const isHasNewVersion = computed(() => {
		if (!appStore.newVersion) {
			return false
		}
		
		if(appStore.newVersion.versionCode > Number(appStore.systemInfo.appVersionCode)){
			return true
		}else{
			return false
		}
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

	onLoad(() => {
		page.windowInfo = uni.getWindowInfo()
	})

	onShow(async () => {
		await reloadUserInfoAsync()
		await appStore.versionCheckAsync()
	})
</script>

<style lang="scss" scoped>
	.custom-button {
		width: 600rpx;

		border-radius: 60rpx;

		background-color: $uni-primary;
		color: #fff;
	}


	.user {
		position: relative;
		background-color: #fff;

		.nav-left {
			display: flex;
			flex-direction: row;
			align-items: center;

			.nav-left-content {
				display: flex;
				flex-direction: row;
				align-items: center;
			}
		}

		.header {
			background-color: #f5f5f5;

			display: flex;
			flex-direction: row;

			padding: 40rpx 40rpx;

			.user-name {
				font-size: 48rpx;
				font-weight: 700;

				width: 515rpx;
				color: #152338;

				white-space: nowrap;
				overflow: hidden;
				text-overflow: ellipsis;
			}
		}

		.banner {
			padding: 0 20rpx;
			background-color: #f5f5f5;

			position: relative;

			.banner-container {
				height: 450rpx;
				border-radius: 20rpx
			}

			.banner-decoration {
				position: absolute;

				bottom: -10rpx;
				left: 0;

				width: 750rpx;

				z-index: 1;

				.decoration-image {
					width: 750rpx;
				}
			}
		}
	}
</style>