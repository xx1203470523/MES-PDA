/**
 * 提示音播报工具
 */
import type { voiceType } from './voice-utils-types'
/*
 * 播报提醒声音对象
 */
let innerAudioContext = uni.createInnerAudioContext()

/**音乐地址 */
export const getVoiceUrl = (_voiceType ?: voiceType) => {
	let voiceUrl = '/static/voice/An-Ok.wav'
	switch (_voiceType) {
		case 'errorVoicePDA':
			voiceUrl = '/static/voice/An-No.wav'
			break
		case 'successVoicePDA':
			voiceUrl = '/static/voice/An-Ok.wav'
			break
	}
	return voiceUrl
}

/**
 * 播放报警音
 * @param _voiceType
 * @returns
 */
export const startPlay = (_voiceType ?: voiceType) => {
	//PDA端播报提示音
	//#ifdef APP
	if (innerAudioContext != undefined) {
		innerAudioContext.stop()
		innerAudioContext.destroy() //销毁对象
	}
	innerAudioContext = uni.createInnerAudioContext() //创建对象
	//音乐地址
	innerAudioContext.src = getVoiceUrl(_voiceType)
	innerAudioContext.play() //播放
	innerAudioContext.onError(function () {
		innerAudioContext.destroy() //异常卡住后销毁对象
	})
	innerAudioContext.onPlay(function () {
		console.log(innerAudioContext.src)
	})
	// #endif

	//H5端播报提示音
	// #ifndef APP
	innerAudioContext.currentTime = 0 //从头开始播放提示音
	innerAudioContext.autoplay = true
	innerAudioContext.src = getVoiceUrl(_voiceType)
	innerAudioContext.onError(function () {
		innerAudioContext.stop()
	})
	// #endif
}