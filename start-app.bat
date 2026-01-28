@echo off
echo Starting Artify Application...

set ROOT_DIR=%~dp0

:: Start Backend
echo Starting Backend (ASP.NET Core)...
start "Artify Backend" cmd /k "cd /d %ROOT_DIR% && dotnet run --project Artify.Api.csproj"

:: Start Frontend
echo Starting Frontend (Vite)...
start "Artify Frontend" cmd /k "cd /d %ROOT_DIR%Artifyfrontend && npm run dev"

echo Both services are starting...
echo Backend: https://localhost:7294/swagger
echo Frontend: http://localhost:5173