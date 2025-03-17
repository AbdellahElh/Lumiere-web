FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app
# Copy the .csproj files first to optimize build by using Docker layer caching
# Install the dotnet-ef tool globally
RUN dotnet tool install --global dotnet-ef \
&& dotnet tool install --global dotnet-format

# Add the global tools directory to the PATH
ENV PATH="$PATH:/root/.dotnet/tools"

RUN curl -sL https://deb.nodesource.com/setup_18.x | bash - && \
    apt-get install -y nodejs

# Copy the entire project and build it

# Copy the .csproj files first to optimize layer caching
COPY . ./
RUN npm install tailwind postcss-cli --save-dev \
    && npm install tailwindcss sass @tailwindcss/forms @tailwindcss/aspect-ratio @tailwindcss/typography --save-dev

# Restore .NET dependencies including test projects
RUN dotnet restore /app/Rise.Domain.Tests/Rise.Domain.Tests.csproj
RUN dotnet restore /app/Rise.Client.Tests/Rise.Client.Tests.csproj
RUN dotnet restore /app/Rise.Server.Tests/Rise.Server.Tests.csproj

# Restore the project dependencies
WORKDIR /app/Rise.Server
RUN dotnet restore

WORKDIR /app
RUN dotnet build --no-restore

RUN dotnet ef migrations add 20241028102751_initial.cs --startup-project Rise.Server --project Rise.Persistence

RUN dotnet ef database update --startup-project Rise.Server --project Rise.Persistence

RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory to /app in the runtime container
WORKDIR /app

# Copy the published output from the build container to the runtime container
COPY --from=build-env /app/out .

# Expose port 80 and 443 for the app
EXPOSE 80 443

# Copy the appsettings.json from Jenkins' template directory into the container (this overwrites the version from the repo)
COPY templates/appsettings.json /app/appsettings.json
RUN cat /app/appsettings.json


# Set the entry point to run the application
ENTRYPOINT ["dotnet", "Rise.Server.dll"]