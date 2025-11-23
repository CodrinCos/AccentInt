# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ARG APP_UID=1000
ARG APP_GID=1000
ENV ASPNETCORE_URLS=http://+:8080
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Create non-root user (used in final stage)
RUN set -eux; \
    groupadd -g $APP_GID app || true; \
    useradd -m -u $APP_UID -g $APP_GID app || true

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the whole repo into the build context so project-to-project references are available.
# This avoids missing .csproj errors when the docker build context is the project folder.
COPY . .

WORKDIR /src/AccentInt.ApiService
RUN dotnet restore "AccentInt.ApiService.csproj"
RUN dotnet build "AccentInt.ApiService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/AccentInt.ApiService
RUN dotnet publish "AccentInt.ApiService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN chown -R app:app /app
USER app
ENTRYPOINT ["dotnet", "AccentInt.ApiService.dll"]