:: Generated by: https://github.com/swagger-api/swagger-codegen.git
::

@echo off

dotnet restore src\pvmigrationrest.API
dotnet build src\pvmigrationrest.API
echo Now, run the following to start the project: dotnet run --urls=http://0.0.0.0:8088 -p src\pvmigrationrest.API\pvmigrationrest.API.csproj --launch-profile web.
echo.