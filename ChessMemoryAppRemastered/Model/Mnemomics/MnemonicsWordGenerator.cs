using ChessMemoryAppRemastered.Model.ChessBoard;
using ChessMemoryAppRemastered.Model.ChessBoard.Game;
using ChessMemoryAppRemastered.Model.ChessBoard.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.Mnemomics
{
    public class MnemonicsWordGenerator
    {
        private readonly List<string> words = [];
        public List<string> Words => words;

        private string? previousWord = null;
        private MnemonicsNotation? mnemonicsNotation;

        public MnemonicsWordGenerator()
        {
            Init();
        }

        private async void Init()
        {
            mnemonicsNotation = await MnemonicsNotation.CreateInstanceFromJson();
        }

        public void RemoveLastWord()
        {
            words.RemoveAt(words.Count - 1);
            previousWord = words.Count > 0 ? words.Last() : null;
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
            string mnemonicsSquare = move.ToCoordinate.ToAlphabeticCoordinate();
            List<string> candidateWords = mnemonicsNotation!.GetWordsFromSquare(mnemonicsSquare, amountOfCandidatePieces);
            string word = candidateWords[0];
            
            if (previousWord != null && word == previousWord)
            {
                int indexOfWord = candidateWords.IndexOf(word);
                if (indexOfWord + 1 < candidateWords.Count)
                    word = candidateWords[indexOfWord + 1];
            }

            words!.Add(word);
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
}
