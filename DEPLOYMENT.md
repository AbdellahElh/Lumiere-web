# Free Deployment Guide - Render + Vercel + Supabase

## 🎯 **Recommended Free Stack:**

- **Backend**: Render (750 hours/month free tier)
- **Frontend**: Vercel (unlimited static hosting)
- **Database**: Supabase PostgreSQL (500MB free tier)

## 🚀 **Total Monthly Cost: $0**

## Quick Start Deployment

### 1. Database Setup (Supabase)

1. Go to [supabase.com](https://supabase.com) and create a new project
2. Copy your connection details from **Settings > Database**
3. Note down:
   - Host: `db.xxx.supabase.co`
   - Database name: `postgres`
   - Username: `postgres`
   - Password: `[your-password]`
   - Port: `5432`

### 2. Backend Deployment (Render)

1. Go to [render.com](https://render.com) and connect your GitHub account
2. Click **"New +"** → **"Web Service"**
3. Select your `Lumiere-web` repository
4. Configure the service:

   - **Name**: `lumiere-backend`
   - **Environment**: `Docker` (will auto-detect `render.yaml`)
   - **Plan**: `Free`
   - **Auto-Deploy**: `Yes`

5. Add environment variables in Render dashboard:
   ```
   SUPABASE_DB_HOST=db.xxx.supabase.co
   SUPABASE_DB_NAME=postgres
   SUPABASE_DB_USER=postgres
   SUPABASE_DB_PASSWORD=your-supabase-password
   ASPNETCORE_ENVIRONMENT=Production
   ```

### 3. Frontend Deployment (Vercel)

1. Go to [vercel.com](https://vercel.com) and import your GitHub repository
2. Configure build settings:

   - **Framework Preset**: `Other`
   - **Build Command**:
     ```bash
     cd Rise.Client && npm install && npm run build:css && cd .. && dotnet publish Rise.Client/Rise.Client.csproj -c Release -o ./wwwroot
     ```
   - **Output Directory**: `wwwroot/wwwroot`
   - **Install Command**: `npm install`

3. Add environment variable:
   ```
   BACKEND_URL=https://lumiere-backend.onrender.com
   ```

## Automated Deployment (GitHub Actions)

The project includes GitHub Actions workflows for automatic deployment:

### Required GitHub Secrets

Add these secrets to your GitHub repository (**Settings** → **Secrets and variables** → **Actions**):

```
RENDER_SERVICE_ID=srv-xxxxxxxxxx
RENDER_API_KEY=rnd_xxxxxxxxxx
VERCEL_TOKEN=xxxxxxxxxx
VERCEL_ORG_ID=team_xxxxxxxxxx
VERCEL_PROJECT_ID=prj_xxxxxxxxxx
```

### Backend Workflow (`.github/workflows/deploy-backend.yml`)

Automatically deploys to Render when you push changes to the `main` branch.

### Frontend Workflow (`.github/workflows/deploy-frontend.yml`)

Automatically deploys to Vercel when you push changes to the `main` branch.

## Manual Deployment

### Quick Deploy Script

Run the deployment script:

```bash
./deploy-render.sh
```

This script will:

1. Build TailwindCSS assets
2. Build the .NET application
3. Provide step-by-step deployment instructions

## Free Tier Limitations

### Render Free Tier

- **750 hours/month** (about 31 days if always running)
- **Service sleeps after 15 minutes** of inactivity
- **512MB RAM**, 0.1 CPU
- Cold start time: ~30 seconds

### Vercel Free Tier

- **Unlimited static hosting**
- **100 deployments/day**
- **100GB bandwidth/month**
- No cold starts for static content

### Supabase Free Tier

- **500MB database**
- **50MB file storage**
- **2 projects maximum**
- **Pauses after 1 week inactivity**

## URLs After Deployment

- **Backend API**: `https://lumiere-backend.onrender.com`
- **Frontend**: `https://your-project-name.vercel.app`
- **Health Check**: `https://lumiere-backend.onrender.com/health`
- **Database**: Managed through Supabase dashboard

## Troubleshooting

### Backend Issues

- Check Render logs in dashboard
- Verify environment variables are set
- Ensure Supabase connection string is correct

### Frontend Issues

- Check Vercel build logs
- Verify TailwindCSS builds correctly
- Ensure backend URL is configured

### Database Issues

- Check Supabase project status
- Verify connection details
- Run migrations if needed

## Production Configuration

The following files are configured for production:

- `render.yaml` - Render service configuration
- `vercel.json` - Vercel deployment settings
- `appsettings.Production.json` - Production environment settings
- GitHub Actions workflows for CI/CD

## Cost Breakdown

| Service   | Plan  | Cost         | Limitations                   |
| --------- | ----- | ------------ | ----------------------------- |
| Render    | Free  | $0/month     | 750 hours, sleeps after 15min |
| Vercel    | Hobby | $0/month     | 100GB bandwidth               |
| Supabase  | Free  | $0/month     | 500MB database                |
| **Total** |       | **$0/month** |                               |

## Scaling Options

When you outgrow the free tiers:

- **Render**: $7/month for always-on service
- **Vercel**: $20/month for Pro plan
- **Supabase**: $25/month for Pro plan

All services provide seamless upgrading without downtime.
paths:
[
"Rise.Server/**",
"Rise.Services/**",
"Rise.Domain/**",
"Rise.Persistence/**",
"Rise.Shared/**",
]

jobs:
deploy:
runs-on: ubuntu-latest

    steps:
      - uses: actions/checkout@v3

      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: 8.0.x

      - name: Restore dependencies
        run: dotnet restore

      - name: Build
        run: dotnet build --no-restore

      - name: Publish
        run: dotnet publish Rise.Server/Rise.Server.csproj -c Release -o ${{env.DOTNET_ROOT}}/myapp

      - name: Deploy to Azure
        uses: azure/webapps-deploy@v2
        with:
          app-name: "lumiere-backend"
          publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
          package: ${{env.DOTNET_ROOT}}/myapp

````

### Step 2: Setup Free Database

#### Option A: Azure SQL Database (Free Tier)

```bash
# Create SQL server
az sql server create --name lumiere-sql --resource-group lumiere-rg --location "East US" --admin-user sqladmin --admin-password YourPassword123!

# Create database (Basic tier - essentially free for small apps)
az sql db create --name lumiere-db --server lumiere-sql --resource-group lumiere-rg --edition Basic --compute-model Provisioned --family Gen5 --capacity 5
````

#### Option B: Supabase (Recommended for simplicity)

1. Go to [Supabase](https://supabase.com)
2. Create new project (free tier: 500MB database, 2GB bandwidth)
3. Copy the connection string

### Step 3: Deploy Frontend to Vercel

#### 3.1 Prepare Blazor WebAssembly for Static Hosting

Create `build-frontend.sh`:

```bash
#!/bin/bash
cd Rise.Client
dotnet publish -c Release -o ../dist/wwwroot
cd ../dist/wwwroot
# Create vercel.json for SPA routing
cat > vercel.json << 'EOF'
{
  "routes": [
    { "handle": "filesystem" },
    { "src": "/.*", "dest": "/index.html" }
  ]
}
EOF
```

#### 3.2 Deploy to Vercel

```bash
# Install Vercel CLI
npm i -g vercel

# Deploy
cd dist/wwwroot
vercel --prod
```

## Option 2: Render Free Tier

### Step 1: Deploy to Render

1. Connect GitHub repository to Render
2. Create new Web Service
3. Configure:
   - **Environment**: Docker
   - **Dockerfile**: Use existing Dockerfile
   - **Plan**: Free (750 hours/month)

### Step 2: Environment Variables on Render

```
ASPNETCORE_ENVIRONMENT=Production
CONNECTION_STRING=[your_database_connection_string]
ASPNETCORE_URLS=http://0.0.0.0:$PORT
```

## Database Options (All Free)

### 1. Supabase PostgreSQL (Recommended)

- **Free tier**: 500MB database, 2GB bandwidth
- **Pros**: Easy setup, includes Auth, Real-time features
- **Cons**: PostgreSQL (you'll need to change from MariaDB)

### 2. PlanetScale MySQL

- **Free tier**: 1GB storage, 1 billion reads/month
- **Pros**: MySQL compatible, easy scaling
- **Cons**: Requires credit card for signup

### 3. Azure SQL Database

- **Free tier**: 32MB (very limited)
- **Pros**: Integrated with Azure, SQL Server compatible
- **Cons**: Very small storage limit

### 4. ElephantSQL PostgreSQL

- **Free tier**: 20MB database
- **Pros**: True PostgreSQL, no credit card required
- **Cons**: Very limited storage

## Frontend Deployment (Free Options)

### 1. Vercel (Recommended)

- **Free tier**: Unlimited static sites, 100GB bandwidth
- **Perfect for**: Blazor WebAssembly
- **Custom domains**: Supported

### 2. Netlify

- **Free tier**: 100GB bandwidth, 300 build minutes
- **Perfect for**: Static sites
- **Features**: Form handling, serverless functions

### 3. GitHub Pages

- **Free tier**: Unlimited static sites
- **Limitation**: Public repositories only (unless GitHub Pro)

## Cost Breakdown (Completely Free)

### Option 1: Azure + Vercel + Supabase

- **Azure App Service F1**: $0/month (free tier)
- **Vercel**: $0/month (free tier)
- **Supabase**: $0/month (free tier)
- **Total**: $0/month

### Option 2: Render + Vercel + Supabase

- **Render**: $0/month (750 hours free)
- **Vercel**: $0/month (free tier)
- **Supabase**: $0/month (free tier)
- **Total**: $0/month

## Limitations of Free Tiers

### Azure App Service F1

- ❌ Custom domains require paid plan
- ❌ 60 minutes/day compute time limit
- ❌ No always-on (app sleeps after 20 minutes)
- ✅ 1GB RAM, 1GB storage

### Render Free

- ❌ App sleeps after 15 minutes of inactivity
- ❌ 750 hours/month limit (~25 days)
- ❌ Slower cold starts
- ✅ Automatic HTTPS, custom domains

### Recommended Architecture

For a production-ready free deployment:

```
┌─────────────────┐    ┌──────────────────┐    ┌────────────────┐
│   Vercel        │    │   Azure/Render   │    │   Supabase     │
│   (Frontend)    ├────┤   (Backend API)  ├────┤   (Database)   │
│   Blazor WASM   │    │   .NET 8 API     │    │   PostgreSQL   │
└─────────────────┘    └──────────────────┘    └────────────────┘
        │                         │                        │
        │                         │                        │
    Free Tier               Free Tier                Free Tier
   Unlimited               F1: 60min/day           500MB Storage
```

## Next Steps

1. Choose your preferred option (Azure recommended for .NET)
2. Set up database (Supabase recommended for simplicity)
3. Deploy backend to Azure App Service
4. Deploy frontend to Vercel
5. Configure environment variables and CORS

For detailed step-by-step instructions, see the specific deployment guides for each platform.
