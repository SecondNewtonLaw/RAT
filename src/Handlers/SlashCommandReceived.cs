using Discord;
using Discord.WebSocket;
using Spectre.Console;

namespace RAT;

internal partial class Handlers
{
    internal static async Task SlashComamndReceived(SocketSlashCommand cmdSocket)
    {
        AnsiConsole.MarkupLine($"[cyan]Command Executed[/]: [red bold underline]{cmdSocket.Data.Name}[/]!");

        switch (cmdSocket.Data.Name)
        {
            case "ping":
                await Commands.Ping(cmdSocket);
                break;
            case "rebuildcmd":
                await Commands.RebuildCommands(cmdSocket);
                break;
        }
    }
}