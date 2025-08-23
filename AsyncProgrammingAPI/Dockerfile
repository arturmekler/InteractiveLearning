# AsyncProgrammingAPI/Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["AsyncProgrammingAPI.csproj", "."]
RUN dotnet restore "AsyncProgrammingAPI.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "AsyncProgrammingAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AsyncProgrammingAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Konfiguracja HTTPS - skopiuj certyfikat developmentu
ENV ASPNETCORE_URLS=http://+:5000;https://+:5001
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "AsyncProgrammingAPI.dll"]
