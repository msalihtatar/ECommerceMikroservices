FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim as build
WORKDIR /app
EXPOSE 80
COPY Core/*.csproj Core/
COPY Services/Payment/PaymentAPI/*.csproj Services/Payment/PaymentAPI/
RUN dotnet restore Services/Payment/PaymentAPI/*.csproj
COPY . .
RUN dotnet publish Services/Payment/PaymentAPI/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim as runtime
WORKDIR /app
COPY --from=build /app/out . 
ENTRYPOINT [ "dotnet","PaymentAPI.dll" ]