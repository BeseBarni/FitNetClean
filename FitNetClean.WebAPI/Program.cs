using FastEndpoints;
using FastEndpoints.Swagger;
using FitNetClean.Application;
using FitNetClean.Infrastructure;
using FitNetClean.WebAPI.Extensions;
using FitNetClean.WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddAuthorizationPolicies();

builder.Services.AddGeneratedSettings(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();

var app = builder.Build();

await app.UseApplyMigrations();
await app.UseDatabaseSeed();

app.UseGlobalExceptionHandler();

app.UseRequestIdentity();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapHealthChecks("/health");

app.UseAuthentication();
app.UseAuthorization();

app.UseAuthorizedRequestLogging();

app.UseFastEndpoints(c =>
{
    c.Endpoints.RoutePrefix = "api";
});

app.UseSwaggerGen();

app.Run();

