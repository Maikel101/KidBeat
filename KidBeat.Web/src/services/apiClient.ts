export interface ApiClientOptions {
  baseUrl?: string
  getToken?: () => string | null
}

export class ApiError extends Error {
  readonly status: number

  constructor(status: number, message: string) {
    super(message)
    this.name = 'ApiError'
    this.status = status
  }
}

function resolveBaseUrl(baseUrl?: string): string {
  const url = baseUrl ?? (import.meta.env.VITE_API_BASE_URL as string | undefined)
  if (!url) {
    throw new Error('VITE_API_BASE_URL no está configurada.')
  }
  return url
}

function joinUrl(baseUrl: string, path: string): string {
  return `${baseUrl.replace(/\/+$/, '')}/${path.replace(/^\/+/, '')}`
}

async function parseBody(response: Response): Promise<unknown> {
  const text = await response.text()
  if (!text) {
    return undefined
  }
  try {
    return JSON.parse(text)
  } catch {
    return text
  }
}

function toErrorMessage(payload: unknown, status: number): string {
  if (typeof payload === 'string' && payload) {
    return payload
  }
  if (status === 401) {
    return 'No autorizado.'
  }
  if (status === 403) {
    return 'Acceso denegado.'
  }
  if (status >= 500) {
    return 'Error interno del servidor.'
  }
  return `La petición falló con el estado ${status}.`
}

export function createApiClient(options: ApiClientOptions = {}) {
  const { getToken } = options

  async function request<T>(method: string, path: string, body?: unknown): Promise<T> {
    const headers = new Headers()
    headers.set('Content-Type', 'application/json')

    const token = getToken?.()
    if (token) {
      headers.set('Authorization', `Bearer ${token}`)
    }

    const url = joinUrl(resolveBaseUrl(options.baseUrl), path)

    const requestInit: RequestInit = {
      method,
      headers,
    }
    if (body !== undefined) {
      requestInit.body = JSON.stringify(body)
    }

    let response: Response
    try {
      response = await fetch(url, requestInit)
    } catch {
      throw new ApiError(0, 'No se pudo conectar con el servidor.')
    }

    const payload = await parseBody(response)

    if (!response.ok) {
      throw new ApiError(response.status, toErrorMessage(payload, response.status))
    }

    if (response.status === 204) {
      return undefined as T
    }

    return payload as T
  }

  return {
    get: <T>(path: string) => request<T>('GET', path),
    post: <T>(path: string, body?: unknown) => request<T>('POST', path, body),
    put: <T>(path: string, body?: unknown) => request<T>('PUT', path, body),
    delete: <T>(path: string) => request<T>('DELETE', path),
  }
}
