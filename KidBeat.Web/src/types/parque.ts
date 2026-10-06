export type EstadoParque = 0 | 1 | 2

export interface ParqueDto {
  Id: number
  Nombre: string
  Descripcion: string
  Direccion: string
  Ciudad: string
  Latitud: number | null
  Longitud: number | null
  TieneBanios: boolean
  TieneZonaInfantil: boolean
  EsAccesible: boolean
  TieneZonasSombra: boolean
  EstadoParque: EstadoParque
  FechaAlta: string
}
