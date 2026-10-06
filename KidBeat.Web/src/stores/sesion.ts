import { computed, ref } from 'vue'
import { defineStore } from 'pinia'

import { login as loginRequest } from '@/services/usuario'
import { clearToken, getToken, setToken } from '@/services/token'

export const useSesionStore = defineStore('sesion', () => {
  const token = ref<string | null>(getToken())

  const isAuthenticated = computed(() => token.value !== null)

  async function login(email: string, password: string) {
    const nuevoToken = await loginRequest(email, password)
    token.value = nuevoToken
    setToken(nuevoToken)
  }

  function logout() {
    token.value = null
    clearToken()
  }

  return { token, isAuthenticated, login, logout }
})
