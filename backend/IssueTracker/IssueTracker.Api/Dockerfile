# Consulte https://aka.ms/customizecontainer para aprender a personalizar su contenedor de depuración y cómo Visual Studio usa este Dockerfile para compilar sus imágenes para una depuración más rápida.

# Esta fase se usa cuando se ejecuta desde VS en modo rápido (valor predeterminado para la configuración de depuración)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["IssueTracker.Api/IssueTracker.Api.csproj", "IssueTracker.Api/"]
COPY ["IssueTracker.Application/IssueTracker.Application.csproj", "IssueTracker.Application/"]
COPY ["IssueTracker.Domain/IssueTracker.Domain.csproj", "IssueTracker.Domain/"]
COPY ["IssueTracker.Infrastructure/IssueTracker.Infrastructure.csproj", "IssueTracker.Infrastructure/"]

RUN dotnet restore "IssueTracker.Api/IssueTracker.Api.csproj"

COPY . .

WORKDIR "/src/IssueTracker.Api"
RUN dotnet build "IssueTracker.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "IssueTracker.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IssueTracker.Api.dll"]