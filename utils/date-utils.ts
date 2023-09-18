/**
 * 日期工具
 */

/**
 * 格式化时间-小于10补0
 */
export function formatDigit(n: number){
	return n.toString().replace(/^(\d)$/, '0$1');
}

/**
 * 格式化时间，通用
 * @params formats Y 年 M 月 D 日 h 时 m 分 s 秒
 */
export const formatDate = (timestamp : string | Date, formats: string) => {	
	formats = formats || 'Y-M-D';
	let myDate = undefined;
	if (timestamp) {
		if (typeof (timestamp) != 'string') {
			myDate = timestamp;
		} else {
			myDate = new Date(timestamp);
		}
	} else {
		myDate = new Date();
	}

	let year = myDate.getFullYear();
	let month = formatDigit(myDate.getMonth() + 1);
	let day = formatDigit(myDate.getDate());
	let hour = formatDigit(myDate.getHours());
	let minute = formatDigit(myDate.getMinutes());
	let second = formatDigit(myDate.getSeconds());
	return formats.replace(/Y|M|D|h|m|s/g, (matches) => {
		return {
			Y: year,
			M: month,
			D: day,
			h: hour,
			m: minute,
			s: second
		}[matches];
	});
}