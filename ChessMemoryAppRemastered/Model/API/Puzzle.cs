using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChessMemoryAppRemastered.Model.API;

public class Puzzle
{
    public PuzzleGame? Game { get; private set; }
    [JsonProperty("puzzle")]
    public PuzzleData? Data {  get; private set; }
}

public class PuzzleGame
{
    public string? ID { get; private set; }
    public string? PGN { get; private set; }
}

public class PuzzleData
{
    public string? ID { get; private set; }
    public int Rating { get; private set; }
    public List<string>? Solution {  get; private set; }
    public List<string>? Themes { get; private set; }
    public int InitialPly {  get; private set; }
}
