FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5096
# Usuń EXPOSE 5001 - nie będziemy używać HTTPS

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

# ZMIANA: Tylko HTTP - bez HTTPS
ENV ASPNETCORE_URLS=http://+:5096
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "AsyncProgrammingAPI.dll"]
