/**
 * 字符串工具
 */

import type { TrimType, CaseType } from './string-utils-types'

/**
 * 字符串超出多少字显示省略号
 */
export function strOut(value : string, len = 0, type : 'star' | 'dot' = 'dot') {
	type = type || 'star';
	let restr = '';
	if (value) {
		if (value.length >= len) {
			restr = value.substring(0, len) + (type == 'star' ? '***' : '...');
		} else {
			restr = value;
		}
	}
	return restr;
}

/**
 * 将阿拉伯数字翻译成中文的大写数字
 */
export function numberToChinese(num : number) {
	let AA = new Array("零", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十")

	let BB = new Array("", "十", "百", "仟", "萬", "億", "点", "")

	let a = ("" + num).replace(/(^0*)/g, "").split("."),
		k = 0,
		re = ""

	for (let i = a[0].length - 1; i >= 0; i--) {
		switch (k) {
			case 0:
				re = BB[7] + re
				break;
			case 4:
				if (!new RegExp("0{4}//d{" + (a[0].length - i - 1) + "}$")
					.test(a[0]))
					re = BB[4] + re
				break;
			case 8:
				re = BB[5] + re;
				BB[7] = BB[5]
				k = 0;
				break;
		}

		if (k % 4 == 2 && Number(a[0].charAt(i + 2)) != 0 && Number(a[0].charAt(i + 1)) == 0) {
			re = AA[0] + re
		}

		if (Number(a[0].charAt(i)) != 0) {
			re = AA[a[0].charAt(i)] + BB[k % 4] + re
		}
		k++;
	}

	// 加上小数部分(如果有小数部分)
	if (a.length > 1) {
		re += BB[6];
		for (let i = 0; i < a[1].length; i++) {
			re += AA[a[1].charAt(i)]
		}
	}

	if (re == '一十') {
		re = "十"
	}

	if (re.match(/^一/) && re.length == 3) {
		re = re.replace("一", "");
	}

	return re
}

/**
 * 将数字转换为大写金额
 */
export function changeToChinese(num : number) {
	// 验证输入的字符是否为数字
	if (isNaN(num)) {
		return ""
	}

	let numStr = num.toString()

	// 替换tomoney()中的“,”
	numStr = numStr.replace(/,/g, "")

	// 替换tomoney()中的空格
	numStr = numStr.replace(/ /g, "")

	// 替换掉可能出现的￥字符
	numStr = numStr.replace(/￥/g, "")

	// 字符处理完毕后开始转换，采用前后两部分分别转换
	let part = String(numStr).split(".")

	let newchar = ""

	// 小数点前进行转化
	for (let i = part[0].length - 1; i >= 0; i--) {
		if (part[0].length > 10) {
			return ""
			// 若数量超过拾亿单位，提示
		}

		let tmpnewchar = ""

		let perchar = part[0].charAt(i)

		switch (perchar) {
			case "0":
				tmpnewchar = "零" + tmpnewchar
				break
			case "1":
				tmpnewchar = "壹" + tmpnewchar
				break
			case "2":
				tmpnewchar = "贰" + tmpnewchar
				break
			case "3":
				tmpnewchar = "叁" + tmpnewchar
				break
			case "4":
				tmpnewchar = "肆" + tmpnewchar
				break
			case "5":
				tmpnewchar = "伍" + tmpnewchar
				break
			case "6":
				tmpnewchar = "陆" + tmpnewchar
				break
			case "7":
				tmpnewchar = "柒" + tmpnewchar
				break
			case "8":
				tmpnewchar = "捌" + tmpnewchar
				break
			case "9":
				tmpnewchar = "玖" + tmpnewchar
				break
		}
		switch (part[0].length - i - 1) {
			case 0:
				tmpnewchar = tmpnewchar + "元"
				break
			case 1:
				if (Number(perchar) != 0) tmpnewchar = tmpnewchar + "拾"
				break
			case 2:
				if (Number(perchar) != 0) tmpnewchar = tmpnewchar + "佰"
				break
			case 3:
				if (Number(perchar) != 0) tmpnewchar = tmpnewchar + "仟"
				break
			case 4:
				tmpnewchar = tmpnewchar + "万"
				break
			case 5:
				if (Number(perchar) != 0) tmpnewchar = tmpnewchar + "拾"
				break
			case 6:
				if (Number(perchar) != 0) tmpnewchar = tmpnewchar + "佰"
				break
			case 7:
				if (Number(perchar) != 0) tmpnewchar = tmpnewchar + "仟"
				break
			case 8:
				tmpnewchar = tmpnewchar + "亿"
				break
			case 9:
				tmpnewchar = tmpnewchar + "拾"
				break
		}
		newchar = tmpnewchar + newchar
	}

	//小数点之后进行转化
	if (numStr.indexOf(".") != -1) {
		if (part[1].length > 2) {
			// alert("小数点之后只能保留两位,系统将自动截断");
			part[1] = part[1].substring(0, 2)
		}
		for (let i = 0; i < part[1].length; i++) {

			let tmpnewchar = ""

			let perchar = part[1].charAt(i)

			switch (perchar) {
				case "0":
					tmpnewchar = "零" + tmpnewchar
					break;
				case "1":
					tmpnewchar = "壹" + tmpnewchar
					break;
				case "2":
					tmpnewchar = "贰" + tmpnewchar
					break;
				case "3":
					tmpnewchar = "叁" + tmpnewchar
					break;
				case "4":
					tmpnewchar = "肆" + tmpnewchar
					break;
				case "5":
					tmpnewchar = "伍" + tmpnewchar
					break;
				case "6":
					tmpnewchar = "陆" + tmpnewchar
					break;
				case "7":
					tmpnewchar = "柒" + tmpnewchar
					break;
				case "8":
					tmpnewchar = "捌" + tmpnewchar
					break;
				case "9":
					tmpnewchar = "玖" + tmpnewchar
					break;
			}
			if (i == 0) tmpnewchar = tmpnewchar + "角"
			if (i == 1) tmpnewchar = tmpnewchar + "分"
			newchar = newchar + tmpnewchar
		}
	}

	// 替换所有无用汉字
	while (newchar.search("零零") != -1) {
		newchar = newchar.replace("零零", "零")
	}

	newchar = newchar.replace("零亿", "亿")

	newchar = newchar.replace("亿万", "亿")

	newchar = newchar.replace("零万", "万")

	newchar = newchar.replace("零元", "元")

	newchar = newchar.replace("零角", "")

	newchar = newchar.replace("零分", "")

	if (newchar.charAt(newchar.length - 1) == "元") {
		newchar = newchar + "整"
	}

	return newchar;
}

/**
 * 字符串去空格
 */
export function trim(value : string, type : TrimType = 'ba') {
	switch (type) {
		case 'all':
			return value.replace(/\s+/g, "")
		case 'ba':
			return value.replace(/(^\s*)|(\s*$)/g, "")
		case 'before':
			return value.replace(/(^\s*)/g, "")
		case 'after':
			return value.replace(/(\s*$)/g, "")
		default:
			return value
	}
}

/**
 * 转换大小写
 */
export function changeCase(value : string, type : CaseType = 'allToCase') {
	switch (type) {
		case 'firstLower':
			return value.replace(/\b\w+\b/g, (word) => {
				return word.substring(0, 1).toUpperCase() + word.substring(1).toLowerCase()
			})

		case 'firstUpper':
			return value.replace(/\b\w+\b/g, (word) => {
				return word.substring(0, 1).toLowerCase() + word.substring(1).toUpperCase()
			})

		case 'allToCase':
			return value.split('').map((word) => {
				if (/[a-z]/.test(word)) {
					return word.toUpperCase()
				} else {
					return word.toLowerCase()
				}
			}).join('')

		case 'allUpper':
			return value.toUpperCase()

		case 'allLower':
			return value.toLowerCase()

		default:
			return value
	}
}

/**
 * 插入字符串
 */
export function insert(source: string, index: number, newString: string){
	return source.slice(0, index) + newString + source.slice(index)	
}