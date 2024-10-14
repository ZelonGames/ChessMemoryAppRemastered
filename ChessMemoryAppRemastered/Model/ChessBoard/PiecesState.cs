using ChessMemoryAppRemastered.Model.ChessBoard.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.ChessBoard
{
    public readonly struct PiecesState : IEquatable<PiecesState>
    {
        public required Dictionary<Coordinate, Piece> Pieces { get; init; }

        public readonly bool Equals(PiecesState other)
        {
            if (Pieces == null !^ other.Pieces == null)
                return false;

            if (Pieces.Count != other.Pieces.Count)
                return false;

            foreach (var kvp in Pieces)
            {
                if (!other.Pieces.TryGetValue(kvp.Key, out var otherPiece) || !kvp.Value.Equals(otherPiece))
                    return false;
            }

            return true;
        }

        public override readonly int GetHashCode()
        {
            int hash = 19;
            if (Pieces != null)
            {
                foreach (var kvp in Pieces)
                    hash = HashCode.Combine(hash, kvp.Key, kvp.Value);
            }
            return hash;
        }
    }
}
