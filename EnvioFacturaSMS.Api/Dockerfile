#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["EnvioFacturaSMS.Api/EnvioFacturaSMS.Api.csproj", "EnvioFacturaSMS.Api/"]
COPY ["EnvioFacturaSMS.Applications/EnvioFacturaSMS.Applications.csproj", "EnvioFacturaSMS.Applications/"]
COPY ["EnvioFacturaSMS.Domain/EnvioFacturaSMS.Domain.csproj", "EnvioFacturaSMS.Domain/"]
COPY ["EnvioFacturaSMS.Infraestructure/EnvioFacturaSMS.Infraestructure.csproj", "EnvioFacturaSMS.Infraestructure/"]
RUN dotnet restore "EnvioFacturaSMS.Api/EnvioFacturaSMS.Api.csproj"
COPY . .
WORKDIR "/src/EnvioFacturaSMS.Api"
RUN dotnet build "EnvioFacturaSMS.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EnvioFacturaSMS.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EnvioFacturaSMS.Api.dll"]