#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-bionic AS build
WORKDIR /src
COPY ["EnvioFacturaSMS.CronJobContratos/EnvioFacturaSMS.CronJobContratos.csproj", "EnvioFacturaSMS.CronJobContratos/"]
COPY ["EnvioFacturaSMS.Applications/EnvioFacturaSMS.Applications.csproj", "EnvioFacturaSMS.Applications/"]
COPY ["EnvioFacturaSMS.Domain/EnvioFacturaSMS.Domain.csproj", "EnvioFacturaSMS.Domain/"]
COPY ["EnvioFacturaSMS.Infraestructure/EnvioFacturaSMS.Infraestructure.csproj", "EnvioFacturaSMS.Infraestructure/"]
RUN dotnet restore "EnvioFacturaSMS.CronJobContratos/EnvioFacturaSMS.CronJobContratos.csproj"
COPY . .
WORKDIR "/src/EnvioFacturaSMS.CronJobContratos"
RUN dotnet build "EnvioFacturaSMS.CronJobContratos.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EnvioFacturaSMS.CronJobContratos.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EnvioFacturaSMS.CronJobContratos.dll"]