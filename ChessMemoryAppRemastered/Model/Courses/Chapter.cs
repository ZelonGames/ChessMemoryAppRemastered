using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.Courses
{
    public class Chapter
    {
        [JsonProperty("variations")]
        public Dictionary<string, Variation> Variations { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        public Variation? GetVariationByName(string name)
        {
            Variations.TryGetValue(name, out var variation);
            return variation;
        }
    }
}
