using Microsoft.Maui.Controls.PlatformConfiguration;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.ChessBoard.Pieces
{
    public record Bishop : Piece
    {
        public static Dictionary<Coordinate, Move> GetLegalMoves(ChessBoardState chessBoardState, Piece bishop)
        {
            var moves = new Dictionary<Coordinate, Move>();

            // Right Up
            for (int x = 1; bishop.coordinate.x + x < 8 && bishop.coordinate.y + x < 8; x++)
            {
                var coordinate = new Coordinate(bishop.coordinate.x + x, bishop.coordinate.y + x);
                if (chessBoardState.Pieces.TryGetValue(coordinate, out Piece? piece))
                {
                    if (piece.color != bishop.color)
                        moves.Add(coordinate, new Move(Move.MoveType.Capture, coordinate));

                    break;
                }
                else
                    moves.Add(coordinate, new Move(Move.MoveType.Movement, coordinate));
            }

            // Right Down
            for (int x = 1; bishop.coordinate.x + x < 8 && bishop.coordinate.y - x > 0; x++)
            {
                var coordinate = new Coordinate(bishop.coordinate.x + x, bishop.coordinate.y - x);
                if (chessBoardState.Pieces.TryGetValue(coordinate, out Piece? piece))
                {
                    if (piece.color != bishop.color)
                        moves.Add(coordinate, new Move(Move.MoveType.Capture, coordinate));

                    break;
                }
                else
                    moves.Add(coordinate, new Move(Move.MoveType.Movement, coordinate));
            }

            // Left Down
            for (int x = -1; bishop.coordinate.x + x > 0 && bishop.coordinate.y + x > 0; x--)
            {
                var coordinate = new Coordinate(bishop.coordinate.x + x, bishop.coordinate.y + x);
                if (chessBoardState.Pieces.TryGetValue(coordinate, out Piece? piece))
                {
                    if (piece.color != bishop.color)
                        moves.Add(coordinate, new Move(Move.MoveType.Capture, coordinate));

                    break;
                }
                else
                    moves.Add(coordinate, new Move(Move.MoveType.Movement, coordinate));
            }

            // Left Up
            for (int x = -1; bishop.coordinate.x + x > 0 && bishop.coordinate.y - x < 8; x--)
            {
                var coordinate = new Coordinate(bishop.coordinate.x + x, bishop.coordinate.y - x);
                if (chessBoardState.Pieces.TryGetValue(coordinate, out Piece? piece))
                {
                    if (piece.color != bishop.color)
                        moves.Add(coordinate, new Move(Move.MoveType.Capture, coordinate));
                    break;
                }
                else
                    moves.Add(coordinate, new Move(Move.MoveType.Movement, coordinate));
            }

            return moves;
        }
        public override Dictionary<Coordinate, Move> GetLegalMoves(ChessBoardState chessBoardState)
        {
            return GetLegalMoves(chessBoardState, this);
        }
    }
}
