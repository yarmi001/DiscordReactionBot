
# 🤖 Discord Auto-Reaction Bot

Бот на C# (.NET 8), который автоматически ставит реакции (эмодзи) на все сообщения определённого пользователя.

## ✨ Возможности
*   ✅ **Безопасная конфигурация:** Токен и настройки хранятся отдельно.
*   ✅ **Фильтрация:** Реагирует только на указанного пользователя (по ID).
*   ✅ **Кастомизация:** Можно выбрать любой эмодзи для реакции.
*   ✅ **Логирование:** Выводит в консоль информацию о подключении и действиях.

## 📋 Требования
*   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
*   Бот в [Discord Developer Portal](https://discord.com/developers/applications)

## 🚀 Установка и Запуск

### 1. Клонирование
```bash
git clone https://github.com/yarmi001/DiscordReactionBot.git
cd DiscordReactionBot
```

### 2. Настройка (config.json)
Файл `config.json` **исключен из репозитория** для безопасности. Создайте его вручную в папке проекта.

**Содержимое `config.json`:**
```json
{
  "Token": "ВАШ_НОВЫЙ_ТОКЕН_БОТА",
  "TargetUserId": 415540086265085953,
  "ReactionEmoji": "🐳"
}
```

#### 📝 Пояснение полей:
*   `Token`: Токен вашего бота (Reset Token, если кто-то его узнал!).
*   `TargetUserId`: **Числовой** ID пользователя (без кавычек).
    *   *Как узнать:* Включите "Режим разработчика" в Discord (Настройки -> Расширенные), нажмите ПКМ на пользователя -> "Копировать ID".
*   `ReactionEmoji`: Любой смайл (Win + .), который бот будет ставить.

### 3. Права (Intents)
В [Developer Portal](https://discord.com/developers/applications) -> **Bot** -> **Privileged Gateway Intents** включите:
*   ✅ **Message Content Intent** (Чтобы бот видел сообщения)

### 4. Запуск
```bash
dotnet run
```

## 🛠 Структура
*   `Program.cs` — Логика подключения и обработки событий.
*   `config.json` — Секретные настройки (не загружать на GitHub!).
*   `.gitignore` — Правила исключения файлов.