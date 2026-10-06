<script setup lang="ts">
import { useRouter } from 'vue-router'

import { useSesionStore } from '@/stores/sesion'

const router = useRouter()
const sesionStore = useSesionStore()

function cerrarSesion() {
  sesionStore.logout()
  router.push({ path: '/login' })
}
</script>

<template>
  <main class="home">
    <h1>KidBeat</h1>
    <p>Página de inicio provisional. El desarrollo continúa.</p>
    <button type="button" @click="cerrarSesion">Cerrar sesión</button>
  </main>
</template>

<style scoped>
.home {
  max-width: 40rem;
  margin: 4rem auto;
  padding: 0 1rem;
}
</style>
