FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim as build
WORKDIR /app
EXPOSE 80
COPY Core/*.csproj Core/
COPY Frontends/ECommerceWeb/*.csproj Frontends/ECommerceWeb/
RUN dotnet restore Frontends/ECommerceWeb/*.csproj
COPY . .
RUN dotnet publish Frontends/ECommerceWeb/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim as runtime
WORKDIR /app
COPY --from=build /app/out . 
ENTRYPOINT [ "dotnet","ECommerceWeb.dll" ]