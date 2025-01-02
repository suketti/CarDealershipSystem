FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build


WORKDIR /App

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /App
COPY --from=build /App/out .
ENTRYPOINT ["dotnet", "Api.Gateway.dll"]
