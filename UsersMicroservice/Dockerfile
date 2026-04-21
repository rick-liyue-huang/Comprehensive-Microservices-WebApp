# syntax=docker/dockerfile:1.7

# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

# Copy csproj files first to leverage Docker layer caching on restore
COPY UsersMicroservice.sln ./
COPY UsersMicroservice.API/UsersMicroservice.API.csproj            UsersMicroservice.API/
COPY UsersMicroservice.Core/UsersMicroservice.Core.csproj          UsersMicroservice.Core/
COPY UsersMicroservice.Domain/UsersMicroservice.Domain.csproj      UsersMicroservice.Domain/
COPY UsersMicroservice.Infrastructure/UsersMicroservice.Infrastructure.csproj UsersMicroservice.Infrastructure/

RUN dotnet restore UsersMicroservice.API/UsersMicroservice.API.csproj

# Copy the rest of the source and publish
COPY . .
RUN dotnet publish UsersMicroservice.API/UsersMicroservice.API.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS runtime
WORKDIR /app

# Run as the non-root user provided by the base image
USER $APP_UID

ENV ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_HTTP_PORTS=8080 \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "UsersMicroservice.API.dll"]
