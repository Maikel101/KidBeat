<script setup lang="ts">
import { onMounted, ref } from 'vue'

import { ApiError } from '@/services/apiClient'
import { getParques } from '@/services/parque'
import type { ParqueDto } from '@/types/parque'

const parques = ref<ParqueDto[]>([])
const isLoading = ref(true)
const errorMessage = ref('')

const caracteristicas: Array<{ campo: keyof ParqueDto; etiqueta: string }> = [
  { campo: 'TieneBanios', etiqueta: 'Baños' },
  { campo: 'TieneZonaInfantil', etiqueta: 'Zona infantil' },
  { campo: 'EsAccesible', etiqueta: 'Accesible' },
  { campo: 'TieneZonasSombra', etiqueta: 'Sombras' },
]

function toErrorMessage(error: unknown): string {
  if (error instanceof ApiError) {
    if (error.status === 0) {
      return 'No se pudo conectar con el servidor.'
    }
    if (error.status >= 500) {
      return 'Error del servidor. Inténtalo de nuevo.'
    }
    return error.message
  }
  return 'Error inesperado. Inténtalo de nuevo.'
}

async function cargarParques() {
  isLoading.value = true
  errorMessage.value = ''
  try {
    parques.value = await getParques()
  } catch (error) {
    errorMessage.value = toErrorMessage(error)
  } finally {
    isLoading.value = false
  }
}

onMounted(cargarParques)
</script>

<template>
  <main class="home">
    <h1 class="home__title">KidBeat</h1>

    <p v-if="isLoading" class="home__status" role="status">Cargando parques...</p>

    <div v-else-if="errorMessage" class="home__error" role="alert">
      <p>{{ errorMessage }}</p>
      <button class="home__retry" type="button" @click="cargarParques">Reintentar</button>
    </div>

    <p v-else-if="parques.length === 0" class="home__status">Aún no hay parques publicados.</p>

    <ul v-else class="parques">
      <li v-for="parque in parques" :key="parque.Id" class="parque">
        <h2 class="parque__nombre">{{ parque.Nombre }}</h2>
        <p class="parque__ciudad">{{ parque.Ciudad }}</p>
        <p class="parque__descripcion">{{ parque.Descripcion }}</p>
        <ul class="parque__caracteristicas">
          <li v-for="caracteristica in caracteristicas" :key="caracteristica.campo">
            <template v-if="parque[caracteristica.campo]">{{ caracteristica.etiqueta }}</template>
          </li>
        </ul>
      </li>
    </ul>
  </main>
</template>

<style scoped>
.home {
  max-width: 50rem;
  margin: 0 auto;
  padding: 2rem 1rem;
}

.parques {
  list-style: none;
  margin: 0 0 0 -1rem;
  padding: 0;
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
  width: 100%;
  box-sizing: border-box;
}

.parque {
  border: 1px solid #ccc;
  border-radius: 0.5rem;
  padding: 1rem;
  box-sizing: border-box;
  width: 100%;
}

.parque__nombre {
  margin: 0;
}

.parque__ciudad {
  color: #555;
  margin: 0.25rem 0 0.5rem;
}

.parque__descripcion {
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
  margin: 0 0 0.75rem;
}

.parque__caracteristicas {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
  list-style: none;
  margin: 0;
  padding: 0;
}

.parque__caracteristicas li {
  background: #f0f0f0;
  border-radius: 1rem;
  padding: 0.25rem 0.75rem;
}
</style>
