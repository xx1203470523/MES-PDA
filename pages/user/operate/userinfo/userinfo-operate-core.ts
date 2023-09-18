import type { updateuserinfotype } from '@/api/modules/user-center/user-types'
import { reactive } from 'vue'
import { getUserInfoAsync, updateuserinfo } from '@/api/modules/user-center/user'
import { back } from '@/utils/route-utils'

export function init({ }) {
	const page = reactive<{
		windowInfo ?: UniNamespace.GetWindowInfoResult,
		input : {
			nickname : string,
			phonenumber : string,
			email : string
		},
		userinfo : {
			id : string,
			nickname : string,
			phonenumber : string,
			email : string
		},
		warehouse:{
			wareHouseId: string,
		},
		selectData_wareHouses : any[],
	}>({
		input: {
			nickname: '',
			phonenumber: '',
			email: ''
		},
		userinfo: {
			id: '',
			nickname: '',
			phonenumber: '',
			email: ''
		},
		warehouse:{
			wareHouseId: '',
		},
		selectData_wareHouses: [
		]
	})

	const rules = {
		nickname: {
			ruls: [
				{
					required: true,
					errorMessage: '请输入用户昵称'
				}
			]
		}
	}
	
	async function getUserInfo() {
		return await getUserInfoAsync()
	}
	
	async function backhome() {
		back({
			redirectUrl: '/'
		})
	}
	
	
	async function Onint() {
		var userinfo = await getUserInfo()
		console.log(userinfo)
		page.input.nickname = userinfo.user.nickName
		page.input.phonenumber = userinfo.user.phonenumber
		page.input.email = userinfo.user.email
		page.userinfo.id = userinfo.user.id
	}

	async function confirmasync(data : updateuserinfotype) {
		try {
			var result = await updateuserinfo(data)
			if (result == '') {
				uni.showToast({
					title: "修改账户信息成功！",
					icon: 'success',
					success: () => {
						Onint()
					}
				})
			}
		} catch (e) {
			
		}
	}


	return {
		page,
		rules,
		getUserInfo,
		backhome,
		confirmasync,
		Onint
	}
}