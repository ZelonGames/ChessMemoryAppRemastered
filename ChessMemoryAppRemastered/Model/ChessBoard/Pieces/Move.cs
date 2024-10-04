using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.ChessBoard.Pieces
{
    public record Move
    {
        public enum MoveType
        {
            Movement,
            DoublePawnMove,
            Capture,
            EnPassant,
            WhiteKingSideCastle,
            WhiteQueenSideCastle,
            BlackKingSideCastle,
            BlackQueenSideCastle,
            PromotionRook,
            PromotionKnight,
            PromotionBishop,
            PromotionQueen,
        }

        public MoveType moveType;
        public Coordinate coordinate;

        public Move(MoveType type, Coordinate coordinate)
        {
            this.moveType = type;
            this.coordinate = coordinate;
        }
    }
}
