import { store } from '@/store'
import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
		name: 'Home',
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
        component: () => import('@/components/auth/SignInForm.vue'),
      },
      {
        path: 'signup',
				name: 'AuthSignUp',
        component: () => import('@/components/auth/SignUpForm.vue'),
      }
		]
  },
  {
    path: '/profile',
		name: 'Profile',
    component: () => import('@/views/Profile.vue'),
		meta: { requiresAuth: true },
    children: [
      {
        path: '',
				name: 'ProfileIndex',
        component: () => import('@/components/profile/MainSection.vue'),
      },
      {
        path: 'settings',
				name: 'ProfileSettings',
        component: () => import('@/components/profile/SettingsSection.vue'),
      },
			{
				path: 'data',
				name: 'ProfileData',
				component: () => import('@/components/profile/DataSection.vue')
			}
    ]
  },
	{
		path: '/workspace/:id',
		name: 'Workspace',
		props: true,
		meta: { requiresAuth: true },
		component: () => import('@/views/Workspace.vue')
	},
  {
    path: '/:pathMatch(.*)*',
		name: 'NotFound',
    component: () => import('@/views/NotFound.vue')
  }
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes
})

router.beforeResolve(async to => {
	let isLoggedIn: boolean = store.getters['user/isLoggedIn'];
	if (!isLoggedIn) {
		isLoggedIn = await store.dispatch('user/init');
	}
  if (to.meta.requiresAuth && !isLoggedIn) {
		store.commit('pageError');
		return {
			name: 'Auth',
			query: { redirect: to.fullPath },
		};
	} else if ((to.name === 'AuthSignIn' || to.name === 'AuthSignUp') && isLoggedIn) {
		return {
			name: 'ProfileIndex'
		}
	}
	store.commit('pageReady');
})

export default router
