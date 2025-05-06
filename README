# MS Professionals

Sistema de gerenciamento de profissionais e serviços.

## Tecnologias

- .NET 9.0
- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- MediatR
- FluentValidation
- AutoMapper
- Swagger/OpenAPI

## Estrutura do Projeto

```
MSProfessionals/
├── MSProfessionals.API/              # Camada de API
├── MSProfessionals.Application/      # Camada de Aplicação (Commands, Handlers, Validators)
├── MSProfessionals.Domain/           # Camada de Domínio (Entities, Interfaces)
├── MSProfessionals.Infrastructure/   # Camada de Infraestrutura (Repositories, DbContext)
└── MSProfessionals.UnitTests/        # Testes Unitários
```

## Requisitos

- .NET 9.0 SDK
- PostgreSQL
- Visual Studio 2022 ou VS Code

## Configuração

1. Clone o repositório
2. Configure a string de conexão no arquivo `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=ms_professionals;Username=postgres;Password=your_password"
  }
}
```

3. Execute as migrations na ordem correta:

```bash
# 1. Criar o banco de dados
dotnet ef database update --project MSProfessionals.Infrastructure --startup-project MSProfessionals.API --context ApplicationDbContext

# 2. Executar as migrations na ordem:
dotnet ef migrations add InitialCreate --project MSProfessionals.Infrastructure --startup-project MSProfessionals.API --context ApplicationDbContext
dotnet ef migrations add AddUniqueConstraintToProfessionalServices --project MSProfessionals.Infrastructure --startup-project MSProfessionals.API --context ApplicationDbContext
```

## Funcionalidades

### Profissionais
- Listagem paginada de profissionais
- Busca por nome (case-insensitive e sem acentos)
- Detalhes do profissional
- Criação de profissional
- Atualização de profissional
- Exclusão de profissional

### Profissões
- Listagem paginada de profissões
- Busca por nome (case-insensitive e sem acentos)
- Detalhes da profissão
- Criação de profissão
- Atualização de profissão
- Exclusão de profissão

### Serviços
- Listagem paginada de serviços
- Busca por nome (case-insensitive e sem acentos)
- Detalhes do serviço
- Criação de serviço
- Atualização de serviço
- Exclusão de serviço

## Endpoints

### Profissionais
- `GET /api/professional` - Lista profissionais paginados
- `GET /api/professional/{id}` - Obtém detalhes de um profissional
- `POST /api/professional` - Cria um novo profissional
- `PUT /api/professional/{id}` - Atualiza um profissional
- `DELETE /api/professional/{id}` - Remove um profissional

### Profissões
- `GET /api/profession` - Lista profissões paginadas
- `GET /api/profession/{id}` - Obtém detalhes de uma profissão
- `POST /api/profession` - Cria uma nova profissão
- `PUT /api/profession/{id}` - Atualiza uma profissão
- `DELETE /api/profession/{id}` - Remove uma profissão

### Serviços
- `GET /api/service` - Lista serviços paginados
- `GET /api/service/{id}` - Obtém detalhes de um serviço
- `POST /api/service` - Cria um novo serviço
- `PUT /api/service/{id}` - Atualiza um serviço
- `DELETE /api/service/{id}` - Remove um serviço

## Parâmetros de Paginação

Todos os endpoints de listagem suportam os seguintes parâmetros:
- `pageNumber` (padrão: 1) - Número da página
- `pageSize` (padrão: 10) - Tamanho da página
- `name` (opcional) - Filtro por nome (case-insensitive e sem acentos)

## Validações

- Validação de campos obrigatórios
- Validação de formatos (email, telefone, etc.)
- Validação de unicidade (profissões, serviços)
- Validação de relacionamentos

## Testes

Para executar os testes:
```bash
dotnet test
```

## Documentação da API

A documentação da API está disponível em:
- Swagger UI: `https://localhost:5001/swagger`
- OpenAPI JSON: `https://localhost:5001/swagger/v1/swagger.json`

## Observações

- Este é um projeto privado e não aceita contribuições externas
- As migrações devem ser executadas na ordem correta para garantir a integridade do banco de dados
- A busca por nome é case-insensitive e ignora acentuação
- Todas as operações são auditadas com data de criação e atualização 