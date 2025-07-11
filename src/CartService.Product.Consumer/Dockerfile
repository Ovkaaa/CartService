# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only the project files needed for restore - improves caching
COPY CartService.Product.Consumer/*.csproj CartService.Product.Consumer/
COPY CartService.BLL/*.csproj CartService.BLL/
COPY CartService.DAL/*.csproj CartService.DAL/

RUN dotnet restore CartService.Product.Consumer/CartService.Product.Consumer.csproj

# Copy everything else
COPY . .

# Build and publish
RUN dotnet publish ./CartService.Product.Consumer/CartService.Product.Consumer.csproj -c Release -o /out

# Stage 2: Runtime
FROM mcr.microsoft.com/azure-functions/dotnet:4-dotnet8
WORKDIR /home/site/wwwroot

# Copy published output from build stage
COPY --from=build /out .

# Azure Functions runtime starts automatically
