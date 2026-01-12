# Comandos Úteis - DOSFinal

## Compilar o projeto
```bash
dotnet build
```

## Executar a API
```bash
dotnet run --project src/DOSFinal.API/DOSFinal.API.csproj
```

## Restaurar dependências
```bash
dotnet restore
```

## Limpar build
```bash
dotnet clean
```

## Testar endpoints (exemplos com curl)

### Criar um produto
```bash
curl -X POST http://localhost:5000/api/products \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Produto Teste",
    "description": "Descrição do produto teste",
    "price": 99.99,
    "stock": 10
  }'
```

### Listar todos os produtos
```bash
curl http://localhost:5000/api/products
```

### Buscar produto por ID
```bash
curl http://localhost:5000/api/products/{id}
```

### Listar produtos ativos
```bash
curl http://localhost:5000/api/products/active
```

### Atualizar um produto
```bash
curl -X PUT http://localhost:5000/api/products/{id} \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Produto Atualizado",
    "description": "Nova descrição",
    "price": 149.99,
    "stock": 5,
    "isActive": true
  }'
```

### Deletar um produto
```bash
curl -X DELETE http://localhost:5000/api/products/{id}
```

## Adicionar pacotes NuGet (exemplos)

### Entity Framework Core
```bash
dotnet add src/DOSFinal.Infrastructure/DOSFinal.Infrastructure.csproj package Microsoft.EntityFrameworkCore
dotnet add src/DOSFinal.Infrastructure/DOSFinal.Infrastructure.csproj package Microsoft.EntityFrameworkCore.SqlServer
dotnet add src/DOSFinal.Infrastructure/DOSFinal.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Design
```

### AutoMapper
```bash
dotnet add src/DOSFinal.Application/DOSFinal.Application.csproj package AutoMapper
dotnet add src/DOSFinal.Application/DOSFinal.Application.csproj package AutoMapper.Extensions.Microsoft.DependencyInjection
```

### FluentValidation
```bash
dotnet add src/DOSFinal.Application/DOSFinal.Application.csproj package FluentValidation
dotnet add src/DOSFinal.Application/DOSFinal.Application.csproj package FluentValidation.DependencyInjectionExtensions
```
