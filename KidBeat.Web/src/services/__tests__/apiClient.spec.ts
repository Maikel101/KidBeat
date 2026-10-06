import { afterEach, describe, expect, it, vi } from 'vitest'

import { ApiError, createApiClient } from '../apiClient'

const baseUrl = 'https://api.kidbeat.test'

function mockFetchOnce(response: Response) {
  const fetchMock = vi.fn<typeof fetch>().mockResolvedValue(response)
  vi.stubGlobal('fetch', fetchMock)
  return fetchMock
}

afterEach(() => {
  vi.unstubAllGlobals()
  vi.unstubAllEnvs()
})

describe('apiClient', () => {
  it('realiza una petición GET usando la base URL y el path', async () => {
    const parque = { Id: 1, Nombre: 'Parque Central' }
    const fetchMock = mockFetchOnce(
      new Response(JSON.stringify(parque), {
        status: 200,
        headers: { 'Content-Type': 'application/json' },
      }),
    )

    const apiClient = createApiClient({ baseUrl })
    const result = await apiClient.get<{ Id: number; Nombre: string }>('/api/Parque/1')

    expect(fetchMock).toHaveBeenCalledWith(`${baseUrl}/api/Parque/1`, expect.any(Object))
    expect(result).toEqual(parque)
  })

  it('lee la base URL de VITE_API_BASE_URL cuando no se pasa en opciones', async () => {
    vi.stubEnv('VITE_API_BASE_URL', baseUrl)
    mockFetchOnce(new Response('[]', { status: 200 }))

    const apiClient = createApiClient()
    await apiClient.get('/api/Parque')

    expect(fetch).toHaveBeenCalledWith(`${baseUrl}/api/Parque`, expect.any(Object))
  })

  it('falla con un mensaje claro si no hay base URL configurada', async () => {
    vi.stubEnv('VITE_API_BASE_URL', undefined)
    const apiClient = createApiClient()

    await expect(apiClient.get('/api/Parque')).rejects.toThrow(
      'VITE_API_BASE_URL no está configurada.',
    )
  })

  it('establece Content-Type application/json', async () => {
    mockFetchOnce(new Response('{}', { status: 200 }))

    const apiClient = createApiClient({ baseUrl })
    await apiClient.get('/api/Parque')

    const requestInit = vi.mocked(fetch).mock.calls[0]?.[1] as RequestInit
    const headers = new Headers(requestInit.headers)
    expect(headers.get('Content-Type')).toBe('application/json')
  })

  it('añade Authorization Bearer cuando el proveedor devuelve token', async () => {
    mockFetchOnce(new Response('{}', { status: 200 }))

    const apiClient = createApiClient({ baseUrl, getToken: () => 'token-123' })
    await apiClient.get('/api/Parque')

    const requestInit = vi.mocked(fetch).mock.calls[0]?.[1] as RequestInit
    const headers = new Headers(requestInit.headers)
    expect(headers.get('Authorization')).toBe('Bearer token-123')
  })

  it('no añade Authorization cuando no hay token', async () => {
    mockFetchOnce(new Response('{}', { status: 200 }))

    const apiClient = createApiClient({ baseUrl })
    await apiClient.get('/api/Parque')

    const requestInit = vi.mocked(fetch).mock.calls[0]?.[1] as RequestInit
    const headers = new Headers(requestInit.headers)
    expect(headers.get('Authorization')).toBeNull()
  })

  it('serializa el cuerpo en JSON en POST', async () => {
    mockFetchOnce(new Response('{}', { status: 201 }))

    const apiClient = createApiClient({ baseUrl })
    const body = { Nombre: 'Nuevo parque' }
    await apiClient.post('/api/Parque', body)

    const requestInit = vi.mocked(fetch).mock.calls[0]?.[1] as RequestInit
    expect(requestInit.method).toBe('POST')
    expect(requestInit.body).toBe(JSON.stringify(body))
  })

  it('devuelve undefined para respuestas 204', async () => {
    mockFetchOnce(new Response(null, { status: 204 }))

    const apiClient = createApiClient({ baseUrl })
    const result = await apiClient.delete('/api/Parque/1')

    expect(result).toBeUndefined()
  })

  it('lanza ApiError con el status para errores 4xx', async () => {
    mockFetchOnce(new Response(null, { status: 404 }))

    const apiClient = createApiClient({ baseUrl })

    await expect(apiClient.get('/api/Parque/999')).rejects.toBeInstanceOf(ApiError)
    await expect(apiClient.get('/api/Parque/999')).rejects.toMatchObject({ status: 404 })
  })

  it('lanza ApiError con estado 401 para respuestas no autorizadas', async () => {
    mockFetchOnce(new Response(null, { status: 401 }))

    const apiClient = createApiClient({ baseUrl })

    await expect(apiClient.get('/api/Parque')).rejects.toMatchObject({ status: 401 })
  })

  it('lanza ApiError para errores 5xx', async () => {
    mockFetchOnce(new Response(null, { status: 500 }))

    const apiClient = createApiClient({ baseUrl })

    await expect(apiClient.get('/api/Parque')).rejects.toMatchObject({ status: 500 })
  })

  it('usa el cuerpo como mensaje cuando la API responde una cadena (409)', async () => {
    mockFetchOnce(new Response(JSON.stringify('El parque ya existe.'), { status: 409 }))

    const apiClient = createApiClient({ baseUrl })

    await expect(apiClient.post('/api/Parque', {})).rejects.toThrow('El parque ya existe.')
  })

  it('lanza ApiError de conexión si fetch falla', async () => {
    vi.stubGlobal(
      'fetch',
      vi.fn<typeof fetch>().mockRejectedValue(new TypeError('Failed to fetch')),
    )

    const apiClient = createApiClient({ baseUrl })

    await expect(apiClient.get('/api/Parque')).rejects.toMatchObject({ status: 0 })
  })
})
