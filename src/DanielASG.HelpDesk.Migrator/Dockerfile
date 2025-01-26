#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/DanielASG.HelpDesk.Migrator/DanielASG.HelpDesk.Migrator.csproj", "src/DanielASG.HelpDesk.Migrator/"]
COPY ["src/DanielASG.HelpDesk.EntityFrameworkCore/DanielASG.HelpDesk.EntityFrameworkCore.csproj", "src/DanielASG.HelpDesk.EntityFrameworkCore/"]
COPY ["src/DanielASG.HelpDesk.Core/DanielASG.HelpDesk.Core.csproj", "src/DanielASG.HelpDesk.Core/"]
COPY ["src/DanielASG.HelpDesk.Core.Shared/DanielASG.HelpDesk.Core.Shared.csproj", "src/DanielASG.HelpDesk.Core.Shared/"]
RUN dotnet restore "src/DanielASG.HelpDesk.Migrator/DanielASG.HelpDesk.Migrator.csproj"
COPY . .
WORKDIR "/src/src/DanielASG.HelpDesk.Migrator"
RUN dotnet build "DanielASG.HelpDesk.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DanielASG.HelpDesk.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DanielASG.HelpDesk.Migrator.dll", "-s"]
