using ChessMemoryAppRemastered.Model.ChessBoard.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.ChessBoard
{
    public record TerritoryState
    {
        private readonly ChessBoardState chessBoardState;
        public readonly Dictionary<Coordinate, int> controlledSquares;

        public TerritoryState(ChessBoardState chessBoardState, ChessBoardState.PlayerColor color)
        {
            this.chessBoardState = chessBoardState;
            controlledSquares = GetControlledSquaresFromColor(color);
        }

        private Dictionary<Coordinate, int> GetControlledSquaresFromColor(ChessBoardState.PlayerColor color)
        {
            var controlledSquares = new Dictionary<Coordinate, int>();
            var piecesOfColor = chessBoardState.Pieces.Where(x => x.Value.color == color && x.Value.GetType() != typeof(King));

            foreach (var piece in piecesOfColor)
            {
                foreach (var controlledSquare in piece.Value.GetLegalMoves(chessBoardState))
                {
                    if (controlledSquare.Value.moveType is not Move.MoveType.Movement or Move.MoveType.Capture)
                        continue;

                    if (controlledSquares.ContainsKey(controlledSquare.Value.coordinate))
                        controlledSquares[controlledSquare.Value.coordinate]++;
                    else
                        controlledSquares.Add(controlledSquare.Value.coordinate, 1);
                }
            }

            return controlledSquares;
        }
    }
}
