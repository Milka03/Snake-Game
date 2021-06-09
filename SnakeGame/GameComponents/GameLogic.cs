using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows.Controls;

using SnakeGame.ViewModel;

namespace SnakeGame.GameComponents
{
    public static class GameLogic
    {
        private static Snake snake;
        private static Board board;
        public static DispatcherTimer timer { get; set; }
        public static ViewModelClass viewModel { get; set; }
        public static Canvas Canvas { get; set; }
        public static Random Rand { get; set; }

        private static string[] links = new string[]
        {
            "https://cdn.pixabay.com/photo/2016/03/31/19/29/animals-1295060_960_720.png",
            "https://cdn.pixabay.com/photo/2013/07/13/12/05/rattlesnake-159135_960_720.png",
            "https://cdn.pixabay.com/photo/2016/03/28/22/08/cobra-1287036_960_720.png",
            "https://cdn.pixabay.com/photo/2019/02/06/17/09/snake-3979601_960_720.jpg",
            "https://cdn.pixabay.com/photo/2015/09/16/13/42/green-tree-python-942686_960_720.jpg",
            "https://cdn.pixabay.com/photo/2015/02/28/15/25/snake-653639_960_720.jpg"
        };


        public static void InitializeGame(GameSpeedEnum speed, BoardSizeEnum boardSize)
        {
            timer.Stop();
            timer.Interval = new TimeSpan(0, 0, 0, 0, (int)speed);
            if (board != null) 
                board.ClearCanvas();
            board = new Board((int)boardSize, Canvas);
            snake = new Snake(Brushes.YellowGreen, Brushes.Green, 3);

            int X = (board.Rows / 2) - 1;
            int Y = (int)Math.Floor((double)board.Columns / 3);
            snake.InitializeSnake(board, Y, X);
            Apple.Row = X;
            Apple.Col = 3 * board.Columns / 4;
            Apple.Diameter = Tile.Size;
            Apple.DrawApple(Canvas);
            viewModel.Score = 0;
            viewModel.NewGame = true;
        }

        public static void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    if (snake.MovingDirection != Direction.Down)
                        snake.MovingDirection = Direction.Up;
                    break;
                case Key.Down:
                    if (snake.MovingDirection != Direction.Up)
                        snake.MovingDirection = Direction.Down;
                    break;
                case Key.Left:
                    if (snake.MovingDirection != Direction.Right)
                        snake.MovingDirection = Direction.Left;
                    break;
                case Key.Right:
                    if (viewModel.NewGame) 
                    { 
                        timer.Start(); 
                        viewModel.NewGame = false;
                        snake.MovingDirection = Direction.Right;
                    }
                    else if (snake.MovingDirection != Direction.Left)
                        snake.MovingDirection = Direction.Right;
                    break;
                case Key.Escape:
                    Application.Current.Shutdown();
                    break;
            }
        }

        public static void MoveSnake(object sender, EventArgs e)
        {
            int xDir = snake.Head.RowIndex;
            int yDir = snake.Head.ColumnIndex;

            switch (snake.MovingDirection)
            {
                case Direction.Up:
                    yDir -= 1;
                    break;
                case Direction.Down:
                    yDir += 1;
                    break;
                case Direction.Left:
                    xDir -= 1;
                    break;
                case Direction.Right:
                    xDir += 1;
                    break;
            }

            if (SnakeCrushed(xDir, yDir))
            {
                timer.Stop();
                GameOver();
                return;
            }
            snake.Head.Fill(snake.BodyColor);
            snake.Body.Insert(0, snake.Head);
            snake.Head = board[xDir, yDir];

            if (yDir == Apple.Row && xDir == Apple.Col)
            {
                viewModel.Score++;
                MakeNewApple();
                snake.Length++;
            }
            else snake.Body.RemoveAt(snake.Length);

            if ((snake.Body.Last().RowIndex + snake.Body.Last().ColumnIndex) % 2 == 0)
                snake.Body.Last().Fill(Brushes.LightYellow); 
            else
                snake.Body.Last().Fill(Brushes.Wheat);
            snake.DrawHeadTail();
        }

        private static bool SnakeCrushed(int i, int j)
        {
            if (i < 0 || i > board.Rows - 1 || j < 0 || j > board.Columns - 1)
                return true;
            foreach (var part in snake.Body)
            {
                if (part.RowIndex == i && part.ColumnIndex == j)
                    return true;
            }
            return false;
        }

        private static void MakeNewApple()
        {
            int idx = Rand.Next(links.Length);
            viewModel.SnakeBitmap = new BitmapImage(new Uri(links[idx]));

            int newRow = Rand.Next(0, board.Rows);
            int newCol = Rand.Next(0, board.Columns);
            while (snake.Body.Any(i => i.RowIndex == newCol && i.ColumnIndex == newRow) || 
                (newRow == snake.Head.ColumnIndex && newCol == snake.Head.RowIndex))
            {
                newRow = Rand.Next(0, board.Rows);
                newCol = Rand.Next(0, board.Columns);
            }
            Apple.Row = newRow;
            Apple.Col = newCol;
            Apple.DrawApple(Canvas);
        }

        private static void GameOver()
        {
            string message = "You scored " + viewModel.Score + " points. You can now change the settings or press right arrow to start playing new game.";
            MessageBox.Show(message, "Game Over!");

            if (viewModel.Score > viewModel.BestScore)
                viewModel.BestScore = viewModel.Score;
            InitializeGame(viewModel.CurrentSpeed.GameSpeed, viewModel.CurrentBoard.BoardSize);
        }

    }
}
