using ChessMemoryAppRemastered.Model.ChessBoard.FEN;
using ChessMemoryAppRemastered.Model.ChessBoard.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.UI_Components
{
    public class UIPiece
    {
        private static readonly Dictionary<char, string> pieceFileNames = new()
        {
            { 'p', "pawn_black.png" },
            { 'P', "pawn_white.png" },
            { 'r', "rook_black.png" },
            { 'R', "rook_white.png" },
            { 'n', "knight_black.png" },
            { 'N', "knight_white.png" },
            { 'b', "bishop_black.png" },
            { 'B', "bishop_white.png" },
            { 'q', "queen_black.png" },
            { 'Q', "queen_white.png" },
            { 'k', "king_black.png" },
            { 'K', "king_white.png" },
        };

        public readonly Image image;
        public readonly Piece piece;

        public UIPiece(Piece piece)
        {
            this.piece = piece;
            char pieceChar = FenHelper.GetFenPieceChar(piece);

            image = new Image()
            {
                Source = ImageSource.FromFile(pieceFileNames[pieceChar]),
            };
        }
    }
}
