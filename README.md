# Importador de Catálogos

Este projeto é um serviço de importação de catálogos que permite a inserção e manipulação de dados de catálogos. Ele utiliza Entity Framework Core para acesso ao banco de dados e pode ser executado em um contêiner Docker.

## ⚙️ Mecanismos Arquiteturiais

|Análise            |	Design                                      |	Implementação    |
|-------------------|---------------------------------------------|------------------|
|Persistência       |	ORM	                                        | Entity Framework |
|Persistência       |	Banco de dados relacional                   | SqlServer        |
|Back-end	          |  Arquitetura em camadas                     |	.Net8            |
|Documentação de API|Solução para documentação das APIs da solução|	Swagger          |
|Teste de Software  | 	Teste unitários	                          | xUnit            |

## ⚙️ Estrutura Backend

Desenvolvido em DDD (Domain Driven Design) é uma modelagem de software na qual o objetivo é facilitar a implementação de regras e processos, onde visa a divisão de responsabilidades por camadas e é independente da tecnologia utilizada. Ou seja, o DDD é uma filosofia voltado para o domínio do negócio.

* Aplicação: Porta de entrada, responsável por receber as requisições e direcioná-las para camadas mais internas.
* Domínio: Responsável pelo Core do projeto, contendo classes e interfaces que poderão ser utilizadas para compor as regras de negócio.
* Serviço: Um dos responsáveis pelo Core do projeto, onde é utilizado o que há na camada Domínio para realizar, de fato, as regras de negócio.
* Repositorio: Camada para comunicação externa, ou seja, pela comunicação com banco de dados, realizando operações.

## ⚙️ Estrutura do projeto
* Aplicação – Projeto responsável pela organização e exposição das rotas da API.
* Servico – Projeto que contém toda a regra de negócio da API.
* Repositorio – Projeto responsável por centralizar a comunicação com o banco de dados.
* Dominio– Projeto responsável por centralizar todo o Core do projeto.
* TesteUnitario – Projeto com os testes unitários da API.


## Pré-requisitos

- [.NET 8 ](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Docker](https://www.docker.com/get-started)
- [Entity Framework Core Tools](https://docs.microsoft.com/ef/core/cli/dotnet)
- 
 ## Observaçôes importantes:
 - O Arquivo importado deve estar no formato CSV e separado por virgula]
 - O Catalago sera criado na hora da importacao com o mesmo nome do arquivo
 - Para inserir novos dados do catalago existente usar a rota `POST /InserirNovoCatalago`
 - Para inserir um subcatalago passando apenas uma coluna como no arquivo exemplo meusPokemons utilizar a rota `POST /InserirDadosCatalagosExistentes`
 - Para obter a coluna de um catalago para auxiliar no filtro existe a rota `GET /ObterColunaPorCatalago`
 - Para executar os testes unitarios, navegar ate a psta ImportadorServico.Testes e executar o comando  `dotnet test`

- [.NET 8 ](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Docker](https://www.docker.com/get-started)
- [Entity Framework Core Tools](https://docs.microsoft.com/ef/core/cli/dotnet)

## Configuração do Ambiente

### 1. Clonar o Repositório

```bash
git clone https://github.com/anaJuliaPaixao/importador-de-dados-csv.git

```
#### 2. Executar o Contêiner Docker
- vá para a pasta raiz do projeto
- execute:
```bash
cd Importador.Aplicacao
```
- Depois:
```bash
sudo docker compose up -d
```

### 3. Instalar Entity Framework Core Tools

```bash
dotnet tool install --global dotnet-ef
```

### 4. Atualizar o Banco de Dados

#### 4.1. Ajustar caso preciso a String de Conexão

A string de conexao esta no arquivo `appsettings.json`:

#### 4.2. Executar as Migrações

```bash
dotnet ef database update  --project Importador.Repositorio.csproj --startup-project ../Importador.Aplicacao
```
- garanta que o comando execute dentro da pasta Importador.Repositorio

## Endpoints

### 1. `GET /ObterColunaPorCatalago`

#### Descrição

Obtém as colunas de um catálogo específico.

#### Parâmetros

- [`nomeCatalago`](command:_github.copilot.openSymbolFromReferences?%5B%22nomeCatalago%22%2C%5B%7B%22uri%22%3A%7B%22%24mid%22%3A1%2C%22fsPath%22%3A%22%2Fhome%2Fpaulohenrique%2F%C3%81rea%20de%20Trabalho%2FTeste%20Ana%2FImportador.Servico%2FServicos%2FImportadorServicos.cs%22%2C%22external%22%3A%22file%3A%2F%2F%2Fhome%2Fpaulohenrique%2F%25C3%2581rea%2520de%2520Trabalho%2FTeste%2520Ana%2FImportador.Servico%2FServicos%2FImportadorServicos.cs%22%2C%22path%22%3A%22%2Fhome%2Fpaulohenrique%2F%C3%81rea%20de%20Trabalho%2FTeste%20Ana%2FImportador.Servico%2FServicos%2FImportadorServicos.cs%22%2C%22scheme%22%3A%22file%22%7D%2C%22pos%22%3A%7B%22line%22%3A180%2C%22character%22%3A78%7D%7D%5D%5D "Go to definition") (query): Nome do catálogo.

#### Exemplo de Requisição

```http
GET /ObterColunaPorCatalago?nomeCatalago=nomeDoCatalago
```

#### Exemplo de Resposta

```json
[
  "Coluna1",
  "Coluna2",
  "Coluna3"
]
```

### 2. `POST /InserirNovoCatalago`

#### Descrição

Insere um novo catálogo e suas colunas.

#### Parâmetros

- `conteudo` (body): Dados do catálogo em formato `DataTable`.
- `nomeArquivo` (query): Nome do arquivo do catálogo.

#### Exemplo de Requisição

```http
POST /InserirNovoCatalago?nomeArquivo=nomeDoArquivo
Content-Type: application/json

{
  "colunas": ["Coluna1", "Coluna2"],
  "dados": [
    {"Coluna1": "Valor1", "Coluna2": "Valor2"},
    {"Coluna1": "Valor3", "Coluna2": "Valor4"}
  ]
}
```

#### Exemplo de Resposta

```json
{
  "mensagem": "Catálogo inserido com sucesso."
}
```

### 3. `POST /InserirDadosCatalagosExistentes`

#### Descrição

Insere dados em catálogos existentes.

#### Parâmetros

- `conteudo` (body): Dados do catálogo em formato `DataTable`.
- [`catalago`](command:_github.copilot.openSymbolFromReferences?%5B%22catalago%22%2C%5B%7B%22uri%22%3A%7B%22%24mid%22%3A1%2C%22fsPath%22%3A%22%2Fhome%2Fpaulohenrique%2F%C3%81rea%20de%20Trabalho%2FTeste%20Ana%2FImportador.Servico%2FServicos%2FImportadorServicos.cs%22%2C%22external%22%3A%22file%3A%2F%2F%2Fhome%2Fpaulohenrique%2F%25C3%2581rea%2520de%2520Trabalho%2FTeste%2520Ana%2FImportador.Servico%2FServicos%2FImportadorServicos.cs%22%2C%22path%22%3A%22%2Fhome%2Fpaulohenrique%2F%C3%81rea%20de%20Trabalho%2FTeste%20Ana%2FImportador.Servico%2FServicos%2FImportadorServicos.cs%22%2C%22scheme%22%3A%22file%22%7D%2C%22pos%22%3A%7B%22line%22%3A173%2C%22character%22%3A59%7D%7D%5D%5D "Go to definition") (query): Objeto [`Catalago`](command:_github.copilot.openSymbolFromReferences?%5B%22Catalago%22%2C%5B%7B%22uri%22%3A%7B%22%24mid%22%3A1%2C%22fsPath%22%3A%22%2Fhome%2Fpaulohenrique%2F%C3%81rea%20de%20Trabalho%2FTeste%20Ana%2FImportador.Servico%2FServicos%2FImportadorServicos.cs%22%2C%22external%22%3A%22file%3A%2F%2F%2Fhome%2Fpaulohenrique%2F%25C3%2581rea%2520de%2520Trabalho%2FTeste%2520Ana%2FImportador.Servico%2FServicos%2FImportadorServicos.cs%22%2C%22path%22%3A%22%2Fhome%2Fpaulohenrique%2F%C3%81rea%20de%20Trabalho%2FTeste%20Ana%2FImportador.Servico%2FServicos%2FImportadorServicos.cs%22%2C%22scheme%22%3A%22file%22%7D%2C%22pos%22%3A%7B%22line%22%3A173%2C%22character%22%3A50%7D%7D%5D%5D "Go to definition") existente.

#### Exemplo de Requisição

```http
POST /InserirDadosCatalagosExistentes
Content-Type: application/json

{
  "colunas": ["Coluna1", "Coluna2"],
  "dados": [
    {"Coluna1": "Valor1", "Coluna2": "Valor2"},
    {"Coluna1": "Valor3", "Coluna2": "Valor4"}
  ]
}
```

#### Exemplo de Resposta

```json
{
  "mensagem": "Dados inseridos com sucesso."
}
```

