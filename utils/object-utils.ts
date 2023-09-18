/**
 * 对象工具
 */

/**
 * 获取值
 */
function fieldGetValue(field : any, fieldSplit : string[], floor : number, deep : number) {
	if (floor === deep) {
		return field
	} else {
		if (!field) {
			switch (typeof field) {
				case 'object':
					field = {}
					break

				case 'number':
					field = 0
					break

				default:
					field = ''
					break
			}
		}

		return fieldGetValue(field[fieldSplit[floor]], fieldSplit, ++floor, deep)
	}
}

/**
 * 设置值
 */
function fieldSetValue(field : any, fieldSplit : string[], floor : number, deep : number, value : any) {
	if (floor === deep) {
		field[fieldSplit[floor]] = value
	} else {
		if (typeof field[fieldSplit[floor]] !== 'object') {
			field[fieldSplit[floor]] = {}
		}
		fieldSetValue(field[fieldSplit[floor]], fieldSplit, ++floor, deep, value)
	}
}

/**
 * 通过递归查找最底层对象
 * @param field
 * @param fieldStr
 * @returns
 */
export function fieldGet(field : any, fieldStr : string) {
	const fieldSplit = fieldStr.split('.')
	return fieldGetValue(field, fieldSplit, 0, fieldSplit.length)
}

/**
 * 为对象赋值
 * @param field
 * @param fieldStr
 * @param value
 */
export function fieldSet(field : any, fieldStr : string, value : any) {
	const fieldSplit = fieldStr.split('.')
	fieldSetValue(field, fieldSplit, 0, fieldSplit.length - 1, value)
}