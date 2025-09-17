#!/bin/bash

echo "🚀 Starting Render + Vercel + Supabase deployment for Lumiere..."

# Configuration
SERVICE_NAME="lumiere-backend"
echo "📦 Deploying backend to Render..."

# Build CSS first
echo "🎨 Building TailwindCSS..."
cd Rise.Client
npm install
npm run build:css
cd ..

# Build .NET application
echo "🏗️ Building .NET application..."
/Users/abde/.dotnet/dotnet restore
/Users/abde/.dotnet/dotnet build -c Release

echo "✅ Build complete! Now configure deployment:"

echo ""
echo "🔧 Render Setup Instructions:"
echo "1. Go to https://render.com and connect your GitHub repository"
echo "2. Create a new Web Service"
echo "3. Select this repository: $(git remote get-url origin)"
echo "4. Configure the service:"
echo "   - Name: $SERVICE_NAME"
echo "   - Environment: Docker (will use render.yaml)"
echo "   - Plan: Free"
echo "   - Auto-Deploy: Yes"
echo ""

echo "🗄️ Supabase Database Setup:"
echo "1. Go to https://supabase.com and create a new project"
echo "2. Copy your connection string from Settings > Database"
echo "3. Add it to Render environment variables as CONNECTION_STRING"
echo ""

echo "🌐 Vercel Frontend Setup:"
echo "1. Go to https://vercel.com and import your GitHub repository"
echo "2. Select 'Other' framework"
echo "3. Configure build settings:"
echo "   - Build Command: cd Rise.Client && npm install && npm run build:css && cd .. && dotnet publish Rise.Client/Rise.Client.csproj -c Release -o ./wwwroot"
echo "   - Output Directory: wwwroot/wwwroot"
echo "   - Install Command: npm install"
echo ""

echo "🔐 Environment Variables needed:"
echo "For Render service:"
echo "  - CONNECTION_STRING: (your Supabase connection string)"
echo "  - ASPNETCORE_ENVIRONMENT: Production"
echo ""

echo "📝 GitHub Secrets needed for automated deployment:"
echo "  - RENDER_SERVICE_ID: (get from Render dashboard)"
echo "  - RENDER_API_KEY: (generate from Render account settings)"
echo "  - VERCEL_TOKEN: (generate from Vercel account settings)"
echo "  - VERCEL_ORG_ID: (from Vercel project settings)"
echo "  - VERCEL_PROJECT_ID: (from Vercel project settings)"
echo ""

echo "🎯 Free Tier Limits:"
echo "  - Render: 750 hours/month (sleeps after 15min inactivity)"
echo "  - Vercel: Unlimited static hosting"
echo "  - Supabase: 500MB database, 50MB file storage"
echo ""

echo "🌐 Your URLs will be:"
echo "  - Backend: https://$SERVICE_NAME.onrender.com"
echo "  - Frontend: https://your-project-name.vercel.app"
echo "  - Health check: https://$SERVICE_NAME.onrender.com/health"