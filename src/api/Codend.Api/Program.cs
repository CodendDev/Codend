using System.Text.Json.Serialization;
using Codend.Api.Configurations;
using Codend.Api.Extensions;
using Codend.Api.Middlewares;
using Codend.Application;
using Codend.Contracts;
using Codend.Database;
using Codend.Infrastructure;
using Codend.Presentation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
        options.Filters.Add(new AuthorizeFilter(
            new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build())))
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.Converters.Add(new JsonDateTimeConverter());
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddFusionauthAuthentication(builder.Configuration);
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

builder.Services
    .AddContracts()
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddDatabase(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseCustomExceptionHandler();

app.MapControllers();

var database = app.MigrateDatabase();

var buildDate = builder.Configuration["BUILD_DATE"];
var deploymentDate = DateTime.UtcNow.ToString("R");
var helloString = $"Deployment date: {deploymentDate}" + (buildDate is not null ? $"\nBuild date: {buildDate}" : "");
app.MapGet("", () => helloString);

app.Run();