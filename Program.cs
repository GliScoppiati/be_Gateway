using Yarp.ReverseProxy;

var builder = WebApplication.CreateBuilder(args);

// Aggiunge YARP
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();
app.MapReverseProxy();
app.Run();
