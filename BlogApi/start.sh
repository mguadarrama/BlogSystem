#!/bin/bash

# Start SQL Server in the background
/opt/mssql/bin/sqlservr &

# Wait for SQL Server to be ready
echo "Waiting for SQL Server to be ready..."
sleep 30

# Create the database and run migrations
dotnet BlogApi.dll

# Keep the container running
tail -f /dev/null 