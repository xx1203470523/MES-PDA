export type userInfoType = {
	permissions ?: any,
	roles ?: Array<string>,
	user? : {
		avatar ?: string
		createdOn ?: string
		deptId ?: string
		deptName ?: null
		email ?: string
		id ?: string
		loginDate ?: string
		loginIP ?: string
		nickName ?: string
		phonenumber ?: string
		userName ?: string
	}
}

export type resetPasswordType = {
	oldpassword : string
	newpassword : string
}


export type updateuserinfotype = {
	nickname : string
	phonenumber : string
	email : string
}