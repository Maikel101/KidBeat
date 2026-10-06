import { createRouter, createWebHistory } from 'vue-router'

import { useSesionStore } from '@/stores/sesion'
import HomeView from '@/views/HomeView.vue'
import LoginView from '@/views/LoginView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { path: '/', name: 'home', component: HomeView },
    { path: '/login', name: 'login', component: LoginView },
  ],
})

router.beforeEach((to) => {
  const sesionStore = useSesionStore()
  if (to.path === '/login' && sesionStore.isAuthenticated) {
    return { path: '/' }
  }
})

export default router
