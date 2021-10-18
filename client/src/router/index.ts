import { store } from '@/store'
import { VueCookieNext } from 'vue-cookie-next'
import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
		name: 'Index',
    component: () => import('@/views/Home.vue')
  },
  {
    path: '/auth',
		name: 'Auth',
    component: () => import('@/views/Auth.vue')
  },
  {
    path: '/profile',
    component: () => import('@/views/Profile.vue'),
		meta: { requiresAuth: true },
    children: [
      {
        path: '',
				name: 'ProfileIndex',
        component: () => import('@/components/ProfileMain.vue'),
      },
      {
        path: 'settings',
				name: 'ProfileSettings',
        component: () => import('@/components/ProfileSettings.vue'),
      },
			{
				path: 'password',
				name: 'ProfilePassword',
				component: () => import('@/components/ProfilePassword.vue')
			},
			{
				path: 'email',
				name: 'ProfileEmail',
				component: () => import('@/components/ProfileEmail.vue')
			}
    ]
  },
  {
    path: '/:pathMatch(.*)*',
		name: 'NotFound',
    component: () => import('@/views/NotFound.vue')
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

router.beforeEach(async (to) => {
  if (to.meta.requiresAuth && !store.getters.isLoggedIn) {
		console.log(store.state);
		const toAuth = {
			name: 'Auth',
			query: { redirect: to.fullPath },
		};
		const isRemembered: string | null = VueCookieNext.getCookie('remembered');
		if (isRemembered === '1') {
			try {
				await store.dispatch('refresh')
				return true;
			} catch (e) {
				return toAuth;
			}
		}
		return toAuth;
	}
	return true;
})

export default router
