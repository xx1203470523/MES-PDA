<template>
	<view class="user" :style="{'min-height' : pageHeight + 'px'}">
		<pda-nav title="个人中心">
		</pda-nav>

		<view class="header">
			<view class="custom-avatar">
				<image class="img" :src="page.userInfo.user?.avatar || '/static/imgs/mine/default-avatar.png'"
					mode="widthFix"></image>
			</view>

			<view class="ml-2 flex">
				<text class="text-sub-title">{{ page.userInfo.user?.userName }}</text>
				<view class="flex-row mt-1">
					<uni-icons type="phone"></uni-icons>
					<text class="text-sub ml-1">{{ page.userInfo.user?.phonenumber || '未补充电话号码' }} </text>
				</view>
			</view>

			<!-- <view class="flex-1 flex-row flex-justify-end">
				<uni-icons type="right"></uni-icons>
			</view> -->
		</view>

		<!-- <view class="banner">
			<uni-card class="banner-container" background-color="#5085FD" margin="0" padding="40rpx 0">
				<view class="flex flex-1">
					<view class="flex-row flex-align-center">
						<uni-icons type="fire-filled" color="#fff" size="36"></uni-icons>
						<text class="text-white ml-1">数据统计</text>
					</view>
					<view class="flex-row flex-justify-between mt-4">
						<view class="flex-1 flex flex-align-center flex-justify-center">
							<text class="text-sub-title text-white">
								{{ page.receivingRecords }}
							</text>
							<text class="text-sub text-white mt-1">验证记录</text>
						</view>
						<view class="flex-1 flex flex-align-center flex-justify-center">
							<text class="text-sub-title text-white">
								{{ page.onShelvesRecords }}
							</text>
							<text class="text-sub text-white mt-1">操作记录</text>
						</view>
					</view>
					<view class="divider"></view>
					<view class="flex mt-2">
						<tui-text type="white" text="感谢您的付出！"></tui-text>
					</view>
				</view>
			</uni-card>

			<view class="banner-decoration">
				<image class="decoration-image" src="../../../static/imgs/mine/bg.png" mode="widthFix"></image>
			</view>
		</view> -->

		<uni-grid class="mt-2" :column="4" :showBorder="false">
			<uni-grid-item>
				<view class="flex flex-justify-center flex-align-center p-2" @click="toUpdateUserInfo">
					<image class="grid-image" src="../../../static/imgs/mine/edit.png" mode="aspectFill"></image>
					<view class="mt-2">
						<tui-text text="编辑资料"></tui-text>
					</view>
				</view>
			</uni-grid-item>
			<!-- <uni-grid-item>
				<view class="flex flex-justify-center flex-align-center p-2" @click="toVersionUpdate">
					<image class="grid-image" src="../../../static/imgs/mine/update-version.png" mode="aspectFill">
					</image>
					<view class="mt-2">
						<tui-text text="版本更新"></tui-text>
					</view>
				</view>
			</uni-grid-item> -->
			<uni-grid-item>
				<view class="flex flex-justify-center flex-align-center p-2" @click="toUpdatePassword">
					<image class="grid-image" src="../../../static/imgs/mine/edit-password.png" mode="aspectFill">
					</image>
					<view class="mt-2">
						<tui-text text="修改密码"></tui-text>
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

		<tui-select title="请选择默认仓库" :z-index="999" v-model="page.warehouse.data" :list="page.warehouse.list"
			:show="page.warehouse.show" @confirm="saveWarehouseSetting" @close="closeWarehouseSetting"></tui-select>
	</view>
</template>

<script setup name="tabbar-user" lang="ts">
	import { onLoad, onShow } from '@dcloudio/uni-app'

	import { init } from './core'

	const {
		page,
		pageHeight,
		reloadUserInfoAsync,
		loginOut,
		toVersionUpdate,
		reloadUserDefaultWarehouse,
		toUpdatePassword,
		toUpdateUserInfo,
		// openWarehouseSetting,
		closeWarehouseSetting,
		saveWarehouseSetting
	} = init()

	onLoad(() => {
		page.windowInfo = uni.getWindowInfo()
	})

	onShow(async () => {
		await reloadUserInfoAsync()
		await reloadUserDefaultWarehouse()
	})
</script>

<style lang="scss" scoped>
	.user {
		position: relative;

		display: flex;
		flex-direction: column;
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
			align-items: center;
			flex-direction: row;

			padding: 40rpx 20rpx;
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