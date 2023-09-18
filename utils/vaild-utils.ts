/**
 * 验证工具
 */

import type { vaildType } from './vaild-utils-types'

/**
 * 判断值的类型
 */
export function checkValueType(value : string, type : vaildType) {
	switch (type) {
		// 手机号码
		case 'phone':
			return /^1[3|4|5|6|7|8|9][0-9]{9}$/.test(value)

		// 座机
		case 'tel':
			return /^(0\d{2,3}-\d{7,8})(-\d{1,4})?$/.test(value)

		// 身份证
		case 'card':
			return /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/.test(value)

		// 密码以字母开头，长度在6~18之间，只能包含字母、数字和下划线
		case 'pwd':
			return /^[a-zA-Z]\w{5,17}$/.test(value)

		// 邮政编码
		case 'postal':
			return /[1-9]\d{5}(?!\d)/.test(value)

		// QQ号
		case 'qq':
			return /^[1-9][0-9]{4,9}$/.test(value)

		// 邮箱
		case 'email':
			return /^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$/.test(value)

		// 金额(小数点2位)
		case 'money':
			return /^\d*(?:\.\d{0,2})?$/.test(value)

		// 网址
		case 'url':
			return /(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&:/~\+#]*[\w\-\@?^=%&/~\+#])?/.test(value)

		// IP
		case 'ip':
			return /((?:(?:25[0-5]|2[0-4]\\d|[01]?\\d?\\d)\\.){3}(?:25[0-5]|2[0-4]\\d|[01]?\\d?\\d))/.test(value);

		// 日期时间
		case 'date':
			return /^(\d{4})\-(\d{2})\-(\d{2}) (\d{2})(?:\:\d{2}|:(\d{2}):(\d{2}))$/.test(value) ||
				/^(\d{4})\-(\d{2})\-(\d{2})$/
					.test(value)
		// 数字
		case 'number':
			return /^[0-9]$/.test(value)

		// 英文
		case 'english':
			return /^[a-zA-Z]+$/.test(value)

		// 中文
		case 'chinese':
			return /^[\\u4E00-\\u9FA5]+$/.test(value)

		// 小写
		case 'lower':
			return /^[a-z]+$/.test(value)

		// 大写
		case 'upper':
			return /^[A-Z]+$/.test(value)

		// HTML标记
		case 'html':
			return /<("[^"]*"|'[^']*'|[^'">])*>/.test(value)

		default:
			return true;
	}
}

/**
 * 严格的身份证号码校验
 */
export function isCardID(value : string) {
	if (!/(^\d{15}$)|(^\d{17}(\d|X|x)$)/.test(value)) {
		console.log('你输入的身份证长度或格式错误')
		return false
	}
	
	//身份证城市
	const aCity = {
		11: "北京",
		12: "天津",
		13: "河北",
		14: "山西",
		15: "内蒙古",
		21: "辽宁",
		22: "吉林",
		23: "黑龙江",
		31: "上海",
		32: "江苏",
		33: "浙江",
		34: "安徽",
		35: "福建",
		36: "江西",
		37: "山东",
		41: "河南",
		42: "湖北",
		43: "湖南",
		44: "广东",
		45: "广西",
		46: "海南",
		50: "重庆",
		51: "四川",
		52: "贵州",
		53: "云南",
		54: "西藏",
		61: "陕西",
		62: "甘肃",
		63: "青海",
		64: "宁夏",
		65: "新疆",
		71: "台湾",
		81: "香港",
		82: "澳门",
		91: "国外"
	}
	
	if (!aCity[Number(value.substring(0, 2))]) {
		console.log('你的身份证地区非法')
		return false
	}

	// 出生日期验证
	let sBirthday = (value.substring(6, 4) + "-" + Number(value.substring(10, 2)) + "-" + Number(value.substring(12, 2))).replace(
		/-/g,
		"/"),
		d = new Date(sBirthday)

	if (sBirthday != (d.getFullYear() + "/" + (d.getMonth() + 1) + "/" + d.getDate())) {
		console.log('身份证上的出生日期非法')
		return false
	}

	// 身份证号码校验
	let sum = 0,
		weights = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2],
		codes = "10X98765432"
	for (let i = 0; i < value.length - 1; i++) {
		sum += Number(value[i]) * weights[i];
	}
	let last = codes[sum % 11]; //计算出来的最后一位身份证号码
	if (value[value.length - 1] != last) {
		console.log('你输入的身份证号非法')
		return false
	}

	return true
}

/**
 * 检查密码安全等级
 */
export function checkPwdLevel(value: string){
	let Lv = 1;
	if (value.length < 6) {
		return Lv
	}
	if (/[0-9]/.test(value)) {
		Lv++
	}
	if (/[a-z]/.test(value)) {
		Lv++
	}
	if (/[A-Z]/.test(value)) {
		Lv++
	}
	if (/[\.|-|_]/.test(value)) {
		Lv++
	}
	return Lv;
}