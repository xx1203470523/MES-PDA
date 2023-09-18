/**
 * 颜色工具
 */

/**
 * hex颜色代码转换rgb
 */
export function hexToRGB(hex : string, opacity ?: number) {

	// 16进制颜色值校验规则
	var pattern = /^(#?)[a-fA-F0-9]{6}$/

	// 如果值不符合规则返回空字符
	if (!pattern.test(hex)) {
		return '';
	}

	// 如果有#号先去除#号
	var v = hex.replace(/#/, '');
	var rgbArr = [];
	var rgbStr = '';

	for (var i = 0; i < 3; i++) {
		var item = v.substring(i * 2, i * 2 + 2);
		var num = parseInt(item, 16);
		rgbArr.push(num);
	}

	rgbStr = rgbArr.join();
	rgbStr = 'rgb' + (opacity ? 'a' : '') + '(' + rgbStr + (opacity ? ',' + opacity : '') + ')';
	return rgbStr;
}