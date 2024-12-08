using System.Text.Json;
using System.Text.Json.Serialization;
using Application;
using Application.Features.WoodenConstruction.Queries.GetBeamFull;
using AutoMapper;
using Core;
using Core.Models;
using Infrastructure;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Server.Middlewares;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddHttpClient();
    
    builder.Services.AddSwaggerGen(v =>
    {
        v.SwaggerDoc("v1", new OpenApiInfo()
        {
            Title = "HDS server",
            Version = "1.0.0"
        });
    
        v.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = """
                          JWT Authorization header using the Bearer scheme.
                          Enter 'Bearer' [space] and then your token in the text input below.
                          Example: 'Bearer 12345abcdef'
                          """,
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer"
        });

        string[] cqrsProjects = ["Server", "Application", "Infrastructure", "Core", "MathCore"];

        foreach (var project in cqrsProjects)
        {
            v.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{project}.xml"));    
        }
    });
    
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


    app.UseSwagger();
    app.UseSwaggerUI();


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