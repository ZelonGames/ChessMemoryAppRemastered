using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.Mnemomics
{
    public class MnemonicsNotation
    {
        public static Random rnd = new();
        
        [JsonProperty]
        private Dictionary<string, Dictionary<string, List<string>>>? Coordinates { get; set; }

        public static async Task<MnemonicsNotation> CreateInstanceFromJson()
        {
            string jsonData = await ChessTextToJson.GetContentFromFile("chessMnemonicsNotations.json");
            return JsonConvert.DeserializeObject<MnemonicsNotation>(jsonData)!;
        }

        public List<string> GetWordsFromSquare(string square, int candidates)
        {
            return Coordinates![square.ToLower()][candidates.ToString()];
        }
    }
}