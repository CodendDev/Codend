using System.Text.Json.Serialization;
using Codend.Api.Configurations;
using Codend.Api.Extensions;
using Codend.Application;
using Codend.Database;
using Codend.Infrastructure;
using Codend.Infrastructure.Authentication;
using Codend.Presentation;
using Microsoft.AspNetCore.Authorization;
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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.Configure<FusionauthConfiguration>(builder.Configuration.GetSection("Fusionauth"));
builder.Services.AddFusionauthAuthentication();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddDatabase(builder.Configuration)
    .AddPresentation();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var database = app.MigrateDatabase();

app.MapGet("", () => $"Hello {database}");

app.Run();