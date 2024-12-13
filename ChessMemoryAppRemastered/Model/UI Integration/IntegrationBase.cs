using ChessMemoryAppRemastered.Model.UI_Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.UI_Integration;

public abstract class IntegrationBase(UIChessBoard uIChessBoard)
{
    protected readonly UIChessBoard uIChessBoard = uIChessBoard;
    protected UISquare.HighlightType highlightType;

    public abstract void Dispose();
}
