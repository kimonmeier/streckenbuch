# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0-noble AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0-noble AS build

ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Streckenbuch.Server/Streckenbuch.Server.csproj", "Streckenbuch.Server/"]
COPY ["Streckenbuch.Miku/Streckenbuch.Miku.csproj", "Streckenbuch.Miku/"]
COPY ["Streckenbuch.Client/Streckenbuch.Client.csproj", "Streckenbuch.Client/"]
COPY ["Streckenbuch.Shared/Streckenbuch.Shared.csproj", "Streckenbuch.Shared/"]
RUN dotnet restore "./Streckenbuch.Server/Streckenbuch.Server.csproj"
COPY . .
WORKDIR "/src/Streckenbuch.Server"

RUN dotnet build "./Streckenbuch.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Streckenbuch.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Streckenbuch.Server.dll"]