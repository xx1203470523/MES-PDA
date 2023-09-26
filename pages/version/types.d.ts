import type { versionOutputType } from '@/api/modules/user-center/version-types'

export type PageType = {
	windowInfo ?: UniNamespace.GetWindowInfoResult
	handle : {
		fileName : string
		isDownload : boolean
		progress : number
		progressTitle : string
	}
	result : {
		info ?: versionOutputType
		content ?: string
	}
}