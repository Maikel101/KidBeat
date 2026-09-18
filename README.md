
# KidBeat

Aplicación web orientada a descubrir, consultar y aportar información sobre parques infantiles.

El objetivo de KidBeat es facilitar a las familias la búsqueda de parques y permitir que los usuarios contribuyan con valoraciones, comentarios e imágenes.

## Tecnologías

### Backend

* **.NET 10 LTS**
* **ASP.NET Core Web API**
* **C#**
* **Entity Framework Core 10**
* **SQL Server**
* **JWT (JSON Web Token)**
* **Swagger / OpenAPI**

### Frontend

* **Vue 3**
* **TypeScript**

## Arquitectura

KidBeat está desarrollado como una **aplicación monolítica con arquitectura por capas**.

```text
Vue 3 + TypeScript
        │
        │ HTTP / JSON
        ▼
ASP.NET Core Web API
        │
        ▼
    Services
        │
        ▼
   EF Core / DbContext
        │
        ▼
    SQL Server
```

El backend separa las responsabilidades principales entre **Controllers, Services, DTOs, Data, Models y Migrations**.

La documentación técnica detallada del backend se encuentra en:

**[BACKEND.md](BACKEND.md)**

## Funcionalidades

Actualmente el backend incluye:

* Registro de usuarios.
* Inicio de sesión mediante JWT.
* Autenticación y autorización.
* Gestión de parques.
* Comentarios de usuarios.
* Valoraciones de parques.
* Imágenes asociadas a parques.
* Imagen principal por parque.
* Registro de visitas anónimas y autenticadas.
* Validación de datos mediante DTOs.
* Restricciones de integridad en base de datos.
* Documentación y pruebas mediante Swagger / OpenAPI.

## Seguridad

Las contraseñas se almacenan mediante hash utilizando `PasswordHasher`.

Las claves utilizadas para JWT se gestionan mediante **ASP.NET Core User Secrets** durante el desarrollo y no forman parte del repositorio.

## Estado del proyecto

**En desarrollo.**

KidBeat se está construyendo progresivamente, comenzando por el backend y su API REST antes de completar la integración con el frontend.

## Objetivo

La idea principal de KidBeat se resume en:

**Descubrir → Consultar → Decidir → Aportar**
