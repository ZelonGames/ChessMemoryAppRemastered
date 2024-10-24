using ChessMemoryAppRemastered.Model.ChessBoard;
using ChessMemoryAppRemastered.Model.ChessBoard.FEN;
using ChessMemoryAppRemastered.Model.ChessBoard.Game;
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
        public string PreviewFen { get; private set; }
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

        // The old json file used only position FEN but I want the full FEN
        public void UpdateFens()
        {
            var variations = Chapters.SelectMany(x => x.Value.Variations);
            foreach (var variation in variations)
            {
                ChessBoardState chessBoard = ChessBoardFenGenerator.Generate(FenHelper.STATRING_FEN);
                foreach (var move in variation.Value.Moves)
                {
                    LegalMove legalMove = MoveNotationHelper.TryGetLegalMoveFromNotation(chessBoard, move.MoveNotation);
                    chessBoard = MoveHelper.GetNextStateFromMove(legalMove);
                    string fen = FenHelper.ConvertToFenString(chessBoard);
                    move.UpdateFen(fen);
                }
            }

            string data = JsonConvert.SerializeObject(this);
            using (StreamWriter wr = new("C:\\dev\\test.json"))
            {
                wr.WriteLine(data);
            }
        }

        public Chapter? GetChapterByName(string name)
        {
            Chapters.TryGetValue(name, out var chapter);
            return chapter;
        }
    }
}
