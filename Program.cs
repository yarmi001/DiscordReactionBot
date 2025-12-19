using Discord;
using Discord.Net;
using Discord.WebSocket;
using System.Text.Json;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
// Чтение конфигурационного файла
if (!File.Exists("config.json"))
{
    Log("CRITICAL","Файл config.json не найден");
    return;
}

var jsonOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    ReadCommentHandling = JsonCommentHandling.Skip
};

// Десериализация конфигурации
string jsonString = await File.ReadAllTextAsync("config.json");
var config = JsonSerializer.Deserialize<BotConfig>(jsonString);
// Проверка наличия токена
if (config == null || string.IsNullOrEmpty(config.Token) || config.TargetUserId == 0)
{
    Log("CRITICAL", "Ошибка в config.json. Проверьте Токен и ID.");
    return;
}

// Настройка клиента Discord
var socketConfig = new DiscordSocketConfig
{
    GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent,
    LogGatewayIntentWarnings = false
};

using var client = new DiscordSocketClient(socketConfig);// Создание клиента

// Обработчики событий
client.Log += (LogMessage msg) => 
{
    Log(msg.Severity.ToString(), msg.Message ?? msg.Exception?.Message ?? "Unknown");
    return Task.CompletedTask;
};
// Событие готовности
client.Ready += () => 
{
    Log("INFO", $"Бот авторизован как {client.CurrentUser.Username}");
    Log("INFO", $"Цель (ID): {config.TargetUserId}");
    Log("INFO", $"Реакция: {config.ReactionEmoji}");
    return Task.CompletedTask;
};

client.MessageReceived += async (SocketMessage message) => 
{
    // Игнорирование сообщений от ботов
    if (message.Author.IsBot) return;

    // Проверка на целевого пользователя
    if (message.Author.Id == config.TargetUserId)
    {
        try
        {
            Log("DETECT", $"Сообщение от {message.Author.Username} в канале #{message.Channel.Name}");
            
            // Создаем эмодзи из строки в конфиге
            var emoji = new Emoji(config.ReactionEmoji);
            
            // Ставим реакцию
            await message.AddReactionAsync(emoji);
            Log("SUCCESS", "Реакция поставлена.");
        }
        catch (HttpException ex) when (ex.HttpCode == System.Net.HttpStatusCode.Forbidden)
        {
            // Специфическая ошибка: Нет прав ставить реакции
            Log("ERROR", "Нет прав ставить реакции в этом канале/сервере");
        }
        catch (Exception ex)
        {
            // Остальные ошибки
            Log("ERROR", $"Не удалось среагировать: {ex.Message}");
        }
    }
};

// Вход и запуск бота
try 
{
    await client.LoginAsync(TokenType.Bot, config.Token);
    await client.StartAsync();
}
catch (Exception ex)
{
    Log("CRITICAL", $"Ошибка подключения: {ex.Message}");
    return;
}

// Бесконечное ожидание
await Task.Delay(-1);
// Функция логирования с цветами
static void Log(string level, string message)
{
    switch (level.ToUpper())
    {
        case "CRITICAL": Console.ForegroundColor = ConsoleColor.Red; break;
        case "ERROR":    Console.ForegroundColor = ConsoleColor.DarkRed; break;
        case "SUCCESS":  Console.ForegroundColor = ConsoleColor.Green; break;
        case "DETECT":   Console.ForegroundColor = ConsoleColor.Cyan; break;
        case "INFO":     Console.ForegroundColor = ConsoleColor.White; break;
        default:         Console.ForegroundColor = ConsoleColor.Gray; break;
    }
    
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [{level}] {message}]");
    Console.ResetColor();
}

class BotConfig // Класс конфигурации бота
{
    public string Token { get; set; } = "";
    public ulong TargetUserId { get; set; }
    public string ReactionEmoji { get; set; } = "🐳";
}