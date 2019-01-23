﻿using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace GeneralPripp
{
    public class CommandHandler
    {
        private DiscordSocketClient _client;
        private CommandService _service;

        private char _prefix = '.';

        public CommandHandler(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            _service.AddModulesAsync(Assembly.GetEntryAssembly());
            _client.MessageReceived += MessageReceived;
            }

        private async Task MessageReceived(SocketMessage message)
        {
            var msg = message as SocketUserMessage;
            if (msg == null) return;

            var context = new SocketCommandContext(_client, msg);

            int argPos = 0;

            if (msg.HasCharPrefix(_prefix, ref argPos))
            {
                var result = await _service.ExecuteAsync(context, argPos);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    await context.Channel.SendMessageAsync(result.ErrorReason);
                }
            }

        }
    }
}