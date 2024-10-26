using ChessMemoryAppRemastered.Model.ChessBoard;
using ChessMemoryAppRemastered.Model.ChessBoard.FEN;
using ChessMemoryAppRemastered.Model.ChessBoard.Game;
using ChessMemoryAppRemastered.Model.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model;

public class VariationNavigator(Course course, Variation variation)
{
    public LegalMove CurrentMove { get; private set; }
    private readonly Variation? variation = variation;
    private readonly int resetIndex = variation!.Moves.FindIndex(x => x.Fen == course.PreviewFen);

    public ChessBoardState GetNextState(ChessBoardState currentState)
    {
        string currentStateFen = FenHelper.ConvertToFenString(currentState);
        int currentMoveIndex = variation!.Moves.FindIndex(x => x.Fen == currentStateFen);
        if (currentMoveIndex == -1 || currentMoveIndex + 1 >= variation.Moves.Count)
            return currentState;

        CourseMove nextMove = variation.Moves[currentMoveIndex + 1];
        LegalMove legalMove = MoveNotationHelper.TryGetLegalMoveFromNotation(currentState, nextMove.MoveNotation);
        CurrentMove = legalMove;
        return MoveHelper.GetNextStateFromMove(legalMove);
    }

    public ChessBoardState GetPreviousState(ChessBoardState currentState)
    {
        string currentStateFen = FenHelper.ConvertToFenString(currentState);
        int currentMoveIndex = variation!.Moves.FindIndex(x => x.Fen == currentStateFen);
        if (currentMoveIndex == -1 || currentMoveIndex - 1 < resetIndex)
            return currentState;

        CourseMove previousMove = variation.Moves[currentMoveIndex - 1];
        ChessBoardState previousState = ChessBoardFenGenerator.Generate(previousMove.Fen);
        return previousState;
    }

    public ChessBoardState GetStartState()
    {
        CourseMove resetMove = variation!.Moves[resetIndex];
        ChessBoardState currentState = ChessBoardFenGenerator.Generate(resetMove.Fen);
        return currentState;
    }
}