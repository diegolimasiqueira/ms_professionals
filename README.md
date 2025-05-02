# MSProfessionals

Microserviço para gerenciamento de profissionais, desenvolvido em ASP.NET Core 8.0 seguindo os princípios de Clean Architecture, Domain-Driven Design (DDD), CQRS e SOLID.

## Estrutura do Projeto

O projeto está organizado em camadas seguindo os princípios da Clean Architecture:

- **MSProfessionals.API**: Camada de apresentação, responsável por expor os endpoints da API
- **MSProfessionals.Application**: Camada de aplicação, contendo os casos de uso e regras de negócio
- **MSProfessionals.Domain**: Camada de domínio, contendo as entidades e interfaces
- **MSProfessionals.Infrastructure**: Camada de infraestrutura, implementando as interfaces do domínio
- **MSProfessionals.UnitTests**: Projeto de testes unitários

## Tecnologias Utilizadas

- .NET 8.0
- Entity Framework Core
- PostgreSQL
- MediatR
- Swagger/OpenAPI
- xUnit
- Moq

## Configuração do Banco de Dados

O projeto utiliza PostgreSQL como banco de dados. As configurações de conexão estão no arquivo `appsettings.json` e `appsettings.Development.json`.

### Estrutura do Banco de Dados

- Schema: `shc_professional`
- Tabelas:
  - `tb_country_codes`: Códigos de países
  - `tb_currencies`: Moedas
  - `tb_languages`: Idiomas
  - `tb_professionals`: Profissionais
  - `tb_professional_address`: Endereços dos profissionais
  - `tb_time_zones`: Fusos horários

## Executando o Projeto

1. Clone o repositório
2. Restaure os pacotes NuGet
3. Configure as strings de conexão no `appsettings.json`
4. Execute as migrations do Entity Framework
5. Execute o projeto

## Testes

Os testes unitários podem ser executados usando o comando:

```bash
dotnet test
```

## Documentação da API

A documentação da API está disponível através do Swagger UI quando o projeto é executado em ambiente de desenvolvimento.

## Licença

Este é um projeto privado e não está aberto para contribuições externas. 