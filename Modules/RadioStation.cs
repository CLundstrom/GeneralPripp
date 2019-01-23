using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Discord;
using Discord.Commands;

namespace GeneralPripp.Modules
{
    public class RadioStation
    {
        //public int Index { get; private set; }
        public string Name { get; private set; }
        public string Url { get; private set; }
        public string Description { get; private set; }
        public RadioStation CurrentStation { get; private set; }
        public List<RadioStation> List { get; private set; }

        public RadioStation()
        {
            this.List = Json.LoadData();
            CurrentStation = List.First();
        }

    

        public RadioStation Next()
        {
            var currentIndex = List.IndexOf(CurrentStation);

            if (List.Count > currentIndex)
            {
                currentIndex++;
            }
            else
            {
                return CurrentStation;
            }

            return null;
        }

        public RadioStation Get(RadioStation station)
        {
            CurrentStation = List.Find(x => string.Equals(x.Name, station.Name, StringComparison.OrdinalIgnoreCase));

            return CurrentStation;
        }

        public RadioStation Get(string name)
        {
            CurrentStation = List.Find(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
            return CurrentStation;
        }

        public EmbedBuilder ListStations()
        {
            var embed = new EmbedBuilder()
            {
                Title = "Available networks",
                Color = Color.Blue
            };

            foreach (var radio in List)
            {
                embed.AddField(radio.Name, radio.Description);
            }
            return embed;
        }
        public EmbedBuilder RecentlyAdded()
        {
            try
            {
                var recent = List.LastOrDefault();
                var embed = new EmbedBuilder()
                {
                    Title = "Station Successfully added",
                    Color = Color.Blue
                };
                //embed.AddField("Index: ", recent.Index);
                embed.AddField("Name:", recent.Name);
                embed.AddField("Description:", recent.Description);
                embed.AddField("Url:", recent.Url);

                return embed;
            }

            catch (NullReferenceException e)
            {
                Console.WriteLine("Embed is null" + e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
            return null;
        }

        public EmbedBuilder NowPlaying(string name, string description)
        {
            var embed = new EmbedBuilder()
            {
                Title = "Now playing",
                Color = Color.Green
            };
            embed.AddField(name, description);
            return embed;
        }

        private void AddStation(string name, string description, string url)
        {
            var newStation = new RadioStation()
            {
                //Index = List.Count+1,
                Name = name,
                Description = description,
                Url = url,
            };
            List.Add(newStation);
            Json.SaveData(List);
        }

        public bool ValidateInput(string text)
        {
            if (text != null)
            {
                List<string> formatted = text.Split(',').ToList();
                if (formatted.Count != 3)
                {
                    Console.WriteLine(CommandError.UnknownCommand);
                    return false;
                }
                AddStation(formatted[0], formatted[1], formatted[2]);
            }
            return true;
        }
    }
}
