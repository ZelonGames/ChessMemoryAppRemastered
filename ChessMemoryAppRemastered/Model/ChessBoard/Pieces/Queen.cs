using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.ChessBoard.Pieces
{
    public record Queen : Piece
    {
        public static Dictionary<Coordinate, Move> GetLegalMoves(ChessBoardState chessBoardState, Piece queen)
        {
            var rookMoves = Rook.GetLegalMoves(chessBoardState, queen);
            var bishopMoves = Bishop.GetLegalMoves(chessBoardState, queen);

            return rookMoves.Concat(bishopMoves).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public override Dictionary<Coordinate, Move> GetLegalMoves(ChessBoardState chessBoardState)
        {
            return GetLegalMoves(chessBoardState, this);
        }
    }
}
