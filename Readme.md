# CodendAPI

This repository is a submodule of the main application. You can find the main application at https://github.com/CodendDev/Codendapp.

## Run application

### Docker Compose

**Clone repository**

```bash
git clone https://github.com/CodendDev/Codend codend
cd codend
```

**Start the application**

```bash
docker compose up
```

**Access the application**

Visit Swagger http://127.0.0.1:8080/swagger in your browser.

**Docker configuration files**

- [nginx config](nginx/nginx.conf)
- [environmental variables](.env)

### Development

**Start dependencies using Docker**

If you want to use PostgreSQL as your database

```bash
docker compose -f docker-compose.Development.PSQL.yml up
```

If you want to use Microsoft SQL Server as your database

```bash
docker compose -f docker-compose.Development.MSSQL.yml up
```

Note: If you use MSSQL as your database, PostgreSQL will also be started because
FusionAuth container is dependent on it.

**Setup appsettings.json**

There are example appsettings files. You have to copy configuration according to
your database container.

Although it is not required you can fill Azure Email Communication Service
configuration to enable email notifications.

**Start application**

```bash
dotnet run --project src/api/Codend.Api/Codend.Api.csproj
```
