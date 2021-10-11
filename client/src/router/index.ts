import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    component: () => import('@/views/Home.vue')
  },
  {
    path: '/auth',
    component: () => import('@/views/Auth.vue')
  },
  {
    path: '/profile',
    component: () => import('@/views/Profile.vue'),
    children: [
      {
        path: '',
        component: () => import('@/components/ProfileMain.vue'),
      },
      {
        path: 'settings',
        component: () => import('@/components/ProfileSettings.vue'),
      }
    ]
  },
  {
    path: '/:pathMatch(.*)*',
    component: () => import('@/views/NotFound.vue')
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
