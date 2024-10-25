
using ChessMemoryAppRemastered.Model.ChessBoard;
using ChessMemoryAppRemastered.Model.ChessBoard.FEN;
using ChessMemoryAppRemastered.Model.ChessBoard.Game;
using ChessMemoryAppRemastered.Model.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.ChessBot
{
    public class VariationMoveSelector
    {
        private readonly static Random rnd = new();
        private readonly List<string> variationNames = [
            "Arkhangelsk #1",
            "Arkhangelsk #2",
            "Arkhangelsk #3",
            "Arkhangelsk #4",
            "Arkhangelsk #5",
            "Arkhangelsk #6",
            "Arkhangelsk #7",
            "9...Ba7?",
            "10...Bxa5",
            "10...Nxa5 #1",
            "10...Nxa5 #2",
            "10...Nxa5 #3",
        ];
        private readonly List<Variation> variations = [];

        public VariationMoveSelector(Course course)
        {
            variations = course.Chapters.SelectMany(x => x.Value.Variations.Where(x => variationNames.Contains(x.Value.Name)).Select(x => x.Value)).ToList();
        }

        public string? TryGetRandomMoveNotationFromVariations(ChessBoardState chessBoardState)
        {
            string fen = FenHelper.ConvertToFenString(chessBoardState);
            var candidateVariations = variations.Where(x => x.Moves.Any(x => x.Fen == fen));
            var candidateMoves = new Dictionary<string, CourseMove>();

            foreach (var variation in candidateVariations)
            {
                CourseMove move = variation.Moves.Where(x => x.Fen == fen).First();
                int currentIndex = variation.Moves.IndexOf(move);
                if (currentIndex + 1 >= variation.Moves.Count)
                    continue;
                CourseMove nextMove = variation.Moves[currentIndex + 1];
                if (variation.Moves.Count > currentIndex + 1)
                    candidateMoves.TryAdd(nextMove.MoveNotation, nextMove);
            }
            if (candidateMoves.Count == 0)
                return null;

            return candidateMoves.Values.ToList()[rnd.Next(0, candidateMoves.Count)].MoveNotation;
        }
    }
}
