# Importador de Catálogos

Este projeto é um serviço de importação de catálogos que permite a inserção e manipulação de dados de catálogos. Ele utiliza Entity Framework Core para acesso ao banco de dados e pode ser executado em um contêiner Docker.

## Pré-requisitos

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Docker](https://www.docker.com/get-started)
- [Entity Framework Core Tools](https://docs.microsoft.com/ef/core/cli/dotnet)

## Configuração do Ambiente

### 1. Clonar o Repositório

```bash
git clone https://github.com/seu-usuario/seu-repositorio.git
cd seu-repositorio
```

### 2. Configurar o Docker

#### 2.1. Construir a Imagem Docker

```bash
docker build -t importador-catalogos .
```

#### 2.2. Executar o Contêiner Docker

```bash
docker run -d -p 5000:80 --name importador-catalogos importador-catalogos
```

### 3. Instalar Entity Framework Core Tools

```bash
dotnet tool install --global dotnet-ef
```

### 4. Atualizar o Banco de Dados

#### 4.1. Adicionar a String de Conexão

Adicione a string de conexão ao arquivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ImportadorCatalogos;User Id=seu-usuario;Password=sua

-s

enha;"
  }
}
```

#### 4.2. Executar as Migrações

```bash
dotnet ef database update
```

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

## Contribuição

1. Faça um fork do projeto.
2. Crie uma branch para sua feature (`git checkout -b feature/nova-feature`).
3. Commit suas mudanças (`git commit -am 'Adiciona nova feature'`).
4. Faça um push para a branch (`git push origin feature/nova-feature`).
5. Crie um novo Pull Request.

## Licença

Este projeto está licenciado sob a Licença MIT. Veja o arquivo LICENSE para mais detalhes.

---

Com este guia, você deve ser capaz de configurar e executar o projeto de importação de catálogos, bem como entender e utilizar os endpoints disponíveis.
