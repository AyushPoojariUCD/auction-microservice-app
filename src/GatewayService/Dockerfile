FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY auction-microservice-app.sln ./
COPY src/GatewayService/GatewayService.csproj src/GatewayService/GatewayService.csproj

RUN dotnet restore ./src/GatewayService/GatewayService.csproj

# Copy the app folders over
COPY . .
RUN dotnet publish ./src/GatewayService/GatewayService.csproj -c Release -o /app/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
EXPOSE 80
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet", "GatewayService.dll" ]
