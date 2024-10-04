using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.ChessBoard
{
    public record FiftyMoveRuleState
    {
        public int counter;

        public FiftyMoveRuleState()
        {

        }

        /// <summary>
        /// Resets after a piece that's not a pawn has moved or after a capture
        /// </summary>
        public void ResetCounter()
        {
            counter = 0;
        }

        public void IncrementCounter()
        {
            counter++;
        }
    }
}
