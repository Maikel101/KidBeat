import { afterEach, describe, expect, it } from 'vitest'

import { TOKEN_KEY, clearToken, getToken, setToken } from '../token'

afterEach(() => {
  sessionStorage.clear()
})

describe('token', () => {
  it('almacena y recupera el token en sessionStorage', () => {
    setToken('jwt-value')

    expect(getToken()).toBe('jwt-value')
  })

  it('devuelve null si no hay token almacenado', () => {
    expect(getToken()).toBeNull()
  })

  it('elimina el token', () => {
    setToken('jwt-value')

    clearToken()

    expect(getToken()).toBeNull()
    expect(sessionStorage.getItem(TOKEN_KEY)).toBeNull()
  })
})
