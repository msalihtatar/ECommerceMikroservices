FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim as build
WORKDIR /app
EXPOSE 80
COPY Core/*.csproj Core/

COPY Services/Order/OrderApplication/*.csproj Services/Order/OrderApplication/
COPY Services/Order/OrderCore/*.csproj Services/Order/OrderCore/
COPY Services/Order/OrderDomain/*.csproj Services/Order/OrderDomain/
COPY Services/Order/OrderInfrastructure/*.csproj Services/Order/OrderInfrastructure/
COPY Services/Order/OrderAPI/*.csproj Services/Order/OrderAPI/

RUN dotnet restore Services/Order/OrderAPI/*.csproj
COPY . .
RUN dotnet publish Services/Order/OrderAPI/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim as runtime
WORKDIR /app
COPY --from=build /app/out . 
ENTRYPOINT [ "dotnet","OrderAPI.dll" ]