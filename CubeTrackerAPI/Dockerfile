# Stage 1: Build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything and restore packages
COPY . .
RUN dotnet restore

# Publish the app
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Run the app (smaller, lighter image)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render assigns a port dynamically via the PORT env variable
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "CubeTrackerAPI.dll"]