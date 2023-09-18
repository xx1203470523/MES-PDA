/**
 * 数组工具
 */

/**
 * 判断一个元素是否在数组中
 */
export function contains(arr : Array<any>, val : any) {
	return arr.indexOf(val) != -1 ? true : false
}

/**
 * 数组去重
 */
export function distinct(arr : Array<any>) {
	if (Array.hasOwnProperty('from')) {
		return Array.from(new Set(arr))
	} else {
		let n = {},
			r = []

		for (let i = 0; i < arr.length; i++) {
			if (!n[arr[i]]) {
				n[arr[i]] = true
				r.push(arr[i])
			}
		}

		return r;
	}
}

/**
 * 数组移除某个值
 */
export function remove(arr : Array<any>, value : any) {
	let index = arr.indexOf(value);
	if (index > -1) {
		arr.splice(index, 1);
	}
	return arr;
}

/**
 * 取数组中的最大值
 */
export function max(arr : Array<number>) {
	return Math.max.apply(null, arr);
}

/**
 * 取数组中的最小值
 */
export function min(arr : Array<number>) {
	return Math.min.apply(null, arr);
}

/**
 * 数组求和
 */
export function sum(arr: Array<number>) {
	return arr.reduce((pre, cur) => {
		return pre + cur
	})
}