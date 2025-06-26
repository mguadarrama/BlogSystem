FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Add build argument to control SQL Server installation
ARG INSTALL_SQL_SERVER=false

# Install SQL Server only if INSTALL_SQL_SERVER is true
RUN if [ "$INSTALL_SQL_SERVER" = "true" ] ; then \
    apt-get update && apt-get install -y curl gnupg2 && \
    curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    curl https://packages.microsoft.com/config/debian/11/prod.list > /etc/apt/sources.list.d/mssql-release.list && \
    apt-get update && ACCEPT_EULA=Y apt-get install -y mssql-tools unixodbc-dev ; \
    fi

# Set environment variables for SQL Server (only used if installed)
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=YourStrong!Passw0rd

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

# Copy and set up start script only if SQL Server is installed
ARG INSTALL_SQL_SERVER=false
RUN if [ "$INSTALL_SQL_SERVER" = "true" ] ; then \
    echo '#!/bin/bash\n\
/opt/mssql/bin/sqlservr &\n\
sleep 30\n\
dotnet BlogApi.dll\n\
tail -f /dev/null' > /app/start.sh && \
    chmod +x /app/start.sh && \
    ENTRYPOINT ["/app/start.sh"] ; \
    else \
    ENTRYPOINT ["dotnet", "BlogApi.dll"] ; \
    fi 