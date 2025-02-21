# Use .NET Core 3.1 SDK for building
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

# Install OpenSSL for cert generation (Linux-based)
RUN apt-get update && apt-get install -y openssl

# Generate a self-signed HTTPS certificate
RUN dotnet dev-certs https -ep /https-certificate.pfx -p adrputra

# Use .NET Core 3.1 runtime for running the client
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app

# Copy the application from the build stage
COPY Client/. ./

ENV ASPNETCORE_URLS=http://+:3999

ENTRYPOINT ["dotnet", "Client.dll"]
