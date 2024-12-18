﻿using JChessLib;
using JChessLib.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.Mnemomics;

public class MnemonicsWordGenerator
{
    private readonly Stack<string> words = [];
    public string LastWord => words.Count > 0 ? words.Peek() : "";

    private string? previousWord = null;
    private MnemonicsNotation? mnemonicsNotation;

    public MnemonicsWordGenerator()
    {
        InitAsync();
    }

    private async void InitAsync()
    {
        mnemonicsNotation = await MnemonicsNotation.CreateInstanceFromJson();
    }

    public void TryRemoveLastWord()
    {
        if (words.Count == 0)
            return;
        words.Pop();
        previousWord = words.Count > 0 ? words.Peek() : null;
    }

    public async Task AddWordFromMove(LegalMove move)
    {
        Piece movingPiece = move.GetPieceToMove();
        ChessBoardState chessBoard = move.ChessBoardStateBeforeMoveMade;
        var pieces = chessBoard.PiecesState.Pieces
            .OrderBy(x => x.Value.coordinate.Y)
            .OrderBy(x => x.Value.coordinate.X)
            .Where(x => x.Value.color == movingPiece.color);

        int amountOfCandidatePieces = 0;

        foreach (var piece in pieces)
        {
            bool canPieceMakeTheSameMove = piece.Value.GetLegalMoves(chessBoard).ContainsKey(move.ToCoordinate);
            if (canPieceMakeTheSameMove)
                amountOfCandidatePieces++;
            if (piece.Value == move.GetPieceToMove())
                break;
        }

        var mnemonicsNotation = await MnemonicsNotation.CreateInstanceFromJson();
        string mnemonicsSquare = move.ToCoordinate.ConvertToAlphabeticCoordinate();
        List<string> candidateWords = mnemonicsNotation!.GetWordsFromSquare(mnemonicsSquare, amountOfCandidatePieces);
        string word = candidateWords[0];

        if (previousWord != null && word == previousWord)
        {
            int indexOfWord = candidateWords.IndexOf(word);
            if (indexOfWord + 1 < candidateWords.Count)
                word = candidateWords[indexOfWord + 1];
        }

        words!.Push(word);
        previousWord = word;
    }

    public string GetWordsAsString()
    {
        string wordString = "";
        foreach (var word in words)
        {
            wordString += word + "\n";
        }
        return wordString;
    }
}
