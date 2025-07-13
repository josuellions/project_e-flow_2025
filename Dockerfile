FROM mcr.microsoft.com/dotnet/sdk:8.0

# Define diretório base no container
WORKDIR /app

# Copia todos os arquivos do projeto para dentro do container
COPY . .

# Restaura dependências da solução
RUN dotnet restore TesteTecnico.sln

# Compila toda a solução (sem executar nada)
RUN dotnet build TesteTecnico.sln --no-restore