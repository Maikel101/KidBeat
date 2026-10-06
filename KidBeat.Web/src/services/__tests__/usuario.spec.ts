import { afterEach, describe, expect, it, vi } from 'vitest'

import { login } from '../usuario'

const baseUrl = 'https://api.kidbeat.test'

function mockFetchOnce(response: Response) {
  const fetchMock = vi.fn<typeof fetch>().mockResolvedValue(response)
  vi.stubGlobal('fetch', fetchMock)
  return fetchMock
}

function fetchCall() {
  const calls = vi.mocked(fetch).mock.calls
  const call = calls[calls.length - 1]
  return { url: String(call?.[0]), init: call?.[1] as RequestInit | undefined }
}

afterEach(() => {
  vi.unstubAllGlobals()
  vi.unstubAllEnvs()
})

describe('services/usuario', () => {
  it('devuelve el token cuando el login es correcto', async () => {
    vi.stubEnv('VITE_API_BASE_URL', baseUrl)
    mockFetchOnce(new Response(JSON.stringify('jwt-token'), { status: 200 }))

    const token = await login('usuario@example.com', 'password123')

    expect(token).toBe('jwt-token')
    expect(fetchCall().url).toBe(`${baseUrl}/api/Usuario/login`)
    expect(fetchCall().init?.method).toBe('POST')
    expect(JSON.parse(String(fetchCall().init?.body))).toEqual({
      Email: 'usuario@example.com',
      Password: 'password123',
    })
  })

  it('lanza ApiError con estado 401 para credenciales incorrectas', async () => {
    vi.stubEnv('VITE_API_BASE_URL', baseUrl)
    mockFetchOnce(new Response(null, { status: 401 }))

    await expect(login('usuario@example.com', 'incorrecta')).rejects.toMatchObject({ status: 401 })
  })

  it('lanza ApiError para errores 5xx', async () => {
    vi.stubEnv('VITE_API_BASE_URL', baseUrl)
    mockFetchOnce(new Response(null, { status: 500 }))

    await expect(login('usuario@example.com', 'password123')).rejects.toMatchObject({
      status: 500,
    })
  })
})
