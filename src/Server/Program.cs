using System.Text.Json;
using System.Text.Json.Serialization;
using Application;
using Core;
using Infrastructure;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Server.Middlewares;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddHttpClient();

    builder.Services.AddServerServices(builder.Configuration);
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddCoreServices();

    builder.Services.ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    });
    builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    });
    builder.Host.UseSerilog();
    var app = builder.Build();

    // TODO: move this script to ci cd module 
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();
    }

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionsMiddleware();
        app.UseHsts();
    }

    app.UseCors(corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin());
    app.UseHttpsRedirection();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseStaticFiles();
    app.UseRouting();

    app.UseCookiePolicy();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}