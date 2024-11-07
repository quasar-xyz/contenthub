FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ContentHub.Frontend/ContentHub.Frontend.csproj", "ContentHub.Frontend/"]
RUN dotnet restore "ContentHub.Frontend/ContentHub.Frontend.csproj"
COPY . .
WORKDIR "/src/ContentHub.Frontend"
RUN dotnet build "ContentHub.Frontend.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ContentHub.Frontend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContentHub.Frontend.dll"]
