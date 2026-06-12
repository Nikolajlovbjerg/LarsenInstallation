# --- Build stage ---
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Tooling needed to publish the Blazor WebAssembly client
RUN dotnet workload install wasm-tools

# NEW: Install Python3 so Emscripten (wasm-tools) can compile native assets
RUN apt-get update && apt-get install -y python3 && rm -rf /var/lib/apt/lists/*

# Copy only project files first to leverage Docker cache for restore
COPY Server/Server.csproj Server/
COPY Client/Client.csproj Client/
COPY Core/Core.csproj Core/
RUN dotnet restore Server/Server.csproj

# Copy the rest of the code and publish
COPY . .
RUN dotnet publish Server/Server.csproj -c Release -o /app/publish /p:UseAppHost=false

# --- Runtime stage ---
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production

# Render injects $PORT; bind ASP.NET Core to it (fallback 8080 for plain `docker run`)
ENTRYPOINT ["sh", "-c", "ASPNETCORE_URLS=http://+:${PORT:-8080} dotnet Server.dll"]