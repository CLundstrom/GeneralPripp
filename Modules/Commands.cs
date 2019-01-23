using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;

namespace GeneralPripp.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        private readonly IAudioClient _audioClient = null;
        private readonly AudioHandler _audioHandler = new AudioHandler();

        [Command("Dippen")]
        public async Task Cmd0()
        {
            await Context.Channel.SendMessageAsync("är en preb");
        }

        [Command("Soppa")]
        public async Task Cmd1()
        {
            await Context.Channel.SendMessageAsync("dippens dopp i dippen");
        }

        [Command("doppa")]
        public async Task Cmd2()
        {
            await Context.Channel.SendMessageAsync("sippens sopp i doppen");
        }

        [Command("Lel")]
        public async Task Lebensraum()
        {
            var embed = new EmbedBuilder()
            {
                Title = "Wiki",
                Color = Color.Red,
                Description =
                    "Enkel artikel från wikipedia" +
                    "tbd"
            };

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("repeat")]
        [Summary("Echoes a message.")]
        [Alias("upprepa")]
        public async Task Repeat(string repeat)
        {
            await Context.Channel.SendMessageAsync(repeat);
        }

        [Command("stop")]
        [Summary("Echoes a message.")]
        [Alias("radioff")]
        public async Task EndRadio()
        {
            AudioHandler.StopTransmit();


            var embed = new EmbedBuilder()
            {
                Title = "Broadcast stopped",
                Color = Color.Red
            };
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("stations")]
        [Summary("Lists radio stations.")]
        [Alias("radio")]
        public async Task ListStations()
        {
            var radio = new RadioStation();
            var embed = radio.ListStations();

            await Context.Channel.SendMessageAsync("", false, embed);
        }

 
        [Command("addstation")]
        [Summary("Adds a radio station. Format: Name, description, url")]
        [Alias("addradio")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AddStation(string input)
        {
            RadioStation radio = new RadioStation();
            radio.ValidateInput(input);
            var embed = radio.RecentlyAdded();
            await Context.Channel.SendMessageAsync("", false, embed);
        }


        [Command("play", RunMode = RunMode.Async)]
        [Alias("radio")]
        public async Task Broadcast(string name)
        {
            // Fetching requested radiostation
            var radio = new RadioStation();
            radio = radio.Get(name);

            IVoiceChannel channel = null;
           
            // Get the users audio channel
            channel = (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null)
            {
                await Context.Channel.SendMessageAsync("User must be in a voice channel, or a voice channel must be passed as an argument.");
            }

            // Establish connection
            if (channel != null) await channel.ConnectAsync();

            await Task.Delay(2000);

            // Broadcast station
            var embed = radio.NowPlaying(radio.Name, radio.Description);
            await Context.Channel.SendMessageAsync("", false, embed);

            // Transmit
            await _audioHandler.TransmitAudio(_audioClient, radio.Url);

        }

        [Command("help", RunMode = RunMode.Async)]
        [Summary("Cancel Stream")]
        [Alias("cmds", "commands")]
        public async Task ListCommands()
        {
            var embed = new EmbedBuilder()
            {
                Title = "Available Commands",
                Color = Color.Purple
            };
            embed.AddField("Help", "Prints all available commands.");
            embed.AddField("Repeat \"message\", Upprepa \"message\"", "Echoes a message.");
            embed.AddField("Lel", "Test");
            embed.AddField("Addradio, Addstation \"Name, description, url\"", "Adds a station to the list. Requires Admin.");
            embed.AddField("Radio, Play \"channel\"", "Plays specified radiostream.");
            embed.AddField("Stations, Radio", "List available stations.");

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("userinfo", RunMode = RunMode.Async)]
        [Summary("Returns info about the current user, or the user parameter, if one passed.")]
        [Alias("user", "whois")]
        public async Task UserInfoAsync([Summary("The (optional) user to get info for")] SocketUser user = null)
        {
            var userInfo = user ?? this.Context.Client.CurrentUser;
            var messages = await this.Context.Channel.GetMessagesAsync()
                .Flatten();

            new LogMessage(LogSeverity.Info, userInfo.Username, "WHOIS");


            List<IMessage> msg = new List<IMessage>();
            foreach (var message in messages)
            {
                if (message.Author == userInfo)
                {
                    msg.Add(message);
                }
            }

            await ReplyAsync($"User ID: {userInfo.Username}#{userInfo.Discriminator}\nMessages posted in **#{Context.Channel.Name}**: **{msg.Count}** ");
        }

        [Command("purge", RunMode = RunMode.Async)]
        [Summary("Purges x amount of messages.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        //[RequireBotPermission(GuildPermission.Administrator)]
        [Alias("delete")]
        public async Task PurgeMessagesAsync(uint amount)
        {
            var messages = await Context.Channel.GetMessagesAsync((int)amount + 1)
                .Flatten();
            await Task.Delay(1000);
            await this.Context.Channel.DeleteMessagesAsync(messages);
            const int delay = 10000;

            var m = await this.ReplyAsync($"Purging {amount} messages. This message will be deleted in {delay / 1000}s");
            await Task.Delay(delay);
            await m.DeleteAsync();
        }


    }
}
