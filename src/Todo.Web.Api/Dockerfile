FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Todo.Web.Api/Todo.Web.Api.csproj", "src/Todo.Web.Api/"]
COPY ["src/Todo.Infrastructure/Todo.Infrastructure.csproj", "src/Todo.Infrastructure/"]
COPY ["src/Todo.Application/Todo.Application.csproj", "src/Todo.Application/"]
COPY ["src/Todo.Domain/Todo.Domain.csproj", "src/Todo.Domain/"]
COPY ["src/BuildingBlocks/BuildingBlocks.csproj", "src/BuildingBlocks/"]
RUN dotnet restore "./src/Todo.Web.Api/Todo.Web.Api.csproj"
COPY . .
WORKDIR "/src/src/Todo.Web.Api"
RUN dotnet build "./Todo.Web.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Todo.Web.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Todo.Web.Api.dll"]