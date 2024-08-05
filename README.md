# Product API Service

Este projeto é uma API de serviço de produtos construída com .NET 8.0, utilizando Kafka para mensageria e SQLite como banco de dados. O projeto segue os princípios de DDD (Domain-Driven Design), SOLID e Clean Code.

## Tecnologias Utilizadas

- .NET 8.0
- Kafka
- SQLite
- Docker
- Automapper
- Dependency Injection
- Swagger

## Funcionalidades

1. Cadastro de produtos.
2. Publicação de produtos cadastrados em um tópico Kafka.
3. Busca de produtos com caching.
4. Atualização de produtos.
5. Documentação da API com Swagger.

## Requisitos

- .NET 8.0 SDK
- Docker
- Docker Compose

## Configuração do Ambiente
Passo 1: Execute o Docker Compose para iniciar Kafka e ZooKeeper:
Obs.: O comando deve ser executado no terminal, dentro do diretório onde encontra-se o arquivo docker-compose.yml.

docker-compose up -d

Passo 2: O banco de dados SQLite será criado automaticamente.

Passo 3: Executar Migrações do Entity Framework
No diretório do projeto, execute os seguintes comandos para aplicar as migrações do Entity Framework:

- add-migration InitialMigrate

- update-database

## Uso da API
Documentação Swagger
A documentação da API pode ser acessada via Swagger em https://localhost:7013/swagger/index.html ou http://localhost:5000/swagger/index.html.

Endpoints
- POST /api/product - Cadastro de um novo produto.
- GET /api/product/{id} - Busca de um produto por ID (com cache).
- PUT /api/product - Atualização de um produto.
- Contribuição
- Faça um fork do repositório.
- Crie uma branch para sua feature (git checkout -b feature/nova-feature).
- Commit suas mudanças (git commit -m 'Adicionar nova feature').
- Faça push para a branch (git push origin feature/nova-feature).
- Abra um Pull Request.
- Licença
Este projeto está licenciado sob a licença MIT. Veja o arquivo LICENSE para mais detalhes.

### Passo 1: Clonar o Repositório

```bash
git clone https://github.com/seu-usuario/ProductApiService.git
cd ProductApiService
