import { createRouter, createWebHistory } from 'vue-router'
const routes = [
  {
    path: '/',
    name: 'Home',
    component: () => import('@/views/Home.vue')
  },
  {
    path: '/signin',
    name: 'Sign In',
    component: () => import('@/views/SignIn.vue')
  },
  {
    path: '/signup',
    name: 'Sign Up',
    component: () => import('@/views/SignUp.vue')
  },
  {
    path: '/:pathMatch(.*)*',
    name: 'Not Found',
    component: () => import('@/views/NotFound.vue')
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
