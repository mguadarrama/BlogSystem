FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BlogApi/BlogApi.csproj", "BlogApi/"]
COPY ["BlogCore/BlogCore.csproj", "BlogCore/"]
COPY ["BlogInfra/BlogSystem.Infrastructure.csproj", "BlogInfra/"]
RUN dotnet restore "BlogApi/BlogApi.csproj"
COPY . .
WORKDIR "/src/BlogApi"
RUN dotnet build "BlogApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlogApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlogApi.dll"] 