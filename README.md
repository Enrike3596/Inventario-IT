
# Inventario-TI API

Este es el backend para una aplicación de gestión de inventario de TI, diseñada para centralizar y administrar los activos tecnológicos de una empresa.

## Descripción General

La API de Inventario-TI proporciona una solución robusta para el seguimiento de hardware, software, asignaciones a usuarios, órdenes de compra y el ciclo de vida completo de los activos de TI dentro de la organización. Su objetivo es optimizar la gestión de recursos, mejorar la eficiencia operativa y proporcionar visibilidad clara sobre el inventario tecnológico.

### Necesidad que Resuelve

En muchas empresas, la gestión de activos de TI se realiza de forma manual o con herramientas descentralizadas, lo que provoca:
- Falta de visibilidad sobre los activos disponibles.
- Dificultad para rastrear asignaciones y devoluciones.
- Procesos de compra y baja de activos ineficientes.
- Riesgos de seguridad por activos no controlados.

Esta API centraliza toda la información y los procesos, solucionando estos problemas y proporcionando una única fuente de verdad para el inventario de TI.

## Estructura del Backend

El proyecto sigue una arquitectura por capas, separando las responsabilidades para mantener un código limpio, escalable y fácil de mantener.

-   **`Controllers/`**: Expone los endpoints de la API. Recibe las peticiones HTTP y las dirige a los servicios correspondientes.
-   **`Services/`**: Contiene la lógica de negocio principal. Orquesta las operaciones interactuando con los repositorios.
-   **`Repositories/`**: Se encarga del acceso y la manipulación de los datos en la base de datos a través de Entity Framework Core.
-   **`Models/`**: Define las entidades de la base de datos (ej. `Activos`, `Usuarios`, `AsignacionUsuario`).
-   **`DTOs/`**: (Data Transfer Objects) Define los objetos que se utilizan para transferir datos entre el cliente y el servidor, asegurando que solo se exponga la información necesaria.
-   **`Data/`**: Contiene `AppDbContext`, la clase principal de Entity Framework Core que representa la sesión con la base de datos.
-   **`Migrations/`**: Almacena las migraciones de la base de datos generadas por Entity Framework Core.
-   **`Helpers/`**: Clases de utilidad para tareas comunes, como el manejo de contraseñas.

## Instalación y Configuración

Sigue estos pasos para configurar y ejecutar el proyecto en tu entorno local.

### Prerrequisitos

-   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) o superior.
-   [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) con la carga de trabajo "ASP.NET and web development".
-   O [Visual Studio Code](https://code.visualstudio.com/) con la extensión [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit).
-   Un motor de base de datos como SQL Server, PostgreSQL o SQLite.

### Pasos de Instalación

1.  **Clonar el repositorio:**
    ```bash
    git clone https://github.com/Enrike3596/Inventario-IT.git
    cd Inventario-IT/Inventario-TI.API
    ```

2.  **Configurar la conexión a la base de datos:**
    -   Abre el archivo `appsettings.Development.json`.
    -   Modifica la cadena de conexión `DefaultConnection` para apuntar a tu base de datos.

    **Ejemplo para SQL Server:**
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=InventarioDB;Trusted_Connection=True;MultipleActiveResultSets=true"
    }
    ```

3.  **Aplicar las migraciones:**
    Abre una terminal en la raíz del proyecto (`Inventario-TI.API`) y ejecuta el siguiente comando para crear la base de datos y aplicar el esquema.
    ```bash
    dotnet ef database update
    ```

### Ejecutar en Visual Studio 2022

1.  Abre el archivo `HelpDesk.API.csproj` con Visual Studio 2022.
2.  Asegúrate de que `HelpDesk.API` esté seleccionado como proyecto de inicio.
3.  Presiona `F5` o el botón de "Play" para iniciar la aplicación en modo de depuración.

### Ejecutar en Visual Studio Code

1.  Abre la carpeta `Inventario-TI.API` en VS Code.
2.  Abre la terminal integrada (`Ctrl + ñ`).
3.  Ejecuta el siguiente comando para iniciar la aplicación:
    ```bash
    dotnet watch run
    ```
    El comando `watch` reiniciará automáticamente la aplicación cada vez que realices un cambio en el código.

## Tecnologías Utilizadas

-   **ASP.NET Core 8**: Framework para construir la API web.
-   **Entity Framework Core 8**: ORM para la interacción con la base de datos.
-   **SQL Server / PostgreSQL / SQLite**: Sistema de gestión de base de datos.
-   **JWT (JSON Web Tokens)**: Para la autenticación y autorización.
-   **AutoMapper**: Para mapear entre entidades y DTOs (si se implementa).
-   **Swagger/OpenAPI**: Para la documentación de la API.
