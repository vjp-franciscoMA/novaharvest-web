# NovaHarvest

## Arranque

### Con Docker

Desde la carpeta raíz del proyecto, ejecutar el siguiente comando para construir las imágenes y arrancar los contenedores:

```bash
docker compose up -d --build
```

Para detener los contenedores y eliminar los recursos creados por Docker Compose:

```bash
docker compose down
```

Una vez iniciados los servicios, se puede acceder a la aplicación desde el navegador mediante las siguientes direcciones:

- **Inicio:** http://localhost:8080/
- **Acerca de:** http://localhost:8080/about
- **Estado del servidor:** http://localhost:8080/status
- **Hoja de estilos:** http://localhost:8080/css/site.css
- **Logotipo:** http://localhost:8080/img/logo.png

### Sin Docker

Para ejecutar el backend directamente con ASP.NET Core, entrar en la carpeta del proyecto:

```bash
cd backend/NovaHarvest.Web
```

Ejecutar la aplicación con:

```bash
dotnet run
```

ASP.NET Core mostrará en la terminal la dirección local en la que se está ejecutando la aplicación. El puerto puede variar según la configuración de cada equipo.

Utilizando la dirección que aparezca en la terminal, se puede acceder a las siguientes rutas:

- **Inicio:** `/`
- **Acerca de:** `/about`
- **Estado del servidor:** `/status`
- **Hoja de estilos:** `/css/site.css`
- **Logotipo:** `/img/logo.png`

Por ejemplo, si la terminal muestra `http://localhost:5079`, la página de inicio estará disponible en `http://localhost:5079/`.

## Arquitectura

El proyecto utiliza dos servicios cuando se ejecuta con Docker: Nginx y el backend de ASP.NET Core.

El navegador envía las peticiones a Nginx, que actúa como proxy inverso y las reenvía al backend. ASP.NET Core, mediante Kestrel, genera las páginas HTML y sirve los archivos estáticos, como la hoja de estilos y el logotipo.

```text
Navegador
    |
    | HTTP: localhost:8080
    v
Nginx
Puerto 8080 del equipo -> puerto 80 del contenedor
    |
    | Proxy inverso
    | http://backend:8080
    v
ASP.NET Core (Kestrel)
Puerto interno 8080
    |
    +-- Página de inicio (/)
    +-- Acerca de (/about)
    +-- Estado del servidor (/status)
    +-- CSS (/css/site.css)
    +-- Logotipo (/img/logo.png)
```

El servicio web publica el puerto 8080 del equipo y lo conecta con el puerto 80 del contenedor de Nginx. Nginx reenvía las peticiones al backend utilizando el nombre del servicio `backend` y el puerto 8080 dentro de la red de Docker.

El backend no publica ningún puerto directamente al equipo, ya que las peticiones externas pasan primero por Nginx.

Cuando se ejecuta sin Docker mediante `dotnet run`, el navegador se conecta directamente al servidor Kestrel, sin pasar por Nginx. En este caso, se utiliza la dirección y el puerto que ASP.NET Core muestra en la terminal.