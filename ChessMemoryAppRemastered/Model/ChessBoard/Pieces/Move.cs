using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.ChessBoard.Pieces
{
    public record struct Move
    {
        public enum Type
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

        public enum Promotion
        {
            Rook,
            Knight,
            Bishop,
            Queen,
            None,
        }

        public Type type { get; init; }
        public Coordinate coordinate { get; init; }
        public Promotion promotion { get; init; }

        public Move(Type type, Coordinate coordinate, Promotion promotion = Promotion.None)
        {
            this.type = type;
            this.coordinate = coordinate;
            this.promotion = promotion;
        }
    }
}
