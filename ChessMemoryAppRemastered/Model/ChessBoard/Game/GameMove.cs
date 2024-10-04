using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.ChessBoard.Game
{
    public record struct GameMove
    {
        public Coordinate fromCoordinate;
        public Coordinate toCoordinate;

        public GameMove(Coordinate fromCoordinate, Coordinate toCoordinate)
        {
            this.fromCoordinate = fromCoordinate;
            this.toCoordinate = toCoordinate;
        }
    }
}
