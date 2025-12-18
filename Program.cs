using Discord;
using Discord.WebSocket;
using System.Text.Json;
// Чтение конфигурационного файла
if (!File.Exists("config.json"))
{
    Console.WriteLine("Файл config.json не найден");
    return;
}
// Десериализация конфигурации
string jsonString = await File.ReadAllTextAsync("config.json");
var config = JsonSerializer.Deserialize<BotConfig>(jsonString);
// Проверка наличия токена
if (string.IsNullOrEmpty(config?.Token))
{
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
    Console.WriteLine("\nБот подключен");
    return Task.CompletedTask;
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
}