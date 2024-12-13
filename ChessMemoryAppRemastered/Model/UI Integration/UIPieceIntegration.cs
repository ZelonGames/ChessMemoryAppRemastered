using JChessLib;
using JChessLib.Pieces;
using ChessMemoryAppRemastered.Model.UI_Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.UI_Integration;

public class UIPieceIntegration : IntegrationBase
{
    private UIPiece? clickedPiece = null;
    private readonly PointerGestureRecognizer pointerRecognizer;
    public UIPieceIntegration(UIChessBoard uIChessBoard)
        : base(uIChessBoard)
    {
        highlightType = UISquare.HighlightType.Green;
        pointerRecognizer = new PointerGestureRecognizer();

        pointerRecognizer.PointerEntered += Piece_PointerEntered;
        pointerRecognizer.PointerExited += Piece_PointerExited;
        pointerRecognizer.PointerReleased += Piece_PointerReleased;

        foreach (var square in this.uIChessBoard.squares.Values)
            square.contentView.GestureRecognizers.Add(pointerRecognizer);
    }

    public override void Dispose()
    {
        pointerRecognizer.PointerEntered -= Piece_PointerEntered;
        pointerRecognizer.PointerExited -= Piece_PointerExited;
        pointerRecognizer.PointerReleased -= Piece_PointerEntered;

        foreach (var piece in uIChessBoard.pieces.Values)
            piece.image.GestureRecognizers.Remove(pointerRecognizer);
    }

    private void Piece_PointerEntered(object? sender, PointerEventArgs e)
    {
        if (clickedPiece is null)
        {
            ResetHighlights();
            HighlightSquares(sender!);
        }
    }

    private void Piece_PointerExited(object? sender, PointerEventArgs e)
    {
        if (clickedPiece is not null)
            return;

        ResetHighlights();
    }

    private void Piece_PointerReleased(object? sender, PointerEventArgs e)
    {
        var contentView = sender as ContentView;
        UIPiece uIPiece = uIChessBoard.pieces
            .FirstOrDefault(x => x.Value.image == (contentView!.Content as Image)).Value;

        if (uIPiece is null)
        {
            if (clickedPiece is not null)
            {
                ResetHighlights();
                clickedPiece = null;
            }
            return;
        }

        if (clickedPiece is not null && clickedPiece != uIPiece)
            ResetHighlights();

        clickedPiece = uIPiece;

        HighlightSquares(sender!);
    }

    private void ResetHighlights()
    {
        var highlightedSquares = uIChessBoard.squares
            .Where(x => x.Value._HighlightType == highlightType);

        foreach (var square in highlightedSquares)
            square.Value.SetHighlight(UISquare.HighlightType.None);
    }

    private void HighlightSquares(object sender)
    {
        var contentView = sender as ContentView;
        UIPiece uIPiece = uIChessBoard.pieces
            .FirstOrDefault(x => x.Value.image == (contentView!.Content as Image)).Value;

        if (uIPiece is null)
            return;

        Dictionary<Coordinate, Move> legalMoves =
            uIPiece!.piece.GetLegalMoves(uIChessBoard.chessBoardState);

        foreach (var move in legalMoves.Values)
            uIChessBoard.squares[move.coordinate].SetHighlight(highlightType);
    }
}
