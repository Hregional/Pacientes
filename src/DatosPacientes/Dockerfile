#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
# Instalar utilidades para validar conexión a la DB
RUN apt-get update && apt-get install -y iputils-ping telnet && rm -rf /var/lib/apt/lists/*

#for testing
RUN apt-get update && apt-get install -y curl

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["DatosPacientes.csproj", "."]
RUN dotnet restore "./DatosPacientes.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "DatosPacientes.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DatosPacientes.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DatosPacientes.dll"]