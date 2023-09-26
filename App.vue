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
			this.init()

			// #ifdef APP-PLUS
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
			loginRememberHandle() {
				const _loginRemember = getCache('login-remember')
				if (_loginRemember && _loginRemember.data) {} else {
					delCache('login-remember')
					delCache('token')
				}
			},
			init() {
				const appStore = useAppStore()
				appStore.loadSystemInfo()
				
				this.loginRememberHandle()
			},
			initApp() {
				plus.screen.lockOrientation('portrait-primary')
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