﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace Codend.Api.Configurations;

/// <summary>
/// Class that contains methods for swagger configuration.
/// </summary>
public static class SwaggerConfiguration
{
    /// <summary>
    /// Extension method that configures and add swagger.
    /// </summary>
    /// <param name="services">IServiceCollection object.</param>
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Codend - simple project management app",
                Description = "An ASP.NET Core Web API for managing projects and more.",
            });

            // Adding authentication to swagger
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "Enter JWT Bearer token **_only_**",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer", // must be lower case
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme,
                },
            };
            options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, Array.Empty<string>() },
            });
            
            var xmlFiles = Directory.GetFiles(
                    AppContext.BaseDirectory,
                    "*.xml",
                    SearchOption.TopDirectoryOnly)
                .ToList();
            xmlFiles.ForEach(xmlFile => options.IncludeXmlComments(xmlFile));

            options.SupportNonNullableReferenceTypes();
        });
    }
}