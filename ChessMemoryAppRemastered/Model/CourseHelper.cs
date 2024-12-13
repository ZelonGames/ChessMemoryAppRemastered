using JChessLib;
using JChessLib.FEN;
using JChessLib.Courses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model;

public static class CourseHelper
{
    public static async Task<Course> CreateInstanceFromJson(string courseName)
    {
        string jsonData = await ChessTextToJson.GetContentFromFile($"Courses/{courseName}.json");
        return JsonConvert.DeserializeObject<Course>(jsonData)!;
    }

    // The old json file used only position FEN but I want the full FEN
    public static void UpdateFens(Course course)
    {
        var variations = course.Chapters!.SelectMany(x => x.Value!.Variations!);
        foreach (var variation in variations)
        {
            ChessBoardState chessBoard = ChessBoardFenGenerator.Generate(FenHelper.STATRING_FEN);
            foreach (var move in variation.Value!.Moves!)
            {
                LegalMove legalMove = MoveNotationHelper.TryGetLegalMoveFromNotation(chessBoard, move.MoveNotation!);
                chessBoard = MoveHelper.GetNextStateFromMove(legalMove);
                string fen = FenHelper.ConvertToFenString(chessBoard);
                move.UpdateFen(fen);
            }
        }

        string data = JsonConvert.SerializeObject(course);
        using (StreamWriter wr = new("C:\\dev\\test.json"))
        {
            wr.WriteLine(data);
        }
    }
}
