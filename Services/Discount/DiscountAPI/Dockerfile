FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim as build
WORKDIR /app
EXPOSE 80
COPY Core/*.csproj Core/
COPY Services/Discount/DiscountAPI/*.csproj Services/Discount/DiscountAPI/
RUN dotnet restore Services/Discount/DiscountAPI/*.csproj
COPY . .
RUN dotnet publish Services/Discount/DiscountAPI/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim as runtime
WORKDIR /app
COPY --from=build /app/out . 
ENTRYPOINT [ "dotnet","DiscountAPI.dll" ]