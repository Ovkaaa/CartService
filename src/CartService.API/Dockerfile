# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj(s) and restore dependencies
COPY CartService.API/CartService.API.csproj ./CartService.API/
COPY CartService.BLL/CartService.BLL.csproj ./CartService.BLL/
COPY CartService.DAL/CartService.DAL.csproj ./CartService.DAL/

RUN dotnet restore ./CartService.API/CartService.API.csproj

# Copy remaining source code
COPY . .

# Build and publish release build to /app/publish
RUN dotnet publish ./CartService.API/CartService.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/publish .

# Expose port
EXPOSE 8080

# Run the app
ENTRYPOINT ["dotnet", "CartService.API.dll"]
