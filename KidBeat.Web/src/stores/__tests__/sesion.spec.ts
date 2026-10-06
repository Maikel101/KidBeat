import { beforeEach, describe, expect, it, vi } from 'vitest'
import { createPinia, setActivePinia } from 'pinia'

import { TOKEN_KEY } from '@/services/token'
import { useSesionStore } from '../sesion'

const mockLoginRequest = vi.hoisted(() =>
  vi.fn<(email: string, password: string) => Promise<string>>(),
)

vi.mock('@/services/usuario', () => ({
  login: mockLoginRequest,
}))

describe('sesion store', () => {
  beforeEach(() => {
    setActivePinia(createPinia())
    sessionStorage.clear()
    mockLoginRequest.mockReset()
  })

  it('empieza sin sesión cuando no hay token almacenado', () => {
    const store = useSesionStore()

    expect(store.isAuthenticated).toBe(false)
    expect(store.token).toBeNull()
  })

  it('inicializa la sesión desde sessionStorage', () => {
    sessionStorage.setItem(TOKEN_KEY, 'stored-token')

    const store = useSesionStore()

    expect(store.isAuthenticated).toBe(true)
    expect(store.token).toBe('stored-token')
  })

  it('login almacena el token y activa la sesión', async () => {
    mockLoginRequest.mockResolvedValue('jwt-token')
    const store = useSesionStore()

    await store.login('usuario@example.com', 'password123')

    expect(store.token).toBe('jwt-token')
    expect(store.isAuthenticated).toBe(true)
    expect(sessionStorage.getItem(TOKEN_KEY)).toBe('jwt-token')
  })

  it('login fallido no deja sesión activa', async () => {
    mockLoginRequest.mockRejectedValue(new Error('fallo'))
    const store = useSesionStore()

    await expect(store.login('usuario@example.com', 'incorrecta')).rejects.toThrow('fallo')

    expect(store.isAuthenticated).toBe(false)
    expect(sessionStorage.getItem(TOKEN_KEY)).toBeNull()
  })

  it('logout limpia el token y cierra la sesión', async () => {
    mockLoginRequest.mockResolvedValue('jwt-token')
    const store = useSesionStore()
    await store.login('usuario@example.com', 'password123')

    store.logout()

    expect(store.token).toBeNull()
    expect(store.isAuthenticated).toBe(false)
    expect(sessionStorage.getItem(TOKEN_KEY)).toBeNull()
  })
})
