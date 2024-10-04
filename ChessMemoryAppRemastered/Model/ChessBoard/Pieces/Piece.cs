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
        public required PlayerColor color;
        public abstract Dictionary<Coordinate, Move> GetLegalMoves(ChessBoardState chessBoardState);

        public PlayerColor GetEnemyColor()
        {
            if (color == PlayerColor.White)
                return PlayerColor.Black;

            return PlayerColor.White;
        }
    }
}
