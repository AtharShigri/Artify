@echo off
echo Starting artifi Application...

set ROOT_DIR=%~dp0

:: Start Backend
echo Starting Backend (ASP.NET Core)...
start "artifi Backend" cmd /k "cd /d %ROOT_DIR% && dotnet run --project artifi.Api.csproj"

:: Start Frontend
echo Starting Frontend (Vite)...
start "artifi Frontend" cmd /k "cd /d %ROOT_DIR%artififrontend && npm run dev"

echo Both services are starting...
echo Backend: https://localhost:7294/swagger
echo Frontend: http://localhost:5173