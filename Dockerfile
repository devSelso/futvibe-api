FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/Futvibe.Domain/Futvibe.Domain.csproj", "src/Futvibe.Domain/"]
COPY ["src/Futvibe.Application/Futvibe.Application.csproj", "src/Futvibe.Application/"]
COPY ["src/Futvibe.Infrastructure/Futvibe.Infrastructure.csproj", "src/Futvibe.Infrastructure/"]
COPY ["src/Futvibe.WebApi/Futvibe.WebApi.csproj", "src/Futvibe.WebApi/"]
RUN dotnet restore "src/Futvibe.WebApi/Futvibe.WebApi.csproj"

COPY . .
RUN dotnet publish "src/Futvibe.WebApi/Futvibe.WebApi.csproj" \
    -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Futvibe.WebApi.dll"]
