using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model
{
    public record struct Coordinate(int x, int y)
    {
        public readonly string ToAlphabeticCoordinate()
        {
            string rows = "abcdefgh";

            return rows[x].ToString() + (y + 1);
        }
    }
}
