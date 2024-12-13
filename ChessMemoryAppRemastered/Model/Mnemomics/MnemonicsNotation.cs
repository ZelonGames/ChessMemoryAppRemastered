using JChessLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.Mnemomics;

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
        candidates = Math.Min(4, candidates);
        return Coordinates![square.ToLower()][candidates.ToString()];
    }

    public (int candidateNumber, Coordinate toCoordinate)? GetToCoordinateFromWord(string word)
    {
        foreach (var coordinates in Coordinates!)
        {
            var match = coordinates.Value
                .SelectMany(candidateNumber => candidateNumber.Value
                .Where(candidateWord => candidateWord == word)
                .Select(_ => (Convert.ToInt32(candidateNumber.Key), Coordinate.ConvertAlphabeticToCoordinate(coordinates.Key, PlayerColor.White))))
                .FirstOrDefault();

            if (match != default)
                return match;
        }

        return null;
    }
}