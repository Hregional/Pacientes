# Datos Pacientes API

API RESTful desarrollada en .NET 10 para proporcionar acceso rápido, seguro y moderno a la información de los pacientes registrados en el sistema *legacy* del Hospital Regional de Occidente.

Este servicio provee una capa de abstracción intermedia para realizar búsquedas (por Historia Clínica, DPI, Nombres o Fecha de Nacimiento) directamente contra la base de datos instalada actual, asegurando los *endpoints* mediante autenticación JWT (integrado con Keycloak).

---

## 🚀 Tecnologías Principales
*   **.NET 10** (ASP.NET Core Web API)
*   **Entity Framework Core**
*   **Mapster** (Mapeo de DTOs)
*   **Keycloak** (Autenticación / JWT)
*   **Docker & Docker Compose**

---

## 🐳 Despliegue y Uso con Docker (Recomendado)

La forma más sencilla de ejecutar esta API es mediante contenedores Docker, ya que aísla las dependencias y maneja variables de entorno cruciales.

### 1. Variables de entorno necesarias
Antes de ejecutar Docker Compose, asegúrate de configurar las variables de entorno para que los contenedores puedan inyectar la cadena de conexión de la base de datos y la configuración de Keycloak.

Crea un archivo llamado `.env` en la raíz del backend (o donde corras docker-compose) con los siguientes valores (ajustando tus datos reales):

```env
ASPNETCORE_ENVIRONMENT=Production
DB_CONNECTION_STRING=Server=tu-servidor;Database=RecepcionV2;User Id=tu-usuario;Password=tu-password;TrustServerCertificate=True;
KEYCLOAK_AUTHORITY=https://tu-servidor-keycloak/realms/tu-realm
KEYCLOAK_AUDIENCE=tu-audience
KEYCLOAK_CLIENTID=tu-client-id
KEYCLOAK_CLIENTSECRET=tu-client-secret
KEYCLOAK_REQUIREHTTPSMETADATA=true
```

### 2. Levantar el servicio
Abre una terminal, navega a la carpeta principal donde se aloja el archivo `docker-compose.yml` (`src/DatosPacientes/`) y ejecuta:

```bash
docker-compose up -d --build
```

Esto descargará (o compilará) las imágenes del entorno .NET 10, y el API quedará vinculada al puerto **8081** de tu máquina.

### 3. Probar el API
Una vez iniciada, visita la documentación autogenerada para ver y probar los métodos:
* **Swagger UI:** `http://localhost:8081/swagger` (si está habilitado para el ambiente configurado).
* Todos los *endpoints* se encuentran bajo el prefijo `/api/Busqueda/...` y requieren un Header con Token `Bearer`.

### Detener el servicio
Para bajar y limpiar los contenedores, redes y volúmenes montados (sin borrar el volumen `dp-keys`), ejecuta en el mismo directorio:

```bash
docker-compose down
```

---

## 💻 Desarrollo Local (Sin Docker)
Si necesitas desarrollar o hacer *debug* desde Visual Studio:

1. Clona el repositorio: `C:\Users\aajucum\source\repos\Pacientes\`.
2. Dirígete la solución o proyecto principal `DatosPacientes.sln` / `DatosPacientes.csproj`.
3. Configura las cadenas de conexión local y la información de Keycloak dentro de tus `Secrets.json` o `appsettings.Development.json`.
4. Corre el proyecto usando F5 o el comando `dotnet run`.