FROM mcr.microsoft.com/dotnet/sdk:9.0 as build
WORKDIR /app

COPY auction-microservice-app.sln ./
COPY src/SearchService/SearchService.csproj src/SearchService/
COPY src/Contracts/Contracts.csproj src/Contracts/

# Restore package deps
RUN dotnet restore ./src/SearchService/SearchService.csproj

# Copy the app folders over
COPY . .
RUN dotnet publish ./src/SearchService/SearchService.csproj -c Release -o /app/src/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
EXPOSE 80
COPY --from=build /app/src/out .
ENTRYPOINT [ "dotnet", "SearchService.dll" ]