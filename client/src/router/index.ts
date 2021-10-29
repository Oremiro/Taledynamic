import { store } from '@/store'
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
		redirect: () => ({ name: 'AuthSignIn'}),
    component: () => import('@/views/Auth.vue'),
		children: [
			{
        path: 'signin',
				name: 'AuthSignIn',
        component: () => import('@/components/SignInForm.vue'),
      },
      {
        path: 'signup',
				name: 'AuthSignUp',
        component: () => import('@/components/SignUpForm.vue'),
      },
		]
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
				path: 'edit',
				name: 'ProfileEdit',
				component: () => import('@/components/ProfileEdit.vue')
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

router.beforeResolve(async to => {
	let isLoggedIn: boolean = store.getters.isLoggedIn;
	if (!isLoggedIn) {
		isLoggedIn = await store.dispatch('init');
	}
  if (to.meta.requiresAuth && !isLoggedIn) {
		store.commit('pageError');
		return {
			name: 'Auth',
			query: { redirect: to.fullPath },
		};
	}
	store.commit('pageReady');
})

export default router
