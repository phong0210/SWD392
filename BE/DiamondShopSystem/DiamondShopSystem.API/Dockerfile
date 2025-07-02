# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Optional: Create non-root user for security
RUN adduser --disabled-password --gecos '' appuser

# Build image with SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files and restore
COPY ["DiamondShopSystem.API/DiamondShopSystem.API.csproj", "DiamondShopSystem.API/"]
COPY ["DiamondShopSystem.BLL/DiamondShopSystem.BLL.csproj", "DiamondShopSystem.BLL/"]
COPY ["DiamondShopSystem.DAL/DiamondShopSystem.DAL.csproj", "DiamondShopSystem.DAL/"]
RUN dotnet restore "DiamondShopSystem.API/DiamondShopSystem.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/DiamondShopSystem.API"
RUN dotnet build "DiamondShopSystem.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the app
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DiamondShopSystem.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime container
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Optional: Switch to non-root user
# USER appuser

# Optional: Add health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "DiamondShopSystem.API.dll"]