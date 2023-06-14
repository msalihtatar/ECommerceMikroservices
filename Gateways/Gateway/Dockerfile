FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim as build
WORKDIR /app
EXPOSE 80

COPY Gateways/Gateway/*.csproj Gateways/Gateway/
RUN dotnet restore Gateways/Gateway/*.csproj
COPY . .
RUN dotnet publish Gateways/Gateway/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim as runtime
WORKDIR /app
COPY --from=build /app/out . 
ENTRYPOINT [ "dotnet","Gateway.dll" ]