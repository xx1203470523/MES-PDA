import { reactive } from 'vue'
import { getUserInfoAsync, resetPassword } from '@/api/modules/user-center/user'
import type { resetPasswordType } from '@/api/modules/user-center/user-types'
import { back } from '@/utils/route-utils'

export function init({ vaild }) {
	const page = reactive<{
		windowInfo ?: UniNamespace.GetWindowInfoResult,
		userinfo : {
			id : string
		},
		input : {
			uid : string,
			oldpassword : string,
			newpassword : string,
			confirmpassword : string
		}
	}>({
		userinfo: {
			id: ''
		},
		input: {
			uid: '',
			oldpassword: '',
			newpassword: '',
			confirmpassword: ''
		}

	})

	const rules = {
		oldpassword: {
			rules: [
				{
					required: true,
					errorMessage: '请输入旧密码'
				}
			]
		},
		newpassword: {
			rules: [
				{
					required: true,
					errorMessage: '请输入新的密码'
				},
				{
					validateFunction: (rule, value, data, callback) => {
						return new Promise((resolve, reject) => {
							setTimeout(() => {
								if (page.input.confirmpassword == page.input.newpassword) {
									resolve(null)
								} else {
									reject(new Error('两次输入密码不一致'))
								}
							}, 10)
						})
					}
				}
			]
		},
		confirmpassword: {
			rules: [
				{
					required: true,
					errorMessage: '请再次输入新的密码'
				},
				{
					validateFunction: (rule, value, data, callback) => {
						return new Promise((resolve, reject) => {
							setTimeout(() => {
								if (page.input.confirmpassword == page.input.newpassword) {
									resolve(null)
								} else {
									reject(new Error('两次输入密码不一致'))
								}
							}, 10)
						})
					}
				}
			]
		}
	}


	/**
	 * 确认修改密码
	 */
	async function ConfirmPasswordAsync(data : resetPasswordType) {
		return await resetPassword(data)
	}

	/**
	 * 获取用户信息（工号）
	 */
	async function getUserInfo() {
		return await getUserInfoAsync()
	}

	/**
	 * 取消操作
	 */
	async function btnCancel() {
		back({
			redirectUrl: '/'
		})
	}

	/**
	 * 确认修改密码
	 */
	async function ConfirmAsync() {
		if (!await vaild()) {
			return
		}

		let data = {
			oldpassword: page.input.oldpassword,
			newpassword: page.input.newpassword
		}

		try {
			let result = await ConfirmPasswordAsync(data)

			if (!result) {
				uni.showToast({
					title: "修改密码成功！",
					icon: 'success',
					success: () => { OnInit() }
				})
			}
		}
		catch (error) {
			console.log(error)
		}
	}

	/**
	 * 初始化
	 */
	async function OnInit() {
		/* 加载工号信息 */
		var userinfo = await getUserInfo()
		page.userinfo.id = userinfo.user.id

		/* 清空文本框 */
		page.input.confirmpassword = ''
		page.input.oldpassword = ''
		page.input.newpassword = ''
	}

	return {
		page,
		rules,
		ConfirmAsync,
		getUserInfo,
		btnCancel,
		OnInit
	}
}