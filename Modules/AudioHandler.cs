using System;
using System.Collections.Generic;
using Discord.Audio;
using Discord.Commands;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using Discord;

namespace GeneralPripp.Modules
{
    public class AudioHandler : ModuleBase<SocketCommandContext>
    {


        private static Process CreateStream(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-i {path} -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false, //true or false
                RedirectStandardOutput = true,
            });
        }

        public async Task TransmitAudio(IAudioClient client, string path)
        {
            var ffmpeg = CreateStream(path);
            var output = ffmpeg.StandardOutput.BaseStream;
            var discord = client.CreatePCMStream(AudioApplication.Music); // (, 1920)

            await output.CopyToAsync(discord, 1024);
            await discord.FlushAsync();
        }

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);

        public static void StopTransmit()
        {
            var message = "q"; //ffmpeg stop cmd
            Process[] generalPripps = Process.GetProcessesByName("GeneralPripp");
            if (generalPripps.Length == 0)
                return;
            if (generalPripps[0] != null)
            {
                IntPtr child = FindWindowEx(generalPripps[0].MainWindowHandle, new IntPtr(0), "Edit", null);
                SendMessage(child, 0x000C, 0, message);
            }

        }
        
        public void Queue(string song)
        {
            //playlist.Enqueue(song);

        }
    }
}