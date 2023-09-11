using System.Reflection;
using Codend.Api;
using Codend.Api.Extensions;
using Codend.Application;
using Codend.Database;
using Codend.Infrastructure;
using Codend.Presentation;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseAuthorization();

app.MapControllers();

var database = app.MigrateDatabase();

var buildDate = builder.Configuration["BUILD_DATE"];
var deploymentDate = DateTime.UtcNow.ToString("R");
var helloString = $"Deployment date: {deploymentDate}" + (buildDate is not null ? $"\nBuild date: {buildDate}" : "");
app.MapGet("", () => helloString);

app.Run();