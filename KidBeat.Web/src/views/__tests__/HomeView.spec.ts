import { flushPromises, mount, type VueWrapper } from '@vue/test-utils'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'

import { ApiError } from '@/services/apiClient'
import type { ParqueDto } from '@/types/parque'
import HomeView from '../HomeView.vue'

const { mockGetParques } = vi.hoisted(() => ({
  mockGetParques: vi.fn<() => Promise<ParqueDto[]>>(),
}))

vi.mock('@/services/parque', () => ({
  getParques: mockGetParques,
}))

function parque(id: number, nombre: string): ParqueDto {
  return {
    Id: id,
    Nombre: nombre,
    Descripcion: 'Descripción de ejemplo',
    Direccion: 'Calle de ejemplo 1',
    Ciudad: 'Madrid',
    Latitud: null,
    Longitud: null,
    TieneBanios: true,
    TieneZonaInfantil: true,
    EsAccesible: false,
    TieneZonasSombra: false,
    EstadoParque: 1,
    FechaAlta: '2026-01-01T00:00:00Z',
  }
}

async function mountHome(): Promise<VueWrapper> {
  return mount(HomeView)
}

beforeEach(() => {
  mockGetParques.mockReset()
})

afterEach(() => {
  vi.restoreAllMocks()
})

describe('HomeView', () => {
  it('muestra el estado de carga mientras resuelve la petición', async () => {
    let resolveParques!: (value: ParqueDto[]) => void
    mockGetParques.mockImplementation(
      () => new Promise<ParqueDto[]>((resolve) => (resolveParques = resolve)),
    )
    const wrapper = await mountHome()

    expect(wrapper.text()).toContain('Cargando parques...')

    resolveParques([parque(1, 'Parque Central')])
    await flushPromises()

    expect(wrapper.text()).not.toContain('Cargando parques...')
  })

  it('muestra los parques devueltos por el service', async () => {
    mockGetParques.mockResolvedValue([parque(1, 'Parque Central'), parque(2, 'Parque del Retiro')])
    const wrapper = await mountHome()
    await flushPromises()

    expect(wrapper.text()).toContain('Parque Central')
    expect(wrapper.text()).toContain('Parque del Retiro')
    expect(wrapper.text()).toContain('Madrid')
    expect(wrapper.text()).toContain('Baños')
    expect(wrapper.text()).toContain('Zona infantil')
  })

  it('muestra el estado vacío cuando no hay parques', async () => {
    mockGetParques.mockResolvedValue([])
    const wrapper = await mountHome()
    await flushPromises()

    expect(wrapper.text()).toContain('Aún no hay parques publicados.')
  })

  it('muestra un mensaje de error cuando la petición falla', async () => {
    mockGetParques.mockRejectedValue(new ApiError(500, 'Error interno del servidor.'))
    const wrapper = await mountHome()
    await flushPromises()

    expect(wrapper.text()).toContain('Error del servidor. Inténtalo de nuevo.')
    expect(wrapper.find('.home__retry').exists()).toBe(true)
  })

  it('reintenta la carga y muestra los parques tras recuperarse', async () => {
    mockGetParques
      .mockRejectedValueOnce(new ApiError(500, 'Error interno del servidor.'))
      .mockResolvedValueOnce([parque(3, 'Parque Renovado')])
    const wrapper = await mountHome()
    await flushPromises()

    expect(wrapper.text()).toContain('Error del servidor. Inténtalo de nuevo.')

    await wrapper.find('.home__retry').trigger('click')
    await flushPromises()

    expect(mockGetParques).toHaveBeenCalledTimes(2)
    expect(wrapper.text()).toContain('Parque Renovado')
  })
})
