using ChessMemoryAppRemastered.Model.ChessBoard;
using ChessMemoryAppRemastered.Model.ChessBoard.FEN;
using ChessMemoryAppRemastered.Model.ChessBoard.Game;
using ChessMemoryAppRemastered.Model.ChessBoard.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model
{
    public class MoveTree<T>(T item)
    {
        public int Count { get; private set; }
        private T? lastItem;
        private readonly T item = item;
        private MoveTree<T>? parent = null;
        private readonly List<MoveTree<T>> children = [];

        public void AddFrom(MoveTree<T> from, MoveTree<T> next)
        {
            from.children.Add(next);
            next.parent = from;
            Count++;
        }

        public T GetFromDepth(int depth)
        {
            depth = Math.Clamp(depth, 1, Count + 1);
            return Iterate().Take(depth).Last();
        }

        private IEnumerable<T> Iterate()
        {
            lastItem = item;
            yield return item;

            foreach (var child in children)
            {
                foreach (var descendant in child.Iterate())
                {
                    lastItem = descendant;
                    yield return descendant;
                }
            }
        }
    }
}
