import { NgStateEnum } from './enum'

/**
 * 明细对象
 */
export type ManuSfcBindOutputType = {
	/**
	 * id
	 */
	id ?: string

	/**
	 * 模组码
	 */
	sfc ?: string

	/**
	 * 绑定设备码
	 */
	bindSFC ?: string

	/**
	 * 条码绑定类型 ; 1：模组绑定电芯 2：绑定模组
	 */
	type ?: number

	/**
	 * 绑定状态 ; 0-解绑;1-绑定
	 */
	status ?: number

	/**
	 * 位置号
	 */
	location ?: number
}

/**
 * SFC输出
 */
export type BindSFCOutputType = {
	/**
	 * 明细集
	 */
	data: Array<ManuSfcBindOutputType>
	
	/**
	 * NG状态
	 */
	ngState: NgStateEnum
	
	/**
	 * NG位置
	 */
	nGLocationId: string
}

/**
 * SFC输入
 */
export type BindSFCInputType = {
	/**
	 * 模组码
	 */
	sFC : string
	
	/**
	 * 操作类型
	 */
	operateType?: NgStateEnum
}

/**
 * 解绑输入
 */
export type UnBindSFCInputType = BindSFCInputType & {
	bindSFCs?: any[]
}

/**
 * 换绑输入
 */
export type SwitchBindInputType = BindSFCInputType & {
	/**
	 * 旧设备ID
	 */
	oldBindId : string

	/**
	 * 旧绑定的SFC
	 */
	oldBindSFC : string

	/**
	 * 新绑定的SFC
	 */
	newBindSFC : string
}

/**
 * 复投输入
 */
export type RepeatInputType = BindSFCInputType & {
	/**
	 * NG位置
	 */
	nGLocationId : string

	/**
	 * 复投位置
	 */
	repeatLocationId : string
}