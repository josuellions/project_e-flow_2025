# Projeto TesteTecnico (.NET 8) - Docker & Execução

Este repositório contém a aplicação .NET Core 8 `TesteTecnico` e seus testes automatizados.  
Este documento detalha a configuração Docker para build e execução, além dos comandos para rodar a aplicação e os testes via terminal dentro do container.

---

## Estrutura do Projeto

```
.
├── Dockerfile
├── docker-compose.yml
├── TesteTecnico.sln
├── TesteTecnico/
│ └── TesteTecnico.csproj
  ├── tests/
       └── UseCases.Test/
         └── UseCases.Test.csproj
```


---

## Dockerfile

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /app

COPY . .

RUN dotnet restore TesteTecnico.sln
RUN dotnet build TesteTecnico.sln --no-restore
 ```

---

## docker-compose.yml

```yaml
version: "3.9"

services:
  dev:
    build:
      context: .
      dockerfile: Dockerfile
    container_name: testetecnico-dev
    working_dir: /app
    stdin_open: true
    tty: true

```

## Construir a imagem Docker

```bash
docker compose build
```
- ou (sem docker-compose)
```bash
docker build -t e-flow .
```

## Rodar a aplicação via Docker

Executa a aplicação com parâmetros passados no CLI:

```bash
docker compose run dev dotnet run --project ./TesteTecnico/TesteTecnico.csproj -- gerar --modelo 55 --tipo saida
```

##  Rodar os testes via Docker (verbose)

Executa os testes com saída detalhada para debug:
`
```bash
docker compose run dev dotnet test ./tests/UseCases.Test/UseCases.Test.csproj --no-build --verbosity detailed
```
##  Abrir terminal interativo dentro do container

Para rodar comandos manualmente:

```bash
docker compose run dev bash
```

No shell do container, você pode executar:

```bash
dotnet run --project ./TesteTecnico/TesteTecnico.csproj -- gerar -- modelo 55 -- tipo saida
dotnet test ./TesteTecnico/tests/UseCases.Test/UseCases.Test.csproj --verbosity detailed
```

## Dicas Adicionais
Para subir o container em background e liberar seu terminal:

```bash
docker compose up -d
```
Para acompanhar os logs em tempo real:

```bash
docker compose logs -f
```