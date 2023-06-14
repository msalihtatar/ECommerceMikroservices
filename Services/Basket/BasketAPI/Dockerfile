FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim as build
WORKDIR /app
EXPOSE 80
COPY Core/*.csproj Core/
COPY Services/Basket/BasketAPI/*.csproj Services/Basket/BasketAPI/
RUN dotnet restore Services/Basket/BasketAPI/*.csproj
COPY . .
RUN dotnet publish Services/Basket/BasketAPI/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim as runtime
WORKDIR /app
COPY --from=build /app/out . 
ENTRYPOINT [ "dotnet","BasketAPI.dll" ]