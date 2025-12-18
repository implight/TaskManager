# TaskManager

## Установка и запуск

1. Установить [ASP.NET Core Runtime 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

2. Установить PostgreSQL (тестировалось на версии 14.8)

3. Забрать проект
```bash
git clone https://github.com/implight/TaskManager.git
```

5. Перейти в папку `\TaskManager\TaskManager.WebAPI` и через командную строку выполнить 
```bash
dotnet publish -c Release -o ./publish
```

5. Перейти в папку `\publish` и создать файл `vault.json` с нужными настройками:
```bash
{
  "Database": {
	"Database": "task_manager",
    "Host": "localhost",
    "Port": 5432,
    "Username": "postgres",
    "Password": "postgres"
  },
  "Jwt": {
    "SECRET": "<Enter your secret-key>"
  }
}
```

6. В PostgreSQL создать базу данных с указанным именем

7. В папке `\publish\appsettings.Production.json` указать нужные порты для Http/Https

8. В папке `\publish` через командную строку выполнить
```bash
dotnet TaskManager.WebAPI.dll
```

8. В браузере открыть (указать порт)
```bash
http://localhost:5005/swagger/index.html
```

9. При тестировании указывать (опционно)
```bash
   X-API-Version: 1
```
## Архитектура

### Слои
- **Domain** - Бизнес-логика, сущности, доменные события
- **Application** - Use cases, CQRS с MediatR, DTOs, валидация
- **Infrastructure** - Доступ к данным (EF Core + PostgreSQL), кеширование, фоновые сервисы
- **WebAPI** - Controllers, middleware, authentication, Swagger

### Паттерны
- Clean Architecture
- Domain-Driven Design (DDD)
- CQRS c MediatR
- Vertical Slice Architecture
- Repository Pattern
- Unit of Work
- Result Pattern
- Factory Pattern
