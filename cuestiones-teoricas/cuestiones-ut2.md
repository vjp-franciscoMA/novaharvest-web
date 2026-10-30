# Cuestiones UT2 – NovaHarvest

## 1. Cliente y servidor

Cuando abro la página `/status`, el navegador actúa como cliente y envía una petición al servidor. Primero llega a Nginx, que se encarga de pasarla al backend mediante el proxy inverso. Después, ASP.NET Core ejecuta el código C# que genera la página con la fecha y la hora del servidor. Finalmente, devuelve el contenido al navegador para que se muestre. Si en el futuro se pueden reservar productos, la comprobación de que no se reserven más kilos de los disponibles debe hacerse en el servidor, porque no podemos fiarnos de que el cliente no modifique los datos.

## 2. Estático o dinámico

En NovaHarvest, las páginas de inicio y acerca de podrían ser HTML estático, porque su contenido no cambia continuamente. En cambio, la página `/status` es dinámica porque muestra la fecha y la hora actuales del servidor cada vez que se visita. En el futuro también necesitaremos páginas dinámicas, por ejemplo, una para consultar los kilos disponibles de cada producto y otra para realizar reservas. Estas páginas necesitan consultar y actualizar información, por lo que no bastaría con tener un HTML fijo.

## 3. Ejecución en el servidor

Cuando el navegador solicita `http://localhost:8080/status`, la petición llega primero al contenedor `web`, donde está Nginx. Nginx utiliza `proxy_pass` para enviarla al servicio `backend`, que escucha en el puerto 8080. Allí ASP.NET Core recibe la petición y ejecuta el código C# asociado a la ruta `/status`. Ese código genera el HTML con la fecha y la hora y lo devuelve a Nginx, que se lo entrega al navegador. Si utilizáramos CGI, se ejecutaría un programa externo para atender la petición. Con un módulo empotrado, el código se ejecutaría dentro del propio servidor web.

## 4. Reverse proxy

### a) Comparación de las cabeceras `Server`

Al ejecutar `curl.exe -I http://localhost:8080/status` con Docker, aparece la cabecera `Server: nginx/1.27.5`. En cambio, al ejecutar el proyecto directamente con `dotnet run`, aparece `Server: Kestrel`.

Esto se debe a que, con Docker, las peticiones pasan primero por Nginx, que actúa como proxy inverso y las reenvía al backend. Sin Docker, el navegador se comunica directamente con Kestrel, que es el servidor web de ASP.NET Core.

### b) ¿Qué ocurre al detener el backend?

Ejecuté `docker compose stop backend` para detener el backend y después probé las direcciones `/status` y `/css/site.css`. Las dos devolvieron un error `502 Bad Gateway`. Esto ocurre porque Nginx intenta enviar las peticiones al backend, pero este está detenido y no puede responder.

### c) ¿Por qué el backend no publica un puerto?

El backend no necesita publicar un puerto al exterior porque Nginx se comunica con él mediante la red interna de Docker. De esta forma, los usuarios acceden a la aplicación a través de Nginx y no directamente al backend.

## 5. nginx

Nginx no sirve `site.css` ni `logo.png` directamente porque está configurado como proxy inverso y reenvía las peticiones al backend. Es ASP.NET Core quien se encarga de servir esos archivos desde la carpeta `wwwroot`. Si NovaHarvest se publicara en Internet, Nginx podría aportar funciones como gestionar las conexiones HTTPS y repartir las peticiones entre varios servidores para evitar que uno solo tenga toda la carga.

## 6. Tecnologías

Si tuviéramos que cambiar ASP.NET Core por otra tecnología de servidor, por ejemplo PHP, tendríamos que modificar los archivos del backend. Cambiarían el código de `Program.cs`, el archivo del proyecto `.csproj` y el `Dockerfile` del backend, porque habría que utilizar una imagen adecuada para PHP. En cambio, se podrían mantener `compose.yaml` y la configuración `nginx.conf`, haciendo los cambios necesarios si cambiara el puerto o la forma de comunicarse. La estructura de las carpetas y los archivos estáticos también podrían mantenerse.

## 7. Marcado y código

### Código de una de las páginas de Program.cs

He elegido la ruta `/status`:

```csharp
app.MapGet("/status", () => Results.Content(
$"""
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>NovaHarvest - Estado del servidor</title>
    <link rel="stylesheet" href="/css/site.css">
</head>
<body>
    <header>
        <img src="/img/logo.png" alt="Logo de NovaHarvest" width="200">
        <h1>NovaHarvest</h1>
        <nav>
            <a href="/">Inicio</a>
            <a href="/about">Acerca de</a>
            <a href="/status">Estado</a>
        </nav>
    </header>

    <main>
        <h1>Estado del servidor</h1>
        <p>Fecha y hora del servidor: {DateTime.Now:dd/MM/yyyy HH:mm:ss}</p>
        <p>Página generada por ASP.NET Core (Kestrel)</p>
    </main>

    <footer>
        NovaHarvest · IES Valle del Jerte · {DateTime.Now.Year}
    </footer>
</body>
</html>
""",
"text/html"));
```

### Dos problemas concretos de generar HTML a base de cadenas

1. **Es difícil de mantener:** si queremos cambiar la estructura o el diseño de las páginas, tenemos que modificar directamente las cadenas HTML dentro del código C#.

2. **Se repite mucho código:** tenemos que escribir manualmente las etiquetas HTML en cada ruta, haciendo que el archivo `Program.cs` sea más largo y que los cambios sean más pesados.

### ¿Qué parte se repite en las tres páginas?

En las tres páginas se repite la estructura HTML, especialmente el `header`, el logotipo, el nombre de NovaHarvest, el menú de navegación (`nav`) y el `footer`. La parte que cambia principalmente es el contenido del `main`.

## 8. Docker

Con el entorno levantado tengo dos contenedores: uno para `web`, que utiliza Nginx, y otro para `backend`, que ejecuta ASP.NET Core. También tengo dos imágenes, una para cada servicio. La imagen es la plantilla con la que se crea un contenedor, mientras que el contenedor es el servicio que está funcionando. La imagen final del backend no incluye el SDK porque solo necesita ejecutar la aplicación ya compilada, no volver a compilarla. Sin el archivo `.dockerignore`, Docker podría copiar carpetas como `bin` y `obj` al contexto de construcción, haciendo que se envíen archivos que no hacen falta y aumentando el tamaño.

## 9. Herramientas

| Herramienta | Para qué la he utilizado |
|---|---|
| Visual Studio Code | Escribir y modificar los archivos del proyecto. |
| C# y ASP.NET Core | Crear las páginas y generar su contenido desde el servidor. |
| .NET SDK | Crear, compilar y ejecutar el proyecto. |
| Docker | Crear y ejecutar los contenedores. |
| Docker Compose | Gestionar los servicios `web` y `backend`. |
| Nginx | Recibir las peticiones y enviarlas al backend. |
| Git | Guardar los cambios y controlar las versiones del proyecto. |
| GitHub | Guardar el repositorio y poder acceder a él online. |
| PowerShell | Ejecutar los comandos de Docker, Git y .NET. |
| Navegador web | Comprobar que las páginas se muestran correctamente. |

## 10. Observación

Al comparar la fecha y hora de la página /status ejecutada con Docker y mediante dotnet run, observé que había una diferencia de una hora. Para comprobarlo, ejecuté date dentro del contenedor y Get-Date en Windows. Docker mostraba la hora UTC (11:05), mientras que Windows mostraba las 12:05.

Esto se debe a que el contenedor utiliza la zona horaria UTC y Windows utiliza la hora local de España (UTC+1). Por tanto, aunque la aplicación es la misma, la hora que muestra puede variar dependiendo de la zona horaria del entorno donde se ejecuta.