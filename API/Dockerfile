# Use .NET Core 3.1 SDK for building the API
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

# Copy project files and restore dependencies
COPY API/*.csproj ./
RUN dotnet restore

# Install the compatible EF Core tools (version 3.1)
RUN dotnet tool install --global dotnet-ef --version 3.1.8

# Copy the entire project and build it
COPY API/. ./
RUN dotnet publish -c Release -o /out

# Use .NET Core 3.1 runtime image for running the API
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /out .

# Disable HTTPS
ENV ASPNETCORE_URLS=http://+:8999 

# Copy necessary files for migrations (optional)
COPY --from=build /app /app

# Run migrations before starting the app
WORKDIR /app/API  # Ensure we're in the correct directory
ENTRYPOINT ["dotnet", "API.dll", "migrate"]