using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.ChessBoard
{
    public enum CastlingMove
    {
        WhiteKingSide,
        WhiteQueenSide,
        BlackKingSide,
        BlackQueenSide,
    }

    public readonly struct CastlingState : IEquatable<CastlingState>
    {

        public required HashSet<CastlingMove> AllowedKingCastlingMoves { get; init; }

        public readonly bool Equals(CastlingState other)
        {
            return AllowedKingCastlingMoves.SetEquals(other.AllowedKingCastlingMoves);
        }

        public override readonly int GetHashCode()
        {
            int hash = 19;
            foreach (var move in AllowedKingCastlingMoves)
            {
                hash = HashCode.Combine(hash, move);
            }
            return hash;
        }
    }
}
