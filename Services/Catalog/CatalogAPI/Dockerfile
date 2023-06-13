FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim as build
WORKDIR /app
EXPOSE 80
COPY Core/*.csproj Core/
COPY Services/Catalog/CatalogAPI/*.csproj Services/Catalog/CatalogAPI/
RUN dotnet restore Services/Catalog/CatalogAPI/*.csproj
COPY . .
RUN dotnet publish Services/Catalog/CatalogAPI/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim as runtime
WORKDIR /app
COPY --from=build /app/out . 
ENTRYPOINT [ "dotnet","CatalogAPI.dll" ]
