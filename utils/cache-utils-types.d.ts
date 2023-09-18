export type CacheKeyType = 'token' | 'login-remember'

export type CacheOptionType = {
	key : CacheKeyType,
	value : any,
	expired ?: {
		time ?: number,
		unit ?: 'day' | 'hours' | 'minutes'
	}
}

export type CacheResultType = {
	data : any,
	expired : number,
	msg : string
}