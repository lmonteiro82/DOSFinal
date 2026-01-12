# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY DOSFinal.sln ./
COPY src/DOSFinal.API/DOSFinal.API.csproj src/DOSFinal.API/
COPY src/DOSFinal.Application/DOSFinal.Application.csproj src/DOSFinal.Application/
COPY src/DOSFinal.Domain/DOSFinal.Domain.csproj src/DOSFinal.Domain/
COPY src/DOSFinal.Infrastructure/DOSFinal.Infrastructure.csproj src/DOSFinal.Infrastructure/
COPY tests/DOSFinal.Tests/DOSFinal.Tests.csproj tests/DOSFinal.Tests/

# Restore dependencies
RUN dotnet restore

# Copy source code
COPY . .

# Build the application
RUN dotnet build -c Release --no-restore

# Run tests
RUN dotnet test -c Release --no-build --verbosity normal

# Publish the application
RUN dotnet publish src/DOSFinal.API/DOSFinal.API.csproj -c Release -o /app/publish --no-build

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# Copy published files
COPY --from=build /app/publish .

# Expose port
EXPOSE 80

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost/status || exit 1

# Entry point
ENTRYPOINT ["dotnet", "DOSFinal.API.dll"]
