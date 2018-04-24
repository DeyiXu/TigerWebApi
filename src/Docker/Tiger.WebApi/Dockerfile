FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY Tiger.WebApi/Tiger.WebApi.csproj Tiger.WebApi/
RUN dotnet restore Tiger.WebApi/Tiger.WebApi.csproj
COPY . .
WORKDIR /src/Tiger.WebApi
RUN dotnet build Tiger.WebApi.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Tiger.WebApi.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Tiger.WebApi.dll"]
