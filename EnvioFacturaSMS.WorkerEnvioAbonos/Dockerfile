FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-bionic AS build
WORKDIR /src
COPY ["EnvioFacturaSMS.WorkerEnvioAbonos/EnvioFacturaSMS.WorkerEnvioAbonos.csproj", "EnvioFacturaSMS.WorkerEnvioAbonos/"]
COPY ["EnvioFacturaSMS.Applications/EnvioFacturaSMS.Applications.csproj", "EnvioFacturaSMS.Applications/"]
COPY ["EnvioFacturaSMS.Domain/EnvioFacturaSMS.Domain.csproj", "EnvioFacturaSMS.Domain/"]
COPY ["EnvioFacturaSMS.Infraestructure/EnvioFacturaSMS.Infraestructure.csproj", "EnvioFacturaSMS.Infraestructure/"]
RUN dotnet restore "EnvioFacturaSMS.WorkerEnvioAbonos/EnvioFacturaSMS.WorkerEnvioAbonos.csproj"
COPY . .
WORKDIR "/src/EnvioFacturaSMS.WorkerEnvioAbonos"
RUN dotnet build "EnvioFacturaSMS.WorkerEnvioAbonos.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EnvioFacturaSMS.WorkerEnvioAbonos.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EnvioFacturaSMS.WorkerEnvioAbonos.dll"]