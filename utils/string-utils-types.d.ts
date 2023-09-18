/**
 * all 去除全部空格
 * ba 去除前后空格
 * before 去除前面空格
 * after 去除后面空格
 */
export type TrimType = 'all' | 'ba' | 'before' | 'after'

/**
 * firstUpper 首字母大写
 * firstLower 首字母小写
 * allToCase 全部小写转大写、大写转小写
 * allUpper 全部大写
 * allLower 全部小写
 */
export type CaseType = 'firstUpper' | 'firstLower' | 'allToCase' | 'allUpper' | 'allLower'