using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model
{
    public record struct Coordinate(int X, int Y)
    {
        public const string rows = "abcdefgh";

        public readonly string ToAlphabeticCoordinate()
        {
            // 0,0 == A1
            return rows[X].ToString().ToUpper() + (Y + 1);
        }

        public static Coordinate FromAlphabeticCoordinate(string alphabeticCoordinate)
        {
            // A1 == 0,0
            alphabeticCoordinate = alphabeticCoordinate.ToLower();
            char alphabeticX = alphabeticCoordinate[0];
            int x = rows.IndexOf(alphabeticX);
            int y = (int)char.GetNumericValue(alphabeticCoordinate[1]) - 1;

            return new Coordinate(x, y);
        }
    }
}
