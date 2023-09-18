/**
 * 随机工具
 */

/**
 * 返回随机数（与时间戳拼接）
 */
export function uniqueId() {
	let a = Math.random,
		b = parseInt

	return Number(new Date()).toString() + b((10 * a()).toString()) + b((10 * a()).toString()) + b((10 * a()).toString());
}

/**
 * 数组随机洗牌算法
 */
export function shuffle(arr : Array<any>) {
	let result = [],
		random : number;

	while (arr.length > 0) {
		random = Math.floor(Math.random() * arr.length);
		result.push(arr[random])
		arr.splice(random, 1)
	}

	return result;
}

/**
 * 取随机整数
 * @param {Object} min
 * @param {Object} max
 */
export function random(min : number, max : number) {
	if (arguments.length === 2) {
		return Math.floor(min + Math.random() * ((max + 1) - min))
	} else {
		return null;
	}
}