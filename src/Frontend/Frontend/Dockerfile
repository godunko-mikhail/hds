FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Frontend/Frontend/Frontend.csproj", "src/Frontend/Frontend/"]
COPY ["src/Frontend/Frontend.Client/Frontend.Client.csproj", "src/Frontend/Frontend.Client/"]
RUN dotnet restore "src/Frontend/Frontend/Frontend.csproj"
COPY . .
WORKDIR "/src/src/Frontend/Frontend"
RUN dotnet build "Frontend.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Frontend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Frontend.dll"]
