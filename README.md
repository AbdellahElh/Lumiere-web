# Rise - [GROUPNAME]

## Team Members

- [MEMBER1_NAME] - [MEMBER1_EMAIL] - [MEMBER1_GITHUB_USERNAME]

## Technologies & Packages Used

- [Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor) - Frontend
- [ASP.NET 8](https://dotnet.microsoft.com/en-us/apps/aspnet) - Backend
- [Entity Framework 8](https://learn.microsoft.com/en-us/ef/) - Database Access
- [EntityFrameworkCore Triggered](https://github.com/koenbeuk/EntityFrameworkCore.Triggered) - Database Triggers
- [User Secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets) - Securely store secrets in DEV.
- [GuardClauses](https://github.com/ardalis/GuardClauses) - Validation Helper
- [bUnit](https://bunit.dev) - Blazor Component Testing
- [xUnit](https://xunit.net) - (Unit) Testing
- [nSubstitute](https://nsubstitute.github.io) - Mocking for testing
- [Shouldly](https://docs.shouldly.org) - Helper for testing

## Setup Instructions (From Scratch)

### Prerequisites

Before setting up this project, ensure you have the following installed:

1. **.NET 8.0 SDK** - Download from [Microsoft's official site](https://dotnet.microsoft.com/download/dotnet/8.0)
2. **Node.js** (v18 or higher) - Download from [nodejs.org](https://nodejs.org/)
3. **Docker** - Download from [docker.com](https://www.docker.com/products/docker-desktop)
4. **Git** - Download from [git-scm.com](https://git-scm.com/)

### Step-by-Step Installation

#### 1. Clone and Navigate to Project

```bash
git clone [REPOSITORY_URL]
cd Lumiere-web
```

#### 2. Verify .NET Installation

```bash
dotnet --version
# Should output version 8.0.xxx
```

#### 3. Verify Node.js Installation

```bash
node --version
# Should output v18.x.x or higher
npm --version
```

#### 4. Install Node.js Dependencies

Navigate to the client project and install dependencies:

```bash
cd Rise.Client
npm install
cd ..
```

#### 5. Restore .NET Dependencies

```bash
dotnet restore
```

#### 6. Build the Solution

```bash
dotnet build
```

**Note:** If you encounter package version conflicts (TypeLoadException), you may need to update package versions in the project files to ensure compatibility.

#### 7. Setup Database with Docker

##### Start MariaDB Container

```bash
docker run --name mariadb-lumiere -e MYSQL_ROOT_PASSWORD=root -e MYSQL_DATABASE=lumiere -p 3306:3306 -d mariadb:latest
```

##### Verify Database Connection

```bash
docker ps
# Should show the mariadb-lumiere container running
```

#### 8. Configure Database Connection

Ensure your connection string in `Rise.Server/appsettings.json` matches your Docker setup:

```json
{
  "ConnectionStrings": {
    "MariaDb": "Server=localhost,3306;Database=lumiere;Uid=root;Pwd=root;"
  }
}
```

#### 9. Install Entity Framework Tools

```bash
dotnet tool install --global dotnet-ef --version 9.0.0
```

#### 10. Run Database Migrations

```bash
dotnet ef database update --startup-project Rise.Server --project Rise.Persistence
```

#### 11. Run the Application

```bash
dotnet run --project Rise.Server --no-build
```

The application will be available at:

- HTTPS: https://localhost:5001
- HTTP: http://localhost:5000

### Troubleshooting

#### Package Version Conflicts

If you encounter TypeLoadException or method implementation errors:

1. Check that all Entity Framework packages use the same version (9.0.x)
2. Update packages if needed:
   ```bash
   dotnet add Rise.Server package Microsoft.EntityFrameworkCore.Design --version 9.0.1
   ```

#### Database Connection Issues

1. Ensure Docker container is running: `docker ps`
2. Check connection string in `appsettings.json`
3. Verify database exists: `docker exec -it mariadb-lumiere mysql -uroot -proot -e "SHOW DATABASES;"`

#### Build Timeout Issues

If WebAssembly build times out, use the `--no-build` flag:

```bash
dotnet run --project Rise.Server --no-build
```

### Docker Database Management

#### Stop Database

```bash
docker stop mariadb-lumiere
```

#### Start Existing Database

```bash
docker start mariadb-lumiere
```

#### Remove Database (Warning: Data Loss)

```bash
docker stop mariadb-lumiere
docker rm mariadb-lumiere
```

## Creation of the database

To create the database, run the following command in the main folder `Rise`

```
dotnet ef database update --startup-project Rise.Server --project Rise.Persistence
```

> Make sure your connection string is correct in the `Rise/Server/appsettings.json` file.

## Migrations

Adapting the database schema can be done using migrations. To create a new migration, run the following command:

```
dotnet ef migrations add [MIGRATION_NAME] --startup-project Rise.Server --project Rise.Persistence
```

And then update the database using the following command:

```
dotnet ef database update --startup-project Rise.Server --project Rise.Persistence
```

## Tailwind

### Development

1. Install npm.
   ` npm i`
2. To develop with Tailwind, you need to run the following command in the `Rise/Client` folder:

```
npm run watch:css
```

### For production

The build will trigger a `app.min.css` file.
