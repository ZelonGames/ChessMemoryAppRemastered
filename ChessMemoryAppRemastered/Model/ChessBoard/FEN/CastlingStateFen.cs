using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.ChessBoard.FEN
{
    public record CastlingStateFen
    {
        public readonly CastlingState castlingState;

        public CastlingStateFen(string fen)
        {
            string fenCastlingState = fen.Split(' ')[2];

            var allowedKingCastlingMoves = new HashSet<CastlingState.CastlingMove>();
            foreach (var c in fenCastlingState)
            {
                if (char.IsLetter(c))
                    allowedKingCastlingMoves.Add(GetStateFromFenChar(c));
            }

            castlingState = new CastlingState(allowedKingCastlingMoves.ToImmutableHashSet());
        }

        private static CastlingState.CastlingMove GetStateFromFenChar(char c)
        {
            return c switch
            {
                'K' => CastlingState.CastlingMove.WhiteKingSide,
                'Q' => CastlingState.CastlingMove.WhiteQueenSide,
                'k' => CastlingState.CastlingMove.BlackKingSide,
                'q' => CastlingState.CastlingMove.BlackQueenSide,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
