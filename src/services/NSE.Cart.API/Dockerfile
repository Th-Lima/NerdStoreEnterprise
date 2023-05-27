#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["./services/NSE.Cart.API/NSE.Cart.API.csproj", "services/NSE.Cart.API/"]
COPY ["./building blocks/NSE.Core/NSE.Core.csproj", "building blocks/NSE.WebAPI.Core/"]
COPY ["./building blocks/NSE.WebAPI.Core/NSE.WebAPI.Core.csproj", "building blocks/NSE.WebAPI.Core/"]
COPY ["./building blocks/NSE.MessageBus/NSE.MessageBus.csproj", "building blocks/NSE.MessageBus/"]

RUN dotnet restore "services/NSE.Cart.API/NSE.Cart.API.csproj"
COPY . .
WORKDIR "services/NSE.Cart.API"
RUN dotnet build "NSE.Cart.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NSE.Cart.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NSE.Cart.API.dll"]