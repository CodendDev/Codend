FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ARG now
ENV BUILD_DATE=$now

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/api/Codend.Api/Codend.Api.csproj", "src/api/Codend.Api/"]
RUN dotnet restore "src/api/Codend.Api/Codend.Api.csproj"
COPY . .
WORKDIR "/src/src/api/Codend.Api"
RUN dotnet build "Codend.Api.csproj" -c Release -o /app/build

FROM build AS test
WORKDIR /src
ENTRYPOINT ["dotnet", "test"]

FROM build AS publish
RUN dotnet publish "Codend.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Codend.Api.dll"]
