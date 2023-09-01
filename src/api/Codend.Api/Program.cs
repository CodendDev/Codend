using Codend.Api.Extensions;
using Codend.Application;
using Codend.Infrastructure;
using Codend.Persistence;
using Codend.Presentation;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPersistence(builder.Configuration)
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

try
{
    app.MigrateDatabase<SqlServerCodendDbContext>();
    app.Logger.Log(LogLevel.Information, "Using SqlServer.");
}
catch (InvalidOperationException)
{
    app.MigrateDatabase<PostgresCodendDbContext>();
    app.Logger.Log(LogLevel.Information, "Using PostgreSQL.");
}

app.MapGet("", () => "Hello :)");

app.Run();