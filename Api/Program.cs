using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RecipesAPI.Api.Data;
using RecipesAPI.Api.Middlewares;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var serviceName = "RecipesAPI.Api";

builder.Services.AddOpenTelemetryTracing(tracerProviderBuilder =>
{
    tracerProviderBuilder
    .AddConsoleExporter()
    .AddZipkinExporter(o => {
    })
    .AddSource(serviceName)
    .SetResourceBuilder(
        ResourceBuilder.CreateDefault()
            .AddService(serviceName))
    .AddHttpClientInstrumentation()
    .AddAspNetCoreInstrumentation()
    .AddSqlClientInstrumentation();
});

ConfigurationManager configuration = builder.Configuration;

builder.Services.AddSingleton<IConnectionMultiplexer>(opt => 
    ConnectionMultiplexer.Connect(configuration["ConnectionStrings:RedisConnection"]));

builder.Services.AddScoped<IAppRepo, RedisRepo>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseWhen(context => {
    return context.Request.Path.StartsWithSegments("/api/recipes");
}, appBuilder => { 
    appBuilder.UseMiddleware<ApiKeyMiddleware>();
});

app.Run();
