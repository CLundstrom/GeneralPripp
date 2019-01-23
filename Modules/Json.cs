using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GeneralPripp.Modules
{
    internal class Json
    {
        private static readonly string _stations = "stations.dat";

        public static void SaveData(object obj, string filename)
        {
            string tmp = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText(filename, tmp);
        }

        public static void SaveData(object obj)
        {
            string tmp = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText(_stations, tmp);
        }

        public static List<RadioStation> LoadData()
        {
            string tmp = File.ReadAllText(_stations);

            //returns new list if null
            return JsonConvert.DeserializeObject<List<RadioStation>>(tmp) ?? new List<RadioStation> { };
            
        }
    }
}
