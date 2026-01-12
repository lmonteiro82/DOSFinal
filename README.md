# DOSFinal - Estrutura Básica .NET

Este projeto segue a arquitetura em camadas (Clean Architecture) para organização do código.

## 📁 Estrutura do Projeto

```
DOSFinal/
├── src/
│   ├── DOSFinal.API/          # Camada de apresentação (Web API)
│   ├── DOSFinal.Application/  # Camada de aplicação (Casos de uso)
│   ├── DOSFinal.Domain/       # Camada de domínio (Entidades e regras de negócio)
│   └── DOSFinal.Infrastructure/ # Camada de infraestrutura (Acesso a dados, serviços externos)
└── DOSFinal.sln               # Arquivo de solução
```

## 🏗️ Camadas

### **DOSFinal.Domain**
- Contém as entidades de domínio
- Interfaces de repositórios
- Regras de negócio fundamentais
- Não possui dependências de outros projetos

### **DOSFinal.Application**
- Casos de uso da aplicação
- DTOs (Data Transfer Objects)
- Interfaces de serviços
- Depende apenas do Domain

### **DOSFinal.Infrastructure**
- Implementação de repositórios
- Acesso a banco de dados (Entity Framework, Dapper, etc.)
- Serviços externos (APIs, Email, etc.)
- Depende do Domain

### **DOSFinal.API**
- Controllers
- Configuração de middlewares
- Injeção de dependências
- Ponto de entrada da aplicação
- Depende de Application e Infrastructure

## 🚀 Como Executar

### Restaurar dependências
```bash
dotnet restore
```

### Compilar a solução
```bash
dotnet build
```

### Executar a API
```bash
dotnet run --project src/DOSFinal.API/DOSFinal.API.csproj
```

A API estará disponível em: `http://localhost:5000` ou `https://localhost:5001`

## 📦 Tecnologias

- .NET 9.0
- ASP.NET Core Web API
- C#

## 📝 Próximos Passos

1. Adicionar Entity Framework Core para acesso a dados
2. Implementar autenticação e autorização
3. Adicionar testes unitários e de integração
4. Configurar Docker
5. Implementar logging e monitoramento
