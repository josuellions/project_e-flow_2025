# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY TesteTecnico.sln ./
COPY TesteTecnico/ ./TesteTecnico/
COPY tests/UseCases.Test/ ./tests/UseCases.Test/

RUN dotnet restore ./TesteTecnico/TesteTecnico.csproj
RUN dotnet publish ./TesteTecnico/TesteTecnico.csproj -c Release -o /app/publish

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "TesteTecnico.dll"]
