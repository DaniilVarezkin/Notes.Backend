using Notes.Persistence;
using AutoMapper;
using Notes.Application;
using Notes.Application.Common.Mapping;
using System.Reflection;
using Notes.Application.Interfaces;
using Notes.WebApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Notes.WebApi;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllers();
services.AddAutoMapper(config =>
{
    config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    config.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly));
});

services.AddApplication();
services.AddPersistence(builder.Configuration);

services.AddCors(opt =>
{
    opt.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});

services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer("Bearer", options =>
{
    options.Authority = "https://localhost:44359/";
    options.Audience = "NotesWebAPI";
    options.RequireHttpsMetadata = false;
});

services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
});

services.AddTransient<IConfigureOptions<SwaggerGenOptions>,
    ConfigureSwaggerOptions>();

services.AddSwaggerGen(config =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    config.IncludeXmlComments(xmlPath);
});

services.AddApiVersioning();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        var context = serviceProvider.GetRequiredService<NotesDbContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {

    }
}

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
Configure(app, apiVersionDescriptionProvider);

app.Run();

void Configure(WebApplication app, IApiVersionDescriptionProvider provider)
{

    app.UseSwagger();
    app.UseSwaggerUI(cfg =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            cfg.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }

        cfg.RoutePrefix = string.Empty;
        cfg.SwaggerEndpoint("swagger/v1/swagger.json", "Notes API");
    });

    app.UseCustomExceptionHandler();

    app.UseRouting();
    app.UseHttpsRedirection();
    app.UseCors("AllowAll");

    app.UseAuthentication();
    app.UseAuthorization();
    app.UseApiVersioning();
    app.MapControllers();
}