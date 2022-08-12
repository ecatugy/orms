FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR / app
EXPOSE 80 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["nuget.config", "/"]
COPY ["Orms.Api/Orms.Api.csproj", "Orms.Api/"]
COPY ["Orms.Persistence/Orms.Persistence.csproj", "Orms.Persistence/"]
COPY ["Orms.Domain/Orms.Domain.csproj", "Orms.Domain/"]
RUN dotnet restore "Orms.Api/Orms.Api.csproj"
COPY. .
WORKDIR "/src/Orms.Api"
RUN dotnet build "Orms.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Orms.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from= publish / app.
ENTRYPOINT["dotnet", "Auditoria.dll"]