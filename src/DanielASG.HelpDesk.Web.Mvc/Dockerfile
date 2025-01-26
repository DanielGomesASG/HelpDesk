#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/DanielASG.HelpDesk.Web.Mvc/DanielASG.HelpDesk.Web.Mvc.csproj", "src/DanielASG.HelpDesk.Web.Mvc/"]
COPY ["src/DanielASG.HelpDesk.Web.Core/DanielASG.HelpDesk.Web.Core.csproj", "src/DanielASG.HelpDesk.Web.Core/"]
COPY ["src/DanielASG.HelpDesk.Application/DanielASG.HelpDesk.Application.csproj", "src/DanielASG.HelpDesk.Application/"]
COPY ["src/DanielASG.HelpDesk.Application.Shared/DanielASG.HelpDesk.Application.Shared.csproj", "src/DanielASG.HelpDesk.Application.Shared/"]
COPY ["src/DanielASG.HelpDesk.Core.Shared/DanielASG.HelpDesk.Core.Shared.csproj", "src/DanielASG.HelpDesk.Core.Shared/"]
COPY ["src/DanielASG.HelpDesk.Core/DanielASG.HelpDesk.Core.csproj", "src/DanielASG.HelpDesk.Core/"]
COPY ["src/DanielASG.HelpDesk.EntityFrameworkCore/DanielASG.HelpDesk.EntityFrameworkCore.csproj", "src/DanielASG.HelpDesk.EntityFrameworkCore/"]
COPY ["src/DanielASG.HelpDesk.GraphQL/DanielASG.HelpDesk.GraphQL.csproj", "src/DanielASG.HelpDesk.GraphQL/"]
RUN dotnet restore "src/DanielASG.HelpDesk.Web.Mvc/DanielASG.HelpDesk.Web.Mvc.csproj"
COPY . .
WORKDIR "/src/src/DanielASG.HelpDesk.Web.Mvc"
RUN dotnet build "DanielASG.HelpDesk.Web.Mvc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DanielASG.HelpDesk.Web.Mvc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DanielASG.HelpDesk.Web.Mvc.dll"]
