using Codend.Api.Configurations;
using Codend.Api.Extensions;
using Codend.Application;
using Codend.Database;
using Codend.Infrastructure;
using Codend.Infrastructure.Authentication;
using Codend.Presentation;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.Configure<FusionauthConfiguration>(builder.Configuration.GetSection("Fusionauth"));
builder.Services.AddFusionauthAuthentication();

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