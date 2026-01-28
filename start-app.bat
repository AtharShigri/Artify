@echo off
echo Starting Artify Application...

:: Start Backend
echo Starting Backend (ASP.NET Core)...
start "Artify Backend" cmd /k "dotnet run --project Artify.Api.csproj"

:: Start Frontend
echo Starting Frontend (Vite)...
cd Artifyfrontend
start "Artify Frontend" cmd /k "npm run dev"

echo Both services are starting...
echo Backend: https://localhost:7294/swagger
echo Frontend: http://localhost:5173
