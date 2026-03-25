# ── Stage 1: Build ────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project file and restore dependencies first (layer caching)
COPY MovieWatchlist.csproj .
RUN dotnet restore

# Copy everything else and publish
COPY . .
RUN dotnet publish MovieWatchlist.csproj -c Release -o /app/publish

# ── Stage 2: Runtime ──────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/publish .

# SQLite database file will be written here at runtime
VOLUME /app/data

# Set connection string to use the volume path
ENV ConnectionStrings__DefaultConnection="Data Source=/app/data/watchlist.db"

# Expose HTTP port
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "MovieWatchlist.dll"]
