FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim as build
WORKDIR /app
EXPOSE 80
COPY Core/*.csproj Core/
COPY Services/PhotoStock/PhotoAPI/*.csproj Services/PhotoStock/PhotoAPI/
RUN dotnet restore Services/PhotoStock/PhotoAPI/*.csproj
COPY . .
RUN dotnet publish Services/PhotoStock/PhotoAPI/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim as runtime
WORKDIR /app
COPY --from=build /app/out . 
ENTRYPOINT [ "dotnet","PhotoAPI.dll" ]