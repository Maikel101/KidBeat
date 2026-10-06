import { createApiClient } from './apiClient'
import type { LoginUsuarioDto } from '@/types/usuario'

const apiClient = createApiClient()

export async function login(email: string, password: string): Promise<string> {
  const body: LoginUsuarioDto = { Email: email, Password: password }
  return apiClient.post<string>('/api/Usuario/login', body)
}
