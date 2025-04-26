@echo off
setlocal

set PROJECT=src\Microservice\Microservice.csproj
if "%1"=="build" (
  dotnet build %PROJECT% --configuration Development
  goto :eof
)
if "%1"=="start" (
  dotnet run --project %PROJECT% --environment Development
  goto :eof
)
if "%1"=="watch" (
  dotnet watch run --project %PROJECT% --environment Development
  goto :eof
)

echo Usage: %0 ^{build^|start^|watch^}
