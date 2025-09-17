# Use .NET SDK for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Install Node.js
RUN curl -sL https://deb.nodesource.com/setup_18.x | bash - && \
    apt-get install -y nodejs

# Copy everything
COPY . ./

# Build CSS
WORKDIR /app/Rise.Client
RUN npm install && npm run build:css

# Build .NET app
WORKDIR /app
RUN dotnet restore
RUN dotnet build -c Release
RUN dotnet publish Rise.Server/Rise.Server.csproj -c Release -o ./publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/publish .

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://0.0.0.0:10000

EXPOSE 10000

ENTRYPOINT ["dotnet", "Rise.Server.dll"]