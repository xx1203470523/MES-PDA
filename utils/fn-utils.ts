/**
 * 函数工具
 */

/**
 * @method 函数防抖
 * @desc 短时间内多次触发同一事件，只执行最后一次，或者只执行最开始的一次，中间的不执行。
 * @param func 目标函数
 * @param wait 延迟执行毫秒数
 * @param immediate true - 立即执行， false - 延迟执行
*/
export function debounce(func : Function, timeout : any, wait = 1000, immediate = true) {
	return function () {
		let context = this,
			args = arguments

		if (timeout) {
			clearTimeout(timeout)
		}

		if (immediate) {
			let callNow = !timeout

			timeout = setTimeout(() => {
				timeout = null
			}, wait)

			if (callNow) {
				func.apply(context, args)
			}
		} else {
			timeout = setTimeout(() => {
				func.apply(context, args)
			}, wait)
		}

		return timeout
	}
}

/**
 * @method 函数节流
 * @desc 指连续触发事件，但是在 n 秒内只执行一次函数。即 2n 秒内执行 2 次... 。会稀释函数的执行频率。
 * @param func 函数
 * @param wait 延迟执行毫秒数
 * @param type 1 在时间段开始的时候触发 2 在时间段结束的时候触发
*/
export function throttle(func : Function, timeout : any, wait = 500) {
	return function () {
		let context = this
		let args = arguments

		if (timeout) {
			clearTimeout(timeout)
		}

		timeout = setTimeout(() => {
			func.apply(context, args)
		}, wait)

		return timeout
	}
}