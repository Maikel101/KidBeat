<script setup lang="ts">
import { computed, ref } from 'vue'
import { useRouter } from 'vue-router'

import { ApiError } from '@/services/apiClient'
import { useSesionStore } from '@/stores/sesion'

const router = useRouter()
const sesionStore = useSesionStore()

const email = ref('')
const password = ref('')
const errorMessage = ref('')
const isSubmitting = ref(false)

const isEmailValid = computed(() => /^\S+@\S+\.\S+$/.test(email.value.trim()))

function toErrorMessage(error: unknown): string {
  if (error instanceof ApiError) {
    if (error.status === 401) {
      return 'Email o contraseña incorrectos.'
    }
    if (error.status === 0) {
      return 'No se pudo conectar con el servidor.'
    }
    return error.message
  }
  return 'Error inesperado. Inténtalo de nuevo.'
}

async function handleSubmit() {
  errorMessage.value = ''

  if (!email.value.trim() || !password.value) {
    errorMessage.value = 'Introduce el email y la contraseña.'
    return
  }

  if (!isEmailValid.value) {
    errorMessage.value = 'Introduce un email válido.'
    return
  }

  isSubmitting.value = true
  try {
    await sesionStore.login(email.value.trim(), password.value)
    await router.push({ path: '/' })
  } catch (error) {
    errorMessage.value = toErrorMessage(error)
  } finally {
    isSubmitting.value = false
  }
}
</script>

<template>
  <main class="login">
    <h1 class="login__title">KidBeat</h1>

    <form class="login__form" novalidate @submit.prevent="handleSubmit">
      <div class="login__field">
        <label for="email">Email</label>
        <input id="email" v-model="email" name="email" type="email" autocomplete="email" />
      </div>

      <div class="login__field">
        <label for="password">Contraseña</label>
        <input
          id="password"
          v-model="password"
          name="password"
          type="password"
          autocomplete="current-password"
        />
      </div>

      <p v-if="errorMessage" class="login__error" role="alert">{{ errorMessage }}</p>

      <button class="login__submit" type="submit" :disabled="isSubmitting">
        {{ isSubmitting ? 'Iniciando sesión...' : 'Iniciar sesión' }}
      </button>
    </form>
  </main>
</template>

<style scoped>
.login {
  max-width: 22rem;
  margin: 4rem auto;
  padding: 0 1rem;
}

.login__form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.login__field {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

.login__field input {
  padding: 0.5rem;
}

.login__error {
  color: #b00020;
  margin: 0;
}

.login__submit {
  padding: 0.5rem;
}
</style>
