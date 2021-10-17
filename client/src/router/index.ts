import { useStore } from '@/store'
import { useCookie } from 'vue-cookie-next'
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

router.beforeEach((to) => {
	const store = useStore();
  if (to.meta.requiresAuth && !store.state.isLoggedIn) {
		console.log(store.state);
		const toAuth = {
			name: 'Auth',
			query: { redirect: to.fullPath },
		};
		const cookie = useCookie();
		const isRemembered: string | null = cookie.getCookie('remembered');
		if (isRemembered === '1') {
			store.dispatch('refresh')
			.catch((error) => {
				console.log(error);
				return toAuth;
			});
		} else {
			return toAuth;
		}
  }
})

export default router
