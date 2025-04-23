using Yarp.ReverseProxy;

var builder = WebApplication.CreateBuilder(args);

// 1. Configura la policy CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // oppure * ma solo per test
              .AllowAnyHeader()
              .AllowAnyMethod();
              //.AllowCredentials(); // in futuro per cookie o auth header
    });
});

// 2. Aggiungi YARP
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// 3. Applica la policy CORS PRIMA del reverse proxy
app.UseCors("AllowFrontend");

app.MapReverseProxy();

app.Run();

