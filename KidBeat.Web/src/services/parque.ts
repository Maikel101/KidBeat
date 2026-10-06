import { createApiClient } from './apiClient'
import type { ParqueDto } from '@/types/parque'

const apiClient = createApiClient()

export async function getParques(): Promise<ParqueDto[]> {
  return apiClient.get<ParqueDto[]>('/api/Parque')
}

export async function getParque(id: number): Promise<ParqueDto> {
  return apiClient.get<ParqueDto>(`/api/Parque/${id}`)
}
