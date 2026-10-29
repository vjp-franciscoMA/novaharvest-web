
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapStaticAssets();

app.MapGet("/", () => Results.Content(
$"""
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>NovaHarvest - Inicio</title>
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
        <h1>Inicio</h1>
        <p>En el Valle del Jerte se producen excedentes de fruta y verdura que muchas veces no llegan a venderse.</p>
        <p>NovaHarvest busca conectar a los productores con personas y organizaciones que puedan aprovechar estos productos.</p>
        <p>La idea es reducir el desperdicio alimentario y facilitar el aprovechamiento de los excedentes.</p>
    </main>

    <footer>
        NovaHarvest · IES Valle del Jerte · {DateTime.Now.Year}
    </footer>
</body>
</html>
""",
"text/html"));

app.MapGet("/about", () => Results.Content(
$"""
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>NovaHarvest - Acerca de</title>
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
        <h1>Acerca de</h1>
        <p>NovaHarvest es un proyecto de desarrollo web inspirado en la actividad agrícola del Valle del Jerte.</p>
        <p>Centro educativo: IES Valle del Jerte.</p>
        <p>Autor: Francisco Montero Acosta.</p>
        <p>Versión: 0.1.0.</p>
    </main>

    <footer>
        NovaHarvest · IES Valle del Jerte · {DateTime.Now.Year}
    </footer>
</body>
</html>
""",
"text/html"));

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

app.Run();