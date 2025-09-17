# Azure CLI Deployment Script for Rise.Server
# Run this script to deploy your backend to Azure App Service F1 (Free Tier)

#!/bin/bash

echo "🚀 Starting Azure deployment for Lumiere Backend..."

# Configuration
RESOURCE_GROUP="lumiere-rg"
LOCATION="eastus"
APP_SERVICE_PLAN="lumiere-plan"
WEB_APP_NAME="lumiere-backend"  # Change this to your unique name
CONNECTION_STRING=""  # Add your database connection string

# Login to Azure (uncomment if not already logged in)
# az login

echo "📦 Creating resource group..."
az group create --name $RESOURCE_GROUP --location $LOCATION

echo "🏗️ Creating App Service Plan (F1 - Free Tier)..."
az appservice plan create \
  --name $APP_SERVICE_PLAN \
  --resource-group $RESOURCE_GROUP \
  --sku F1 \
  --is-linux

echo "🌐 Creating Web App..."
az webapp create \
  --name $WEB_APP_NAME \
  --resource-group $RESOURCE_GROUP \
  --plan $APP_SERVICE_PLAN \
  --runtime "DOTNETCORE:8.0"

echo "⚙️ Configuring App Settings..."
az webapp config appsettings set \
  --resource-group $RESOURCE_GROUP \
  --name $WEB_APP_NAME \
  --settings \
    ASPNETCORE_ENVIRONMENT=Production \
    CONNECTION_STRING="$CONNECTION_STRING"

echo "🔧 Installing Node.js and building CSS..."
cd Rise.Client
npm install
npm run build:css
cd ..

echo "🏗️ Building .NET application..."
dotnet restore
dotnet build -c Release

echo "📦 Publishing application..."
dotnet publish Rise.Server/Rise.Server.csproj -c Release -o ./publish

echo "🚀 Deploying to Azure..."
az webapp deploy \
  --resource-group $RESOURCE_GROUP \
  --name $WEB_APP_NAME \
  --src-path ./publish \
  --type zip

echo "✅ Deployment complete!"
echo "🌐 Your backend is available at: https://$WEB_APP_NAME.azurewebsites.net"
echo "🔍 Health check: https://$WEB_APP_NAME.azurewebsites.net/health"

# Cleanup
rm -rf ./publish

echo "📝 Next steps:"
echo "1. Update your database connection string in Azure portal"
echo "2. Configure CORS settings if needed"
echo "3. Set up custom domain (optional)"
echo "4. Monitor usage in Azure portal (60 minutes/day limit on F1 tier)"