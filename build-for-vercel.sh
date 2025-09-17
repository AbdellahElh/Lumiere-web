#!/bin/bash

echo "🎯 Building Blazor WebAssembly for Vercel deployment..."

# Build CSS first
echo "🎨 Building TailwindCSS..."
cd Rise.Client
npm install
npm run build:css
cd ..

# Build Blazor WebAssembly 
echo "🏗️ Building Blazor WebAssembly..."
/Users/abde/.dotnet/dotnet publish Rise.Client/Rise.Client.csproj -c Release -o ./dist

echo "📁 Preparing files for Vercel..."
# Copy wwwroot contents to root for Vercel
cp -r ./dist/wwwroot/* ./vercel-deploy/

echo "✅ Blazor WebAssembly build complete!"
echo "📤 Now you can deploy the 'vercel-deploy' folder to Vercel"
echo ""
echo "🌐 Vercel Deployment:"
echo "1. In Vercel dashboard, set Output Directory to: vercel-deploy"
echo "2. Set Build Command to: ./build-for-vercel.sh"
echo "3. Deploy!"