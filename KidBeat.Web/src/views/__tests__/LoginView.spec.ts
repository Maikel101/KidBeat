import { flushPromises, mount, type VueWrapper } from '@vue/test-utils'
import { beforeEach, describe, expect, it, vi } from 'vitest'
import { createRouter, createWebHistory, type Router } from 'vue-router'

import { ApiError } from '@/services/apiClient'
import HomeView from '@/views/HomeView.vue'
import LoginView from '../LoginView.vue'

const { mockLogin } = vi.hoisted(() => ({
  mockLogin: vi.fn<(email: string, password: string) => Promise<void>>(),
}))

vi.mock('@/stores/sesion', () => ({
  useSesionStore: () => ({ login: mockLogin }),
}))

function createTestRouter(): Router {
  return createRouter({
    history: createWebHistory(),
    routes: [
      { path: '/', component: HomeView },
      { path: '/login', component: LoginView },
    ],
  })
}

async function mountLogin(): Promise<VueWrapper> {
  const router = createTestRouter()
  await router.push('/login')
  await router.isReady()

  return mount(LoginView, { global: { plugins: [router] } })
}

async function fillAndSubmit(wrapper: VueWrapper, email: string, password: string) {
  await wrapper.find('input[type="email"]').setValue(email)
  await wrapper.find('input[type="password"]').setValue(password)
  await wrapper.find('form').trigger('submit')
  await flushPromises()
}

describe('LoginView', () => {
  beforeEach(() => {
    mockLogin.mockReset()
  })

  it('renderiza el formulario de login', async () => {
    const wrapper = await mountLogin()

    expect(wrapper.find('input[type="email"]').exists()).toBe(true)
    expect(wrapper.find('input[type="password"]').exists()).toBe(true)
    expect(wrapper.find('button[type="submit"]').text()).toBe('Iniciar sesión')
  })

  it('envía las credenciales y navega a / al iniciar sesión', async () => {
    mockLogin.mockResolvedValue(undefined)
    const router = createTestRouter()
    await router.push('/login')
    await router.isReady()
    const wrapper = mount(LoginView, { global: { plugins: [router] } })

    await fillAndSubmit(wrapper, 'usuario@example.com', 'password123')

    expect(mockLogin).toHaveBeenCalledWith('usuario@example.com', 'password123')
    expect(router.currentRoute.value.path).toBe('/')
  })

  it('muestra error y no navega cuando el login falla con 401', async () => {
    mockLogin.mockRejectedValue(new ApiError(401, 'No autorizado.'))
    const wrapper = await mountLogin()

    await fillAndSubmit(wrapper, 'usuario@example.com', 'incorrecta')

    expect(wrapper.text()).toContain('Email o contraseña incorrectos.')
  })

  it('deshabilita el botón mientras se envía la petición', async () => {
    let resolver!: () => void
    mockLogin.mockImplementation(() => new Promise<void>((resolve) => (resolver = resolve)))
    const wrapper = await mountLogin()

    await wrapper.find('input[type="email"]').setValue('usuario@example.com')
    await wrapper.find('input[type="password"]').setValue('password123')
    await wrapper.find('form').trigger('submit')
    await flushPromises()

    const button = wrapper.find('button[type="submit"]')
    expect(button.attributes('disabled')).toBeDefined()

    resolver()
    await flushPromises()

    expect(button.attributes('disabled')).toBeUndefined()
  })

  it('no envía la petición si los campos están vacíos', async () => {
    const wrapper = await mountLogin()

    await wrapper.find('form').trigger('submit')
    await flushPromises()

    expect(mockLogin).not.toHaveBeenCalled()
    expect(wrapper.text()).toContain('Introduce el email y la contraseña.')
  })

  it('no envía la petición si el email no es válido', async () => {
    const wrapper = await mountLogin()

    await fillAndSubmit(wrapper, 'email-invalido', 'password123')

    expect(mockLogin).not.toHaveBeenCalled()
    expect(wrapper.text()).toContain('Introduce un email válido.')
  })
})
