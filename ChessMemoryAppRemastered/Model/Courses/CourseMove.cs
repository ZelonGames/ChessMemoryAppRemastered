using ChessMemoryAppRemastered.Model.ChessBoard;
using ChessMemoryAppRemastered.Model.ChessBoard.Pieces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.Courses
{
    public class CourseMove
    {
        [JsonProperty("moveNotation")]
        public string MoveNotation { get; private set; }

        [JsonProperty("fen")]
        public string Fen { get; private set; }

        [JsonProperty("color")]
        public PlayerColor Color { get; private set; }

        // Used temporarily to update json files
        public void UpdateFen(string fen)
        {
            Fen = fen;
        }
    }
}
