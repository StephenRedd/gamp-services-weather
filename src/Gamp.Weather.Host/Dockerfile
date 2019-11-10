FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["src/Gamp.Weather.Host/Gamp.Weather.Host.csproj", "src/Gamp.Weather.Host/"]
COPY ["src/Gamp.Weather.Api/Gamp.Weather.Api.csproj", "src/Gamp.Weather.Api/"]
COPY ["src/Gamp.Weather.Domain.Ef.Sql/Gamp.Weather.Domain.Ef.Sql.csproj", "src/Gamp.Weather.Domain.Ef.Sql/"]
COPY ["src/Gamp.Weather.Domain.Ef/Gamp.Weather.Domain.Ef.csproj", "src/Gamp.Weather.Domain.Ef/"]
COPY ["src/Gamp.Weather.Core/Gamp.Weather.Core.csproj", "src/Gamp.Weather.Core/"]
COPY ["src/Gamp.Weather.Abstractions/Gamp.Weather.Abstractions.csproj", "src/Gamp.Weather.Abstractions/"]
COPY ["src/Gamp.Weather.Domain.Mongo/Gamp.Weather.Domain.Mongo.csproj", "src/Gamp.Weather.Domain.Mongo/"]
RUN dotnet restore "src/Gamp.Weather.Host/Gamp.Weather.Host.csproj"
COPY . .
WORKDIR "/src/src/Gamp.Weather.Host"
RUN dotnet build "Gamp.Weather.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gamp.Weather.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gamp.Weather.Host.dll"]