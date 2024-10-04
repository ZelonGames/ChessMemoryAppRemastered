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

        public TerritoryState(ChessBoardState chessBoardState, PlayerColor color)
        {
            this.chessBoardState = chessBoardState;
            controlledSquares = GetControlledSquaresFromColor(color);
        }

        private Dictionary<Coordinate, int> GetControlledSquaresFromColor(PlayerColor color)
        {
            var controlledSquares = new Dictionary<Coordinate, int>();
            var piecesOfColor = chessBoardState.Pieces.Values.Where(x => x.color == color && x is not King);

            foreach (var piece in piecesOfColor)
            {
                foreach (var controlledSquare in piece.GetLegalMoves(chessBoardState).Values)
                {
                    if (controlledSquare.moveType is not Move.MoveType.Movement or Move.MoveType.Capture)
                        continue;

                    if (controlledSquares.ContainsKey(controlledSquare.coordinate))
                        controlledSquares[controlledSquare.coordinate]++;
                    else
                        controlledSquares.Add(controlledSquare.coordinate, 1);
                }
            }

            return controlledSquares;
        }
    }
}
