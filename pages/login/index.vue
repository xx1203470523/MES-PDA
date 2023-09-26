<template>
	<view class="login" :style="{ 'height' : pageHeight }">
		<view class="flex-row flex-align-center flex-justify-center">
			<image class="logo" src="../../static/imgs/login/logo.png" mode="heightFix"></image>
		</view>
		<uni-forms ref="form" class="login-content" errShowType="toast" :rules="rules" :modelValue="page.input">
			<uni-forms-item class="login-content-item" name="username">
				<uni-easyinput v-model="page.input.username" placeholder="请输入工号"
					prefixIcon="person-filled"></uni-easyinput>
			</uni-forms-item>
			<uni-forms-item class="login-content-item" name="password">
				<uni-easyinput v-model="page.input.password" placeholder="请输入密码" type="password"
					prefixIcon="locked-filled" passwordIcon></uni-easyinput>
			</uni-forms-item>
			<view class="login-content-item login-content-item-end">
				<basic-checkbox v-model="page.input.loginRemember" label="记住登录状态"></basic-checkbox>
			</view>
			<view class="login-content-item mt-4">
				<tui-form-button width="500rpx" text="登录" @click="loginHandle"></tui-form-button>
			</view>
		</uni-forms>
	</view>
</template>

<script lang="ts" name="login" setup>
	import { ref, computed, unref } from 'vue'
	import { onLoad } from '@dcloudio/uni-app'
	import { init, rules } from './core';

	const form = ref()

	async function vaild() {
		const _form = unref(form)
		if (_form) {
			try {
				await _form.validate()
				return true
			} catch (err) {
				return false
			}
		}
	}

	const { page, loginHandle } = init({ vaild });
	
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

	onLoad((opt) => {
		page.windowInfo = uni.getWindowInfo()
		if (opt.redirect) {
			page.redirect = decodeURIComponent(opt.redirect)
		}
	})
</script>

<style lang="scss" scoped>
	.login {
		display: flex;
		flex-direction: column;
		background-color: #fff;

		.logo {
			margin-top: 100rpx;
			height: 400rpx;
		}

		.login-content {
			margin-top: 100rpx;
			padding-left: 100rpx;
			padding-right: 100rpx;

			display: flex;

			.login-content-item {
				margin-bottom: 30rpx;

				display: flex;
				flex-direction: row;
				align-items: center;
				justify-content: center;
			}

			.login-content-item-end {
				justify-content: flex-end;
			}
		}
	}
</style>