FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/academy/api
COPY ["Services/CSharpOnlinePlatform/WebApi/WebApi.csproj", "WebApi/"]
COPY ["Services/CSharpOnlinePlatform/Domain/Domain.csproj", "Domain/"]
COPY ["Core/Core.csproj", "../../Core/"]
COPY ["Services/CSharpOnlinePlatform/Application/Application.csproj", "Application/"]
COPY ["Services/CSharpOnlinePlatform/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "WebApi/WebApi.csproj"
COPY . .
WORKDIR "/src/academy/api/WebApi"
RUN dotnet build "WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]
