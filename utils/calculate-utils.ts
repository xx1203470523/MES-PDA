/**
 * 计算工具
 */

/**
 * 浮点数加法运算--解决精度丢失（传入Number，返回Number）
 * @param {Object} arg1
 * @param {Object} arg2
 */
export function floatAdd(arg1 : number, arg2 : number) {
	let r1: number, r2: number, m: number;
	try {
		r1 = arg1.toString().split('.')[1].length;
	} catch (e) {
		r1 = 0;
	}
	try {
		r2 = arg2.toString().split('.')[1].length;
	} catch (e) {
		r2 = 0;
	}
	m = Math.pow(10, Math.max(r1, r2));
	return (arg1 * m + arg2 * m) / m;
}

/**
 * 浮点数乘法运算--解决精度丢失（传入Number，返回Number）
 * @param {Object} arg1
 * @param {Object} arg2
 */
export function FloatMul(arg1: number, arg2: number) {
	let m = 0,
		s1 = arg1.toString(),
		s2 = arg2.toString();
	try {
		m += s1.split('.')[1].length;
	} catch (e) { }
	try {
		m += s2.split('.')[1].length;
	} catch (e) { }
	return (Number(s1.replace('.', '')) * Number(s2.replace('.', ''))) / Math.pow(10, m);
}