# 🤖 Discord Reaction Bot

Простой Discord-бот на C# (.NET 8), созданный для автоматических реакций на сообщения.
На текущем этапе реализовано безопасное подключение с чтением конфигурации из JSON.

## 📋 Требования

*   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
*   Созданный бот в [Discord Developer Portal](https://discord.com/developers/applications)

## 🚀 Установка и Запуск

### 1. Клонирование репозитория
```bash
git clone https://github.com/yarmi001/DiscordReactionBot.git
cd DiscordReactionBot
```

### 2. Настройка конфигурации 
Файл `config.json` исключен из репозитория для безопасности. Вам нужно создать его вручную.

1. Создайте файл `config.json` в папке проекта (рядом с `Program.cs` и `.csproj`).
2. Вставьте в него следующий код:

```json
{
  "Token": "ВСТАВЬТЕ_СЮДА_ВАШ_ТОКЕН_БОТА"
}
```

### 3. Настройка прав (Intents)
Чтобы бот мог работать, в [Discord Developer Portal](https://discord.com/developers/applications) (раздел **Bot** -> **Privileged Gateway Intents**) должны быть включены:
*   ✅ **Message Content Intent** (Обязательно!)

### 4. Запуск
Откройте терминал в папке проекта и выполните:

```bash
dotnet run
```

## 🛠 Структура проекта
*   `Program.cs` — Основная логика бота.
*   `config.json` — (Игнорируется Git) Хранит токен и настройки.
*   `.gitignore` — Список исключенных файлов.


