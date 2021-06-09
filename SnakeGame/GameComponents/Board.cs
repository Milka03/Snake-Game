using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SnakeGame
{
    public class Board
    {
        private readonly Tile[,] tiles;
        public int Rows { get; }
        public int Columns { get; }

        public Board(int boardSize, Canvas board)
        {
            Tile.myCanvas = board ?? throw new ArgumentNullException(nameof(board));
            Tile.Size = (int)Math.Floor(board.Height / boardSize);
            Rows = Columns = boardSize;

            tiles = new Tile[Rows, Columns];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    tiles[i, j] = new Tile(i, j);
                }
            }
        }

        public IEnumerable<Tile> GetTiles()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    yield return tiles[i, j];
                }
            }
        }

        public void ClearCanvas() => Tile.myCanvas.Children.Clear();

        // Indexer
        public Tile this[int i, int j]
        {
            get { return tiles[i, j]; }
        }

    }
}
