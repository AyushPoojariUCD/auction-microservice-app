FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY auction-microservice-app.sln ./
COPY src/AuctionService/AuctionService.csproj src/AuctionService/AuctionService.csproj
COPY src/Contracts/Contracts.csproj src/Contracts/

RUN dotnet restore ./src/AuctionService/AuctionService.csproj

# Copy the app folders over
COPY . .
RUN dotnet publish ./src/AuctionService/AuctionService.csproj -c Release -o /app/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
EXPOSE 80
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet", "AuctionService.dll" ]
