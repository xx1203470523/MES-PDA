<script>
	import {
		getCache,
		delCache
	} from '@/utils/cache-utils'

	import {
		useAppStore
	} from '@/store/app'

	export default {
		onLaunch: function() {
			console.warn('当前组件仅支持 uni_modules 目录结构 ，请升级 HBuilderX 到 3.1.0 版本以上！')
			console.log('App Launch')

			this.init()
			
			// #ifdef APP-PLUS

			// 初始化应用
			this.initApp()

			// #endif
		},
		onShow: function() {
			console.log('App Show')
		},
		onHide: function() {
			console.log('App Hide')
		},
		methods: {
			init() {
				const _loginRemember = getCache('login-remember')
				if (_loginRemember && _loginRemember.data) {} else {
					delCache('login-remember')
					delCache('token')
				}
			},
			initApp() {
				
				plus.screen.lockOrientation('portrait-primary')
				
				plus.runtime.getProperty(plus.runtime.appid, (wgtinfo) => {
					//把版本信息存进 Store app.ts			
				const appStore = useAppStore()				
				appStore.setversionCode(wgtinfo.versionCode)
				})
			}
		}
	}
</script>

<style lang="scss">
	/*每个页面公共css */
	@import '@/uni_modules/uni-scss/index.scss';
	/* #ifndef APP-NVUE */
	@import '@/static/icons/customicons.css';

	// 设置整个项目的背景色
	page {
		background-color: #f5f5f5;
	}

	/* #endif */
	.example-info {
		font-size: 14px;
		color: #333;
		padding: 10px;
	}
</style>