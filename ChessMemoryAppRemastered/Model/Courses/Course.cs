using ChessMemoryAppRemastered.Model.ChessBoard;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.Courses
{
    public class Course
    {
        [JsonProperty("previewFen")]
        public string PreviewFen {  get; private set; }
        [JsonProperty("color")]
        public PlayerColor Color { get; private set; }
        [JsonProperty("name")]
        public string Name { get; private set; }
        [JsonProperty("chessableCourseID")]
        public int ChessableCourseID { get; private set; }

        [JsonProperty("Chapters")]
        public Dictionary<string, Chapter> Chapters { get; private set; }

        public static async Task<Course> CreateInstanceFromJson(string courseName)
        {
            string jsonData = await ChessTextToJson.GetContentFromFile($"Courses/{courseName}.json");
            return JsonConvert.DeserializeObject<Course>(jsonData)!;
        }

        public Chapter? GetChapterByName(string name)
        {
            Chapters.TryGetValue(name, out var chapter);
            return chapter;
        }
    }
}
