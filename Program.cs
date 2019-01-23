using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace GeneralPripp
{
    internal class Program 
    {
        private static void Main(string[] args)
        => new Program().MainAsync().GetAwaiter().GetResult();

        //Connection
        private DiscordSocketClient _client;
        //Handler

        public async Task MainAsync()
        {
            var token = Environment.GetEnvironmentVariable("DiscordToken"); // Keep token private!
            _client = new DiscordSocketClient();
            _client.Log += Log;
            
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            //var test = new Commands();
            _client.Ready += () => Task.CompletedTask;

            var _handler = new CommandHandler(_client);
            
            
            // Block this task until the program is closed.
            await Task.Delay(-1);
        }


        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
