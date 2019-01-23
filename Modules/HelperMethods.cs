using System.Net.Configuration;
using Discord.WebSocket;

namespace GeneralPripp.Modules
{
    class HelperMethods
    {

        public static SocketUserMessage ConvertMessage(SocketMessage msg)
        {
            var message = msg as SocketUserMessage;

            return message;

        }
 
    }
}
