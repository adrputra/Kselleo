# Use .NET Core 3.1 SDK for building the client
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

# Copy project files and restore dependencies
COPY Client/*.csproj ./
RUN dotnet restore

# Copy the entire project and build it
COPY Client/. ./
RUN dotnet publish -c Release -o /out

# Use .NET Core 3.1 runtime image for running the client
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "Client.dll"]
