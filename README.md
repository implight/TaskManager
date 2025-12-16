# TaskManager

1. Установить ASP.NET Core Runtime 8.0.22
```bash
https://dotnet.microsoft.com/en-us/download/dotnet/8.0
```

2. Установить PostgreSQL (тестировалось на версии 14.8)

3. Забрать проект https://github.com/implight/TaskManager.git

4. Перейти в папку `\TaskManager.WebAPI` и через командную строку выполнить 
```bash
dotnet publish -c Release -o ./publish
```

5. Перейти в папку `\publish` и создать файл `vault.json`:
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

Настроить параметры соответствующе.

6. Создать базу данных с указанным именем

7. В папке `\publish` через командную строку выполните 
```bash
dotnet TaskManager.WebAPI.dll --urls "http://localhost:5267"
```

Можно задать свой порт.

8. В браузере открыть 
```bash
http://localhost:5267/swagger/index.html
```
