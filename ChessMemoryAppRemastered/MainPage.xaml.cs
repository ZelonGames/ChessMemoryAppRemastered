using ChessMemoryAppRemastered.Model.ChessBoard.FEN;
using ChessMemoryAppRemastered.Model.ChessBoard.Game;
using ChessMemoryAppRemastered.Model;
using ChessMemoryAppRemastered.Model.ChessBoard;
using System.Security.Cryptography.X509Certificates;

namespace ChessMemoryAppRemastered
{
    public partial class MainPage : ContentPage
    {
        record TestRecord(string A);
        int count = 0;

        public MainPage()
        {
            InitializeComponent();

            var moves = new List<GameMove>
            {
                new(new(3, 6), new(3, 4)),
            };

            var fenHistory = new List<string>();
            ChessBoardState chessBoardState = ChessBoardFenGenerator.Generate("r2qkb1r/1bpp1ppp/2n2n2/pp2P3/4P3/1B3N2/PPP2PPP/RNBQK1R1 b Qkq - 0 8");
            string fen = chessBoardState.ConvertToFenString();
            fenHistory.Add(fen);
            foreach (var potentialMove in moves)
            {
                try
                {
                    var move = new LegalMove(chessBoardState, potentialMove.fromCoordinate, potentialMove.toCoordinate);
                    chessBoardState = MoveHelper.GetNextStateFromMove(move);
                    fen = chessBoardState.ConvertToFenString();
                    fenHistory.Add(fen);
                }
                catch (MoveException ex)
                {
                    throw;
                }
            }
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;
            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }

}

