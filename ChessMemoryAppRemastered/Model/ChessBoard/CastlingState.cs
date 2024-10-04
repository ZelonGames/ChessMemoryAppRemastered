using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.ChessBoard
{
    public record CastlingState
    {
        public enum CastlingMove
        {
            WhiteKingSide,
            WhiteQueenSide,
            BlackKingSide,
            BlackQueenSide,
        }

        public readonly ImmutableHashSet<CastlingMove> allowedKingCastlingMoves;

        public CastlingState(ImmutableHashSet<CastlingMove> allowedKingCastlingMoves)
        {
            this.allowedKingCastlingMoves = allowedKingCastlingMoves;
        }
    }
}
