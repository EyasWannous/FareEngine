FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/FareEngine.API/FareEngine.API.csproj", "src/FareEngine.API/"]
COPY ["src/FareEngine.Application/FareEngine.Application.csproj", "src/FareEngine.Application/"]
COPY ["src/FareEngine.Domain/FareEngine.Domain.csproj", "src/FareEngine.Domain/"]
COPY ["src/FareEngine.Infrastructure/FareEngine.Infrastructure.csproj", "src/FareEngine.Infrastructure/"]
RUN dotnet restore "src/FareEngine.API/FareEngine.API.csproj"
COPY . .
WORKDIR "/src/src/FareEngine.API"
RUN dotnet build "./FareEngine.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FareEngine.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FareEngine.API.dll"]
