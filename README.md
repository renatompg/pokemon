# Busca e Captura de Pokémons

>  This is a challenge by [Coodesh](https://coodesh.com/)

## Descrição
Este projeto tem como objetivo gerenciar mestres de Pokémon e permitir a captura e listagem de Pokémons. Com ele, é possível listar detalhes de um Pokémon, cadastrar mestres, registrar a captura de Pokémons por um mestre e listar todos os Pokémons capturados.

Repositório do projeto: [GitHub](https://github.com/renatompg/pokemon)

## Tecnologias e Frameworks Utilizados
- **Linguagem**: C#
- **Frameworks**:
  - ASP.NET Core
  - Entity Framework Core
  - SQLite (para persistência dos dados de mestres)
- **Padrões de Projeto**:
  - Arquitetura em camadas (services, repositórios, API externa)
  - HATEOAS para navegabilidade entre endpoints
  - Versionamento de API externa (v1: chamada simples, v2: resiliência com retries)

## Recursos do Projeto
- **Cache e Resiliência**: Implementado com MemoryCache para melhorar o desempenho e evitar chamadas repetitivas à API externa. Em uma versão escalável, seria ideal usar cache distribuído, como Redis.
- **Controle de Versões e Migrations**: Utilização de migrations para versionamento das tabelas e controle das entidades.
- **Boas Práticas de Nomenclatura**: Padrão PascalCase para nomes de operações e DTOs em inglês, enquanto mensagens de exceções e comentários no código estão em português.

## Bibliotecas Utilizadas
- `Microsoft.AspNetCore.Mvc.NewtonsoftJson` (8.0.10): Integração do Newtonsoft.Json com ASP.NET Core.
- `Microsoft.AspNetCore.OpenApi` (8.0.10): Geração de documentação OpenAPI/Swagger.
- `Microsoft.EntityFrameworkCore` (8.0.10): ORM para interação com bancos de dados.
- `Microsoft.EntityFrameworkCore.Design` (8.0.10): Ferramentas de design para Entity Framework Core.
- `Microsoft.EntityFrameworkCore.Sqlite` (8.0.10): Suporte ao SQLite.
- `Microsoft.Extensions.Caching.Memory` (8.0.1): Cache em memória.
- `Microsoft.Extensions.Http` (8.0.1): Chamadas HTTP abstratas.
- `Microsoft.Extensions.Http.Polly` (8.0.10): Resiliência com Polly para chamadas HTTP.
- `Newtonsoft.Json` (13.0.3): Serialização e desserialização JSON.
- `Polly` (8.4.2): Implementação de retries e circuit breakers.
- `Swashbuckle.AspNetCore` (6.9.0): Documentação Swagger.
- `Swashbuckle.AspNetCore.Annotations` (6.9.0): Anotações de atributos Swagger.

## Endpoints
- **GET** `/api/pokemon/random`: Retorna uma lista com 10 Pokémons aleatórios.
- **GET** `/api/pokemon/{name}`: Retorna os detalhes de um Pokémon específico pelo nome.
- **GET** `/api/pokemon/master/{name}`: Obtém o registro mestre de um Pokémon específico.
- **POST** `/api/pokemon/master`: Cadastra um novo mestre de Pokémon.
- **POST** `/api/pokemon/master/capture`: Registra a captura de um Pokémon por um mestre.
- **GET** `/api/pokemon/master/capture`: Lista todos os Pokémons capturados por um mestre.

## Instalação e Execução

1. Clone o repositório:
   ```bash
   git clone https://github.com/renatompg/pokemon

2. Navegue até o diretório do projeto e configure o banco de dados SQLite:
   ```bash
   cd PokemonApi
   dotnet ef database update
   
3. Execute o projeto:
   ```bash
   dotnet run

``
## Observações sobre os Testes
Foram desenvolvidos testes de integração e testes unitários para o projeto. Os testes de integração abordam dois cenários principais: comunicação com a API externa e integração com o banco de dados local (SQLite).

## Decisões e Melhorias Futuras
Para um projeto em produção, a separação em múltiplos serviços e a utilização de um cache distribuído (como Redis) são estratégias recomendadas para suportar maior escalabilidade e performance.
