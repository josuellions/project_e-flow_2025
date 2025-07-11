
<img width="987" height="541" alt="2025-07-11_ListaXML" src="https://github.com/user-attachments/assets/03633bc5-7703-4915-af04-878d5f597d5d" />

### Processo seletivo vaga Developer

##### Data: 09/07/2025 a 11/07/2025

##### Developer: Josuel A. Lopes

##### About

Projeto desenvolvido como simulação de manutenção e refatoração de sistemas fiscais com foco em boas práticas e clareza na implementação.

<br/>

# 🧾 Manutenção em Sistema de Emissão Fiscal (C#)

Este projeto tem como objetivo realizar a manutenção em um sistema legado de emissão de Notas Fiscais Eletrônicas (NF-e), desenvolvido em C#. A principal demanda envolve a correção de falhas na emissão de **notas fiscais de devolução**, bem como a aplicação de melhorias estruturais no código-fonte, seguindo boas práticas de organização por camadas.

## 🔧 Funcionalidades Implementadas

### 1. Correção na geração do XML para notas de devolução
- Ajuste da lógica existente para permitir a emissão de notas fiscais de devolução.
- Atualização apenas dos campos necessários:
  - `tpNF` (Tipo de operação);
  - `finNFe` (Finalidade da NF-e).
- Nenhum novo campo foi adicionado ao XML.

### 2. Listagem de XMLs gerados nos últimos 7 dias
- Criação de uma função que retorna os XMLs criados nos últimos 7 dias, exibindo:
  - A chave da nota (simulada/fictícia);
  - O CNPJ do destinatário.

### 3. Organização do código em camadas
- Refatoração da estrutura do projeto com separação de responsabilidades:
  - Criação de classes de serviço para encapsular a lógica principal;
  - Módulos distintos para:
    - Geração de XML;
    - Lista de XML;
  - Redução da lógica no `Program.cs`, tornando o código mais modular e manutenível.

## 🛠️ Tecnologias Utilizadas

- **ASP.NET Core 8 C# (CSharp)**,
- **XML**
- FluentAssert,
- Swagger,
- Bogus,
- Moq

## ✅ Boas Práticas Aplicadas

- Organização camadas pradrão MVC;
- Separação de responsabilidades;
- Código limpo, modular e pronto para expansão futura.

---

<br/>

#### Projeto: **project_e-flow_2025**

</br>

#### 📋 Sumário

---

- [📋 Sumário](#-sumário)
- [📂 Arquitetura e diretórios](#-arquitetura-e-diretórios)
- [📦 Pacotes](#-pacotes)
- [🧰 Dependências](#-dependências)
- [♻️ Variáveis de Ambiente](#-variáveis-de-ambiente)
- [🔥 Como executar](#-como-executar)
- [🧪 Testes](#-testes)
- [📜 Sugestões](#-sugestões)
- [💡 Melhorias](#-version)

<br/>

#### 📂 Arquitetura e diretórios

---

- **Padrão:** MVC (Model View Controller)

```txt
📦 root
 ┗ 📂 TesteTecnico (Projeto)
   ┣ 📜 Dependencias
   ┣ 📂 Properties
   ┣ 📂 App_Data (Dados e arquivos)
   ┣ 📂 Public (Dados e arquivos XML)
   ┣ 📂 src (Aplicação)
   ┃ ┣ 📂 Application (Centralizar e executar operações e gerenciamento de NF-e na aplicação)
   ┃ ┣ 📂 Communication (Classes de request e response)
   ┃ ┣ 📂 Contracts (Interface para persistência e consulta de NF-e)
   ┃ ┣ 📂 Controllers (Orquestrar as requisições e delegar as operações entre a camada e os serviços da lógica de negócio)
   ┃ ┣ 📂 Entities (Classes de persistência e leitura dos dados no **banco dados**)
   ┃ ┣ 📂 Repositories (Responsável por persistir e ler os dados no repositório da aplicação)
   ┃ ┣ 📂 UseCases (Implementa as regras de negocio para tratar os dados de percistência e retorno dos dados)
   ┃ ┣ 📂 Views (Camada de apresentação responsável por renderizar a interface com base nos dados do modelo)
   ┃ ┃  ┗ 📜 GerarViewNotas.cs (Monta a visualização estruturada de notas fiscais processadas, exibição dos dados restorno)
   ┃ ┗ 📂 Utils (Gera dados e estruturadas conforme o padrão da regra de negocio, de forma aleatória para fins de teste ou simulação)
   ┃    ┣ 📜 GerarChaveNFeRandom.cs (Gera chaves eletrônicas de NF-e válidas padrão da SEFAZ, de forma aleatória para teste ou simulação)
   ┃    ┣ 📜 GerarCNPJRandom.cs (Gera CNPJs válidos de forma aleatória, respeitando a estrutura oficial para teste e validação)
   ┃    ┗ 📜 GerarDateRandom.cs (Cria datas aleatórias dentro de um intervalo definido, útil para simulações e testes com dados temporais)
   ┣ 📂 tests
   ┃   ┗ 📂 UseCases.Test (Projeto TESTES)
   ┣ 📜 docker-compose.yaml
   ┣ 📜 Dockerfile.js
   ┣ 📜 .dockerignore
   ┣ 📜 .gitignore
   ┣ 📜 Program.cs
   ┣ 📜 README.md
   ┣ 📜 TesteTecnico.csproj
   ┗ 📜 TesteTecnico.sln

```

<br/>

#### 📦 Pacotes

---

- Versão do ASP.NET Core

    - `Core 8`

- Padronização do código

    - Configurações
        - `docker-compose.yml`
        - `Program.cs` 

<br/>

#### 🧰 Dependências

---

- Docker
    - Docker Compose
      - Criar e inicializar

```bash
docker compose up --build -d
docker ps
```

- Dados
    - File system
        - Criar os arquivos XMLs: `...\TesteTecnico\Public\Files\Notas`
        - Criar os arquivos Lista XMLs: `...\TesteTecnico\App_Data\Files\Logs`

<br/>

#### ♻️ Variáveis de Ambiente

---

- Certifique-se de ter configurado o arquivo `Program.cs` na raiz do projeto `TesteTecnico`, com as variáveis de ambiente necessárias para execução do projeto.
    - `gerar  -- modelo 55 -- tipo saida` ou `listar -- modelo 55 -- tipo saida`
    - `isDev = "ASPNETCORE_ENVIRONMENT" == "Development" | "Production"`

    - **NFeApp.cs**: Variavel Teste ou Desenvolvimento (Utilizado para gerar volume de XML, recomendado 1 a 999)
        - Responsável por centralizar e executar as operações relacionadas ao processamento e gerenciamento de NF-e na aplicação.
            - `...\TesteTecnico\src\Application\NFeApp.cs`
                - `QTD_GERAR_XML = 1; //1 a 999 `  
                
- Caso você não tenha acesso aos valores, solicite ao responsável pelo projeto.

<br/>

#### 🔥 Como executar

---

- Realize o clone do github ou baixe zip do projeto localmente.

    - Instalar ou atualizar os pacotes e dependências com gerenciador de pacotes

```
NuGet
```

- Para executar o projeto certifique de ter instalado `dotnet`.
	- Acesse o diretorio do projeto  `TesteTecnico`
        - Gerar XML
```bash
dotnet --version
dotnet run -- gerar  --modelo 55 --tipo saida .\TesteTecnico.csproj 
```
<br/>

        - Lista XML
```bash
dotnet run -- listar  --modelo 55 --tipo saida .\TesteTecnico.csproj 
```

<br/>

#### 🧪 Testes

---

- Teste Automatizados / Teste Integração
    - TDD (Test Driven Development)
        - Para executar o projeto em modo de test acesse projeto `tests\UseCases.Test`.

```bash
dotnet test
```

ou

```bash
dotnet test -v n
```

<br/>

#### 📜 Sugestões

---

- Refinamentos - Duvidas PO:

    - Aplicação deve aceitar outros tipos de notas
    - A lista deve retornar os XMLs somente do CNPJ de consulta
    - Deve ser possivel outro tipo de nota `entrada` e `saida`


#### 💡 Melhorias

---

- Sugestões Desenvolvedor:

    - Funcionalidade de envio de notificações por email na criação do XML.
    - Funcionalidade para realizar download do XML
    - Persistência os dados em um banco dados
    - Tratativa de erros customizados
    - Implementar um CI/CD
