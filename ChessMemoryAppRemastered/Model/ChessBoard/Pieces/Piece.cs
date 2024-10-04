using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.ChessBoard.Pieces
{
    public abstract record Piece
    {
        public required Coordinate coordinate;
        public required ChessBoardState.PlayerColor color;
        public abstract Dictionary<Coordinate, Move> GetLegalMoves(ChessBoardState chessBoardState);

        public ChessBoardState.PlayerColor GetEnemyColor()
        {
            if (color == ChessBoardState.PlayerColor.White)
                return ChessBoardState.PlayerColor.Black;

            return ChessBoardState.PlayerColor.White;
        }
    }
}
