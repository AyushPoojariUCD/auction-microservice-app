FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /app

COPY auction-microservice-app.sln ./
COPY src/IdentityService/IdentityService.csproj src/IdentityService/IdentityService.csproj
COPY src/Contracts/Contracts.csproj src/Contracts/

RUN dotnet restore ./src/IdentityService/IdentityService.csproj

COPY . . 
RUN dotnet publish ./src/IdentityService/IdentityService.csproj -c Release -o /app/src/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 80
COPY --from=build /app/src/out .
ENTRYPOINT [ "dotnet", "IdentityService.dll" ]