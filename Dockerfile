# --- Build stage ---
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Tooling needed to publish the Blazor WebAssembly client
RUN dotnet workload install wasm-tools

# Restore + publish the Server (it references the Client, which gets bundled in)
COPY . .
RUN dotnet restore Server/Server.csproj
RUN dotnet publish Server/Server.csproj -c Release -o /app/publish /p:UseAppHost=false

# --- Runtime stage ---
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production

# Render injects $PORT; bind ASP.NET Core to it (fallback 8080 for plain `docker run`)
ENTRYPOINT ["sh", "-c", "ASPNETCORE_URLS=http://+:${PORT:-8080} dotnet Server.dll"]
