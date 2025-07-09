# project_e-flow_2025

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
    - Leitura de arquivos;
  - Redução da lógica no `Program.cs`, tornando o código mais modular e manutenível.

## 🛠️ Tecnologias Utilizadas

- **ASP.NET C# (CSharp)
- **XML**

## ✅ Boas Práticas Aplicadas

- Organização por camadas;
- Separação de responsabilidades (SRP);
- Código limpo, modular e pronto para expansão futura.

---

> Projeto desenvolvido como simulação de manutenção e refatoração de sistemas fiscais com foco em boas práticas e clareza na implementação.

