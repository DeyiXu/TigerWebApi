FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY TigerWebApi.sln ./
COPY Tiger.WebApi/Tiger.WebApi.csproj Tiger.WebApi/
COPY Tiger.WebApi.Core/Tiger.WebApi.Core.csproj Tiger.WebApi.Core/
COPY Tiger.WebApi.Core.Service/Tiger.WebApi.Core.Service.csproj Tiger.WebApi.Core.Service/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/Tiger.WebApi
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Tiger.WebApi.dll"]
