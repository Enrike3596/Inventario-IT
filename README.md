
# HelpDesk.API — Inventario IT

Backend para la gestión de inventario de activos tecnológicos, diseñado para centralizar y administrar el ciclo de vida completo de los recursos de TI de una organización.

## Descripción General

La API expone un sistema completo de inventario que cubre desde la compra y registro de activos hasta su asignación, salida, historial de movimientos y baja. Proporciona autenticación JWT, manejo centralizado de errores y respuestas estandarizadas.

### Módulos del Sistema

| Módulo | Endpoints | Descripción |
|---|---|---|
| **Auth** | `POST /api/auth/login`, `GET /api/auth/me`, `POST /api/auth/forgot-password`, `POST /api/auth/reset-password` | Autenticación, perfil del usuario actual y restablecimiento de contraseña |
| **Usuarios** | CRUD `api/usuarios` + `GET /correo/{correo}` | Gestión de usuarios con roles y sedes |
| **Roles** | CRUD `api/roles` | Roles del sistema (ej. Administrador, Técnico, Usuario) |
| **Sedes** | CRUD `api/sedes` | Sedes o ubicaciones de la empresa |
| **Categorías de Activo** | CRUD `api/categoriasactivo` | Categorías para clasificar activos |
| **Órdenes de Compra** | CRUD `api/ordenescompra` | Registro de compras de activos |
| **Activos** | CRUD `api/activos` + `GET /serial/{serial}` | Inventario de activos tecnológicos |
| **Asignaciones** | CRUD `api/asignacionesusuario` + `PATCH /{id}/desactivar` | Asignación de activos a usuarios |
| **Salidas** | CRUD `api/salidas` | Salidas de activos (con detalle y generación de código único) |
| **Detalles de Salida** | `GET /api/detallessalida/salida/{idSalida}`, `GET /api/detallessalida/{id}` | Consulta de líneas de una salida |
| **Canales** | CRUD `api/canales` | Canales de solicitud de salidas |
| **Parqueaderos** | CRUD `api/parqueaderos` | Parqueaderos asociados a sedes |
| **Historial de Activos** | `GET /api/historialactivo`, `GET /api/historialactivo/activo/{idActivo}` | Trazabilidad de movimientos por activo |

## Arquitectura

El proyecto sigue una arquitectura por capas con separación clara de responsabilidades:

```
Controllers (REST endpoints, respuestas estandarizadas con ResponseHelper)
    ↓
Services (lógica de negocio, mapeo Model <-> DTO)
    ↓
Repositories (acceso a datos con EF Core, lógica de borrado lógico/físico)
    ↓
AppDbContext (Entity Framework Core — DbContext con configuraciones)
    ↓
PostgreSQL (base de datos relacional vía Npgsql)
```

Capas transversales:

- **Middleware/ExceptionMiddleware.cs** — Manejo global de excepciones con respuestas ProblemDetails (RFC 7807)
- **Helpers/PasswordHelper.cs** — Hashing de contraseñas con PBKDF2 (SHA-256, 100k iteraciones)
- **Helpers/ResponseHelper.cs** — Envoltorio de respuesta JSON estandarizado: `{ exito, data, mensaje, errores }`
- **Enums/** — Enumeraciones almacenadas como strings en base de datos

## Tecnologías

- **.NET 10** — Framework base
- **ASP.NET Core 10** — API web
- **Entity Framework Core 10** — ORM
- **PostgreSQL** — Base de datos (vía Npgsql)
- **JWT (JSON Web Tokens)** — Autenticación y autorización
- **OpenAPI (Swagger)** — Documentación interactiva de la API
- **CORS** — Política configurada para frontends en `localhost:5173`, `localhost:5174` y `localhost:8080`

## Modelo de Datos

### Entidades Principales

```
Roles ──── Usuarios ──────── Salidas (como usuario entrega)
            │                        └── Salidas (como usuario destino)
            └── AsignacionUsuario
                  └── Activos ──── CategoriaActivo
                  │               └── OrdenCompra
                  └── Parqueadero ─── Sedes
                                    └── Usuarios (por sede)
Salidas ──── DetalleSalida ──── Activos
    ├── Canal
    ├── Parqueadero (destino)
    └── HistorialActivo ──── Activos
                            └── Salidas
```

### Enumeraciones

| Enum | Valores |
|---|---|
| `EstadoActivo` | `Disponible`, `Asignado`, `EnMantenimiento`, `DadoDeBaja` |
| `EstadoAsignacion` | `Activa`, `Finalizada` |
| `EstadoGenerico` | `Activo`, `Inactivo` |
| `EstadoUsuario` | `Activo`, `Inactivo` |
| `TipoMovimiento` | `Entrada`, `Salida`, `Asignacion`, `Devolucion` |

### Estrategia de Borrado

- **Borrado lógico** (cambia estado a `Inactivo` o `DadoDeBaja`): Activos, Usuarios, Sedes, Roles, Categorías, Parqueaderos
- **Borrado físico**: Órdenes de Compra, Canales, Asignaciones, Salidas

## Instalación y Configuración

### Prerrequisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) o [VS Code](https://code.visualstudio.com/) con C# Dev Kit
- PostgreSQL (local o remoto)

### Pasos

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/Enrike3596/Inventario-IT.git
   cd Inventario-IT
   ```

2. Configurar la conexión a PostgreSQL en `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5432;Database=InventarioTI;Username=postgres;Password=tu_password"
   }
   ```

3. Aplicar migraciones y crear la base de datos:
   ```bash
   dotnet ef database update
   ```

4. Ejecutar la aplicación:
   ```bash
   dotnet run
   ```
   La API estará disponible en `http://localhost:5176`.  
   Con Swagger: `http://localhost:5176/openapi/v1.json`

### Ejecutar con Hot Reload

```bash
dotnet watch run
```

## Endpoints de la API

### Autenticación
| Método | Ruta | Descripción |
|---|---|---|
| `POST` | `/api/auth/login` | Iniciar sesión (devuelve JWT) |
| `GET` | `/api/auth/me` | Obtener usuario autenticado |
| `POST` | `/api/auth/forgot-password` | Solicitar restablecimiento de contraseña |
| `POST` | `/api/auth/reset-password` | Restablecer contraseña con token |

### CRUD Completo
Los siguientes módulos exponen `GET /`, `GET /{id}`, `POST /`, `PUT /{id}`, `DELETE /{id}`:

| Recurso | Ruta base |
|---|---|
| Usuarios | `/api/usuarios` |
| Roles | `/api/roles` |
| Sedes | `/api/sedes` |
| Categorías de Activo | `/api/categoriasactivo` |
| Órdenes de Compra | `/api/ordenescompra` |
| Activos | `/api/activos` |
| Asignaciones | `/api/asignacionesusuario` |
| Salidas | `/api/salidas` |
| Canales | `/api/canales` |
| Parqueaderos | `/api/parqueaderos` |

### Consultas Específicas
| Método | Ruta | Descripción |
|---|---|---|
| `GET` | `/api/activos/serial/{serial}` | Buscar activo por número de serie |
| `GET` | `/api/usuarios/correo/{correo}` | Buscar usuario por correo |
| `PATCH` | `/api/asignacionesusuario/{id}/desactivar` | Finalizar asignación |
| `GET` | `/api/detallessalida/salida/{idSalida}` | Detalles de una salida |
| `GET` | `/api/historialactivo/activo/{idActivo}` | Historial de un activo |

## Respuestas de la API

Todas las respuestas siguen un formato estandarizado:

```json
{
  "exito": true,
  "data": { ... },
  "mensaje": "Operación exitosa"
}
```

En caso de error:
```json
{
  "exito": false,
  "mensaje": "Descripción del error",
  "errores": { "campo": ["Error de validación"] }
}
```

Los errores no controlados retornan ProblemDetails (RFC 7807) con `traceId` para depuración.
