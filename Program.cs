using Yarp.ReverseProxy;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// 1. Configura la policy CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // * ma solo per test
              .AllowAnyHeader()
              .AllowAnyMethod();
        //.AllowCredentials(); // in futuro per cookie o auth header
    });
});

// 2. Aggiungi YARP
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddControllers();

// 3. (opzionale) per vuoi cambiare livelli, formati, ecc.
//    builder.Logging.ClearProviders();
//    builder.Logging.AddConsole();
//    builder.Logging.AddDebug();

var app = builder.Build();

// Recupera il logger di Program
var logger = app.Services.GetRequiredService<ILogger<Program>>();

// 4. Middleware di logging delle richieste
app.Use(async (context, next) =>
{
    var method = context.Request.Method;
    var path = context.Request.Path;
    var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

    logger.LogInformation(
        "[ReverseProxy] 🌐 {Method} {Path} from {Ip}",
        method,
        path,
        ip
    );

    await next();
});

// 5. Applica la policy CORS PRIMA del reverse proxy
app.UseCors("AllowFrontend");

app.MapControllers();
app.MapReverseProxy();

app.Run();
