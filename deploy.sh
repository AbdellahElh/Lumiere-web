#!/bin/bash

# Deployment script for Railway and Vercel

echo "🚀 Starting deployment process..."

# Check if git repo is clean
if [[ -n $(git status --porcelain) ]]; then
    echo "📝 Committing current changes..."
    git add .
    git commit -m "Deploy: $(date '+%Y-%m-%d %H:%M:%S')"
fi

# Push to GitHub
echo "📤 Pushing to GitHub..."
git push origin main

echo "✅ Code pushed to GitHub"
echo ""
echo "Next steps:"
echo "1. Go to https://railway.app and deploy from GitHub"
echo "2. Set up PostgreSQL database on Railway"
echo "3. Configure environment variables on Railway"
echo "4. Go to https://vercel.com and deploy frontend"
echo "5. Update API URLs in your frontend code"
echo ""
echo "For detailed instructions, see DEPLOYMENT.md"