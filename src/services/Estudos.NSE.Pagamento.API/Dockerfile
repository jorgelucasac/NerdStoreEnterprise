#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["./services/Estudos.NSE.Pagamento.API/Estudos.NSE.Pagamentos.API.csproj", "services/Estudos.NSE.Pagamento.API/"]
COPY ["./building blocks/Estudos.NSE.Core/Estudos.NSE.Core.csproj", "building blocks/Estudos.NSE.Core/"]
COPY ["./building blocks/Estudos.NSE.WebApi.Core/Estudos.NSE.WebApi.Core.csproj", "building blocks/Estudos.NSE.WebAPI.Core/"]
COPY ["./building blocks/Estudos.NSE.MessageBus/Estudos.NSE.MessageBus.csproj", "building blocks/Estudos.NSE.MessageBus/"]

RUN dotnet restore "services/Estudos.NSE.Pagamento.API/Estudos.NSE.Pagamentos.API.csproj"
COPY . .
WORKDIR "services/Estudos.NSE.Pagamento.API"
RUN dotnet build "Estudos.NSE.Pagamentos.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Estudos.NSE.Pagamentos.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Estudos.NSE.Pagamentos.API.dll"]