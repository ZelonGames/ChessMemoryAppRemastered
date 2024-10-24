using ChessMemoryAppRemastered.Model.ChessBoard;
using ChessMemoryAppRemastered.Model.ChessBoard.Game;
using ChessMemoryAppRemastered.Model.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.Mnemomics
{
    public class CorrectMoveHelper
    {
        public static bool IsMoveCorrect(LegalMove playerMove, Variation variation, int currentMoveIndex)
        {
            CourseMove currentMove = variation.Moves[currentMoveIndex];
            try
            {
                LegalMove correctMove = MoveNotationHelper.TryGetLegalMoveFromNotation(playerMove.ChessBoardStateBeforeMoveMade, currentMove.MoveNotation);
                if (playerMove != correctMove)
                    return false;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
