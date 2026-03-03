# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy each project file individually
COPY DungeonExplorer.Api/DungeonExplorer.Api.csproj DungeonExplorer.Api/
COPY DungeonExplorer.Service/DungeonExplorer.Api.Service.csproj DungeonExplorer.Service/
COPY DungeonExplorer.Storage/DungeonExplorer.Api.Storage.csproj DungeonExplorer.Storage/
COPY DungeonExplorer.Domain/DungeonExplorer.Api.Domain.csproj DungeonExplorer.Domain/

# Restore each project
RUN dotnet restore DungeonExplorer.Api/DungeonExplorer.Api.csproj
RUN dotnet restore DungeonExplorer.Service/DungeonExplorer.Api.Service.csproj
RUN dotnet restore DungeonExplorer.Storage/DungeonExplorer.Api.Storage.csproj
RUN dotnet restore DungeonExplorer.Domain/DungeonExplorer.Api.Domain.csproj

# Copy everything else
COPY . .

# Publish only the API project (references will bring in Service/Storage/Domain)
RUN dotnet publish DungeonExplorer.Api/DungeonExplorer.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DungeonExplorer.Api.dll"]
