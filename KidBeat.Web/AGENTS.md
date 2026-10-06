# AGENTS.md — KidBeat.Web

## 1. Propósito y alcance

Guía persistente para OpenCode antes de implementar funcionalidades en el frontend.

Este documento es específico del frontend `KidBeat.Web` (Vue 3 + TypeScript + Vite).

El backend (`KidBeat/`) es la autoridad de las reglas de negocio, autorización, validación y persistencia. Su documentación técnica se encuentra en `BACKEND.md` y en Swagger/OpenAPI.

No existe, por ahora, un AGENTS.md en la raíz del repositorio.

## 2. Arquitectura y estructura de carpetas

Frontend SPA monolítica que consume una API REST mediante HTTP/JSON. El frontend nunca accede directamente a SQL Server.

Estructura objetivo de `src/`:

```
src/
  components/    # componentes reutilizables
  views/         # páginas asociadas a rutas
  services/      # única capa que realiza llamadas HTTP por dominio
  types/         # contratos TypeScript agrupados por dominio
  composables/   # lógica reutilizable no global (solo si se necesita)
  stores/        # estado global compartido (Pinia, solo cuando sea necesario)
  router/        # configuración de rutas
  __tests__/     # pruebas unitarias
```

Las carpetas se crean de forma incremental, solo cuando haya una funcionalidad que las necesite. No crear carpetas vacías ni estructuras que no se utilicen.

La arquitectura debe mantenerse sencilla y mantenible, evitando la sobreingeniería.

## 3. Convenciones Vue y TypeScript

- Composition API con `<script setup lang="ts">`.
- Componentes pequeños y reutilizables cuando tenga sentido.
- Las vistas representan páginas asociadas a rutas.
- Evitar `any` salvo que exista una justificación clara.
- Respetar `noUncheckedIndexedAccess` (definido en `tsconfig.app.json`).
- Usar el alias `@/` → `src/` para imports.
- Formato según `.prettierrc.json`: sin semicolons, single quotes, `printWidth: 100`.

## 4. Comunicación con la API

- Los `services/` (uno por dominio) son la única capa del frontend que realiza llamadas HTTP a la API.
- Las **views y components no realizan llamadas HTTP directamente**; consumen services (o stores cuando proceda).
- Los DTOs/tipos TypeScript en `types/` se escriben a mano replicando los contratos del backend y se comprueban contra `BACKEND.md`, Swagger/OpenAPI y el código del backend.
- **Prohibido inventar** endpoints, propiedades, códigos HTTP o contratos de la API.
- Cuando se implemente la comunicación real con la API, se creará un `apiClient` centralizado (configuración de base URL y manejo común de peticiones y respuestas).
- CORS y base URL de la API: decisión pendiente; se resolverá en el momento de la integración frontend/backend (CORS en ASP.NET Core vs. proxy de desarrollo de Vite).

## 5. Estado y Pinia

- Pinia únicamente para estado realmente compartido/global (por ejemplo, sesión).
- Estado local con refs locales dentro del componente o composable.
- Usar setup stores.

## 6. Routing

- Vue Router con `createWebHistory`.
- Una vista por ruta en `views/`.
- Guards de ruta si el dominio lo requiere (por ejemplo, autenticación) cuando exista.

## 7. Validación y gestión de errores

- El backend es la autoridad de validación; el frontend valida únicamente lo necesario para la experiencia de usuario.
- Manejo de errores HTTP sin dejar errores sin tratar:
  - `401` → fin de sesión.
  - `4xx` → mensajes al usuario.
  - `5xx` → tratamiento genérico.
- Estrategia de almacenamiento del JWT: decisión pendiente; se decidirá antes de implementar el login del frontend. No fijar localStorage/sessionStorage/memoria + Pinia como regla.

## 8. Tests y controles de calidad

- Vitest + Vue Test Utils.
- Tests en `src/**/__tests__/`.
- Comandos de verificación obligatorios al terminar una tarea:
  - `npm run type-check`
  - `npm run lint`
  - `npm run test:unit`
  - `npm run format`

## 9. Gestión de dependencias

- Gestor de paquetes: `npm` con `package-lock.json`.
- No instalar dependencias sin confirmación previa y sin especificar el motivo.
- Antes de añadir una dependencia, verificar si ya existe alguna que cubra la necesidad.
- No añadir por ahora herramientas de generación automática de tipos (los contratos TypeScript se escriben manualmente).

## 10. Reglas de uso de OpenCode

- Trabajar dentro de `KidBeat.Web`.
- Seguir el flujo de trabajo definido en la sección 14.
- No modificar el backend.
- No inventar contratos de API.
- Preguntar ante cualquier ambigüedad en lugar de asumir.
- No tomar decisiones adicionales importantes sin indicarlas antes al usuario.

## 11. Reglas de Git

- Git lo gestiona el usuario mediante GitExtensions.
- OpenCode no hará commit, push, pull, merge, ni creará ramas.
- OpenCode no ejecutará operaciones Git destructivas.
- OpenCode solo puede consultar el estado o el historial de Git si es necesario.

## 12. Seguridad

- No exponer ni loguear secretos o tokens.
- No guardar credenciales ni datos sensibles en el código ni en la documentación.

## 13. Documentación

- Mantener actualizado el README del frontend.
- Reflejar en AGENTS.md las decisiones de arquitectura y convenciones del frontend.
- Actualizar AGENTS.md cuando cambien convenciones, dependencias o estructura.

## 14. Flujo de trabajo (obligatorio)

Cada tarea debe seguir esta secuencia:

1. **Requisito**: entender qué se pide.
2. **Análisis**: consultar el código existente, `BACKEND.md` y Swagger/OpenAPI.
3. **Plan**: proponer el enfoque antes de implementar.
4. **Aprobación**: esperar la aprobación del usuario antes de modificar archivos.
5. **Implementación**: escribir el código según estas convenciones.
6. **Pruebas**: ejecutar `npm run type-check`, `npm run lint`, `npm run test:unit` y `npm run format`.
7. **Verificación**: confirmar el resultado y resumir lo realizado.