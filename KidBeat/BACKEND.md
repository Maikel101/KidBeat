# KidBeat — Backend

Documentación técnica del backend de **KidBeat**, una aplicación web orientada a descubrir, consultar y aportar información sobre parques infantiles.

---

## 1. Tecnologías

* **.NET 10 LTS**
* **ASP.NET Core Web API**
* **C#**
* **Entity Framework Core 10**
* **SQL Server**
* **JWT (JSON Web Token)** para autenticación
* **Swagger / OpenAPI** para documentación y pruebas de la API
* **Visual Studio 2022**

---

## 2. Arquitectura

KidBeat está desarrollado como una **aplicación monolítica con arquitectura por capas**.

La estructura separa las principales responsabilidades del backend:

```text
┌──────────────────────────────┐
│        Controllers           │
│       Presentación / API     │
└──────────────┬───────────────┘
               │
               ▼
┌──────────────────────────────┐
│          Services            │
│       Lógica de negocio      │
└──────────────┬───────────────┘
               │
               ▼
┌──────────────────────────────┐
│      EF Core / DbContext     │
│      Acceso a datos          │
└──────────────┬───────────────┘
               │
               ▼
┌──────────────────────────────┐
│         SQL Server           │
│        Persistencia          │
└──────────────────────────────┘
```

El frontend se comunica con el backend mediante una **API REST utilizando HTTP y JSON**.

```text
Vue 3 + TypeScript
        │
        │ HTTP / JSON
        ▼
ASP.NET Core Web API
        │
        ▼
   EF Core
        │
        ▼
   SQL Server
```

---

## 3. Estructura del proyecto

```text
KidBeat.Api/
│
├── Controllers/
│   └── Endpoints HTTP de la API
│
├── DTOs/
│   └── Contratos de entrada y salida de la API
│
├── Services/
│   └── Lógica de negocio
│
├── Data/
│   ├── KidBeatDbContext.cs
│   └── Configurations/
│       └── Configuración de entidades mediante Fluent API
│
├── Enums/
│   └── Enumeraciones utilizadas por el dominio
│
├── Models/
│   └── Entidades persistidas en la base de datos
│
├── Migrations/
│   └── Evolución del esquema de base de datos mediante EF Core
│
├── Program.cs
└── appsettings.json
```

---

## 4. Modelos principales

El backend utiliza entidades que representan la información principal de KidBeat:

* **Usuario** — usuarios registrados.
* **Parque** — parques disponibles en la aplicación.
* **Imagen** — fotografías asociadas a los parques.
* **Comentario** — comentarios realizados por los usuarios.
* **Valoracion** — puntuaciones de los usuarios sobre los parques.
* **Visita** — registro de visitas a la aplicación.

### Estado de un parque

Los parques utilizan el enum `EstadoParque`:

```text
Pendiente
Publicado
Rechazado
```

Esto permite controlar el flujo de moderación de los parques aportados por los usuarios.

---

## 5. DTOs

Los endpoints no exponen directamente las entidades de Entity Framework Core.

Se utilizan **DTOs (Data Transfer Objects)** para definir los contratos de entrada y salida de la API.

Ejemplos:

```text
DTOs/
├── Usuario/
├── Parque/
├── Imagen/
├── Comentario/
├── Valoracion/
└── Visita/
```

Los DTOs permiten:

* controlar qué información recibe la API;
* controlar qué información devuelve;
* aplicar validaciones de entrada;
* evitar exponer directamente las entidades de persistencia;
* desacoplar el contrato de la API de la estructura interna de la base de datos.

---

## 6. Servicios

La lógica de negocio está separada de los Controllers mediante servicios.

Ejemplo:

```text
Services/
└── Parque/
    ├── IParqueService.cs
    └── ParqueService.cs
```

El mismo patrón se utiliza para los demás recursos.

Los Controllers se encargan principalmente de:

* recibir las peticiones HTTP;
* obtener los datos de autenticación;
* llamar al servicio correspondiente;
* devolver el código HTTP adecuado.

Los Services contienen la lógica de negocio y utilizan `KidBeatDbContext` para acceder a los datos.

---

## 7. Entity Framework Core

La persistencia utiliza **Entity Framework Core con Code First**.

El contexto principal es:

```text
KidBeatDbContext
```

El contexto contiene los `DbSet` correspondientes a las entidades:

```text
Usuarios
Parques
Imagenes
Comentarios
Valoraciones
Visitas
```

Las configuraciones de las entidades se mantienen separadas mediante `IEntityTypeConfiguration<T>` y Fluent API.

Esto permite definir de forma explícita:

* claves primarias;
* claves foráneas;
* relaciones;
* restricciones;
* índices;
* longitudes máximas;
* comportamiento de borrado;
* restricciones de unicidad.

---

## 8. Reglas de integridad y negocio

Algunas reglas se controlan tanto desde la lógica de negocio como desde la base de datos.

### Una valoración por usuario y parque

Un usuario solamente puede realizar una valoración para un parque.

Se utiliza:

* comprobación en el Service;
* índice único en SQL Server sobre `UsuarioId + ParqueId`.

Si se intenta crear una valoración duplicada, la API responde:

```text
409 Conflict
```

### Una imagen principal por parque

Cada parque puede tener como máximo una imagen marcada como principal.

Se utiliza un índice único filtrado sobre `ParqueId` para las imágenes principales.

Si el parque ya tiene una imagen principal, la API responde:

```text
409 Conflict
```

Estas restricciones mantienen la integridad incluso ante situaciones de concurrencia.

---

## 9. Autenticación

La autenticación utiliza **JWT (JSON Web Token)**.

Flujo simplificado:

```text
Usuario
   │
   │ POST /api/Usuario/login
   ▼
Backend
   │
   │ valida credenciales
   ▼
JWT
   │
   │ Authorization: Bearer <token>
   ▼
Endpoint protegido
   │
   ▼
ASP.NET Core valida el token
```

El middleware de autenticación valida:

* firma del token;
* caducidad;
* clave de firma.

Los endpoints que requieren autenticación utilizan:

```csharp
[Authorize]
```

Los datos del usuario autenticado se obtienen mediante los claims del JWT.

---

## 10. Autorización

La autenticación determina **quién es el usuario**.

La autorización determina **qué puede hacer ese usuario**.

Por ejemplo, un usuario autenticado puede modificar o eliminar únicamente sus propios comentarios, imágenes y valoraciones.

Cuando un usuario intenta modificar un recurso perteneciente a otro usuario, la API responde:

```text
403 Forbidden
```

---

## 11. Contraseñas

Las contraseñas no se almacenan directamente en la base de datos.

Se utiliza `PasswordHasher<Usuario>` para generar y verificar el hash de las contraseñas.

La base de datos almacena el valor derivado:

```text
PasswordHash
```

y no la contraseña original.

---

## 12. Inyección de dependencias

ASP.NET Core utiliza **Dependency Injection (DI)** para registrar y proporcionar los servicios necesarios.

Los servicios de KidBeat se registran con ciclo de vida `Scoped`.

Ejemplo:

```csharp
builder.Services.AddScoped<IParqueService, ParqueService>();
```

Esto permite que los Controllers dependan de interfaces en lugar de crear directamente las implementaciones.

---

## 13. API REST

La API utiliza recursos y métodos HTTP estándar:

| Método | Uso                 |
| ------ | ------------------- |
| GET    | Obtener información |
| POST   | Crear recursos      |
| PUT    | Actualizar recursos |
| DELETE | Eliminar recursos   |

Los endpoints devuelven códigos HTTP adecuados según el resultado de la operación.

Ejemplos:

```text
200 OK
201 Created
204 No Content
401 Unauthorized
403 Forbidden
404 Not Found
409 Conflict
```

---

## 14. Swagger / OpenAPI

La API dispone de documentación interactiva mediante **Swagger / OpenAPI**.

Swagger permite:

* consultar los endpoints disponibles;
* conocer los modelos de entrada y salida;
* ejecutar peticiones;
* probar autenticación mediante JWT;
* comprobar los códigos de respuesta.

En entorno de desarrollo se encuentra disponible mediante:

```text
/swagger
```

---

## 15. Migraciones

La base de datos se gestiona mediante **EF Core Code First Migrations**.

Las migraciones permiten evolucionar el esquema de SQL Server a medida que cambia el modelo del backend.

```text
Modelos
   │
   ▼
EF Core Migration
   │
   ▼
SQL Server
```

Las migraciones se almacenan en:

```text
Migrations/
```

---

## 16. Visitas

El backend registra visitas mediante la entidad `Visita`.

Una visita puede estar asociada a:

* un usuario autenticado;
* ningún usuario, cuando la visita es anónima.

Por tanto, el registro de visitas soporta ambos escenarios.

---

## 17. Validación y respuestas

Los DTOs utilizan validaciones mediante Data Annotations cuando corresponde.

Ejemplos:

```csharp
[Required]
[MaxLength(...)]
[Range(...)]
```

Las respuestas de los Controllers se adaptan al resultado de la operación para utilizar códigos HTTP semánticamente adecuados.

---

## 18. Pruebas realizadas

Durante el desarrollo se han probado los principales escenarios de la API mediante Swagger:

* registro de usuarios;
* login;
* generación y validación de JWT;
* acceso a endpoints protegidos;
* CRUD de parques;
* creación, modificación y eliminación de comentarios;
* autorización sobre recursos pertenecientes a otros usuarios;
* creación y eliminación de imágenes;
* restricción de imagen principal duplicada;
* creación y eliminación de valoraciones;
* restricción de valoración duplicada;
* visitas anónimas;
* visitas autenticadas;
* respuestas `401`, `403`, `404` y `409`.

---

## 19. Seguridad

La clave utilizada para firmar los JWT se gestiona mediante **ASP.NET Core User Secrets** durante el desarrollo y no se almacena en el código fuente.

Las credenciales y secretos de desarrollo no forman parte del repositorio.

El repositorio no contiene claves JWT, contraseñas ni otras credenciales sensibles.

---

## 20. Objetivo de la arquitectura

La arquitectura busca mantener una separación clara de responsabilidades.

Actualmente el proyecto utiliza:

```text
Controllers
     ↓
Services
     ↓
DbContext / EF Core
     ↓
SQL Server
```

Esta estructura permite mantener el código organizado y facilita la evolución futura del proyecto, manteniendo una base sencilla y comprensible para una aplicación monolítica.
