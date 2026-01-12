# DOSFinal - Restaurant Reservation API

Sistema de gestão de reservas de mesas em restaurantes desenvolvido em .NET Core 9.

## 📋 Funcionalidades

- ✅ Criar, consultar, atualizar e cancelar reservas
- ✅ Deteção de conflitos de horários/mesas
- ✅ API REST com documentação Swagger
- ✅ Base de dados SQL Server com Entity Framework Code-First
- ✅ Testes unitários com xUnit (22 testes)
- ✅ Containerização com Docker
- ✅ Pipeline CI/CD com Jenkins e SonarQube
- ✅ Deployment em Kubernetes com Helm

## 📁 Estrutura do Projeto

```
DOSFinal/
├── src/
│   ├── DOSFinal.API/           # Web API (Controllers, Program.cs)
│   ├── DOSFinal.Application/   # Serviços e DTOs
│   ├── DOSFinal.Domain/        # Entidades e Interfaces
│   └── DOSFinal.Infrastructure/ # Repositórios e DbContext
├── tests/
│   └── DOSFinal.Tests/         # Testes unitários (xUnit)
├── helm/
│   └── dosfinal/               # Helm Chart para Kubernetes
├── Dockerfile                  # Build da API
├── docker-compose.yml          # API + SQL Server
├── Jenkinsfile                 # Pipeline CI/CD
└── sonar-project.properties    # Configuração SonarQube
```

## 🚀 Endpoints da API

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/status` | Health check |
| GET | `/api/reservations` | Listar todas as reservas |
| GET | `/api/reservations/{id}` | Detalhes de uma reserva |
| GET | `/api/reservations?date={date}` | Reservas por data |
| POST | `/api/reservations` | Criar reserva |
| PUT | `/api/reservations/{id}` | Atualizar reserva |
| DELETE | `/api/reservations/{id}` | Cancelar reserva |

## 💻 Como Executar

### Localmente
```bash
# Restaurar dependências
dotnet restore

# Executar testes
dotnet test

# Executar a API (requer SQL Server)
dotnet run --project src/DOSFinal.API/DOSFinal.API.csproj
```

### Com Docker
```bash
# Build e execução com docker-compose
docker-compose up --build

# A API estará disponível em: http://localhost:5000
# Swagger: http://localhost:5000/swagger
```

### Com Kubernetes (Helm)
```bash
# Instalar o Helm Chart
helm install dosfinal ./helm/dosfinal --namespace dosfinal --create-namespace

# Verificar status
kubectl get pods -n dosfinal
```

## 📊 Testes

O projeto inclui 22 testes unitários cobrindo:
- `ReservationService` - Lógica de negócio e validação de conflitos
- `Reservation` - Entidade de domínio
- `ReservationDto` - Data Transfer Objects

```bash
# Executar testes com cobertura
dotnet test --collect:"XPlat Code Coverage"
```

## 🔧 Pipeline CI/CD (Jenkins)

O Jenkinsfile inclui os seguintes stages:
1. **Checkout** - Obter código fonte
2. **Restore** - Restaurar dependências
3. **Build** - Compilar solução
4. **Test** - Executar testes unitários
5. **SonarQube** - Análise de qualidade de código
6. **Quality Gate** - Validar métricas
7. **Docker Build** - Criar imagem Docker
8. **Docker Push** - Publicar no registry
9. **Deploy** - Deployment em Kubernetes via Helm

## 📝 Modelo de Dados

```sql
CREATE TABLE Reservations (
    Id INT PRIMARY KEY IDENTITY,
    CustomerName NVARCHAR(100) NOT NULL,
    ReservationDate DATE NOT NULL,
    ReservationTime TIME NOT NULL,
    TableNumber INT NOT NULL,
    NumberOfPeople INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);
```

## 🛠️ Tecnologias

- .NET 9.0
- ASP.NET Core Web API
- Entity Framework Core 9.0
- SQL Server 2022
- xUnit + Moq
- Docker & Docker Compose
- Jenkins
- SonarQube
- Kubernetes + Helm

## 👥 Autores

DOSFinal Team - 2026
