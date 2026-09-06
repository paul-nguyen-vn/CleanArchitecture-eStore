using System.Reflection;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddKeyVaultIfConfigured();

builder.AddApplication();

builder.AddInfrastructure();

builder.AddWebServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors(static builder => 
                    builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowAnyOrigin());

app.UseFileServer();

// Enable Swagger endpoints and UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CleanArchitecture eStore API v1");
    // c.RoutePrefix = string.Empty; // uncomment to serve UI at application root
});

//app.MapOpenApi();
//app.MapScalarApiReference();

app.UseExceptionHandler(options => { });

app.MapDefaultEndpoints();

app.MapEndpoints(typeof(Program).Assembly);

app.Run();

public partial class Program { }
