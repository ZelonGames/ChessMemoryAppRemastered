using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.API;

public class APIRequestHelper
{
    public static async Task<Puzzle?> GetPuzzle()
    {
        string url = $"https://lichess.org/api/puzzle/next?difficulty=hardest";

        try
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Puzzle>(jsonData);
            }
            return null;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
