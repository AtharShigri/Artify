param([string]$message = "deploy: update build")

Write-Host "Building React frontend..." -ForegroundColor Cyan
cd Artififrontend
npm run build

Write-Host "Copying build to wwwroot..." -ForegroundColor Cyan
Copy-Item -Path "dist\*" -Destination "..\wwwroot\" -Recurse -Force

cd ..
git add .
git commit -m $message

Write-Host "Pushing to GitHub..." -ForegroundColor Cyan
git push origin main

Write-Host "Pushing to Azure..." -ForegroundColor Cyan
git push azure main:master

Write-Host "Deployment complete!" -ForegroundColor Green