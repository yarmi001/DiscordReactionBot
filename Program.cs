using Discord;
using Discord.WebSocket;
using System.Text.Json;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
// Чтение конфигурационного файла
if (!File.Exists("config.json"))
{
    Console.WriteLine("Файл config.json не найден");
    return;
}
// Десериализация конфигурации
string jsonString = await File.ReadAllTextAsync("config.json");

var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    ReadCommentHandling = JsonCommentHandling.Skip
};

var config = JsonSerializer.Deserialize<BotConfig>(jsonString);
// Проверка наличия токена
if (string.IsNullOrEmpty(config?.Token) || config.TargetUserId == 0)
{
    Console.WriteLine("ID = 0 или пустой токен в config.json");
    Console.WriteLine("Токен не найден внутри config.json");
    return;
}

// Настройка клиента Discord
var socketConfig = new DiscordSocketConfig
{
    GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
};

using var client = new DiscordSocketClient(socketConfig);// Создание клиента

// Обработчики событий
client.Log += (LogMessage log) => 
{
    Console.WriteLine(log.ToString());
    return Task.CompletedTask;
};
// Событие готовности
client.Ready += () => 
{
    Console.WriteLine($"\nБот подключен. Цель(ID): {config.TargetUserId}, Реакция: {config.ReactionEmoji}\n");
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
            Console.WriteLine($"[Detect] Сообщение от {message.Author.Username}. Ставлю реакцию...");
            
            // Создаем эмодзи из строки в конфиге
            var emoji = new Emoji(config.ReactionEmoji);
            
            // Ставим реакцию
            await message.AddReactionAsync(emoji);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error] Не удалось поставить реакцию: {ex.Message}");
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
    Console.WriteLine($"Ошибка: {ex.Message}");
    return;
}

// Бесконечное ожидание
await Task.Delay(-1);

class BotConfig // Класс конфигурации бота
{
    public string Token { get; set; } = "";
    public ulong TargetUserId { get; set; }
    public string ReactionEmoji { get; set; } = "🐳";
}