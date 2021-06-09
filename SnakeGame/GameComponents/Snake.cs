using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;

using SnakeGame.ViewModel;

namespace SnakeGame
{
    public class Snake
    {
        private Path headEllipse;
        private Path tailEllipse;
        public SolidColorBrush BodyColor { get; set; }
        public SolidColorBrush HeadColor { get; set; }
        public Direction MovingDirection { get; set; }
        public int Length { get;  set; }
        internal List<Tile> Body { get; }
        internal Tile Head { get;  set; }

        public Snake(SolidColorBrush bodyColor, SolidColorBrush headColor, int length)
        {
            Length = length;
            BodyColor = bodyColor;
            HeadColor = headColor;
            MovingDirection = Direction.Right;
            Body = new List<Tile>();
            headEllipse = null;
            tailEllipse = null;
        }

        public void InitializeSnake(Board board, int startX, int startY)
        {
            Head = board[startX, startY];
            //Head.Fill(HeadColor);
            for (int i = 1; i <= Length; i++)
            {
                var square = board[startX - i, startY];
                if (i < Length) square.Fill(BodyColor);
                Body.Add(square);
            }
            DrawHeadTail();
        }

        
        public void DrawHeadTail()
        {
            if (headEllipse != null) Tile.myCanvas.Children.Remove(headEllipse);
            if (tailEllipse != null) Tile.myCanvas.Children.Remove(tailEllipse);
            int arcLength = 7 * Tile.Size;

            switch (MovingDirection)
            {
                case Direction.Left:
                    headEllipse = DrawEllipse(arcLength, Tile.Size, Head.X + Tile.Size, Head.Y, 0, Tile.Size, HeadColor, 1);
                    break;
                case Direction.Right:
                    headEllipse = DrawEllipse(arcLength, Tile.Size, Head.X, Head.Y, 0, Tile.Size, HeadColor, 0);
                    break;
                case Direction.Up:
                    headEllipse = DrawEllipse(Tile.Size, arcLength, Head.X, Head.Y + Tile.Size, Tile.Size, 0, HeadColor, 0);
                    break;
                case Direction.Down:
                    headEllipse = DrawEllipse(Tile.Size, arcLength, Head.X, Head.Y, Tile.Size, 0, HeadColor, 1);
                    break;
            }

            if (Body[Length - 2].RowIndex == Body.Last().RowIndex + 1) //Tail on the left
                tailEllipse = DrawEllipse(arcLength, Tile.Size, Body.Last().X + Tile.Size, Body.Last().Y, 0, Tile.Size, BodyColor, 1);

            else if (Body[Length - 2].RowIndex == Body.Last().RowIndex - 1) //Tail on the right
                tailEllipse = DrawEllipse(arcLength, Tile.Size, Body.Last().X, Body.Last().Y, 0, Tile.Size, BodyColor, 0);

            else if (Body[Length - 2].ColumnIndex == Body.Last().ColumnIndex + 1) //Tail pointed upwards
                tailEllipse = DrawEllipse(Tile.Size, arcLength, Body.Last().X, Body.Last().Y + Tile.Size, Tile.Size, 0, BodyColor, 0);

            else if (Body[Length - 2].ColumnIndex == Body.Last().ColumnIndex - 1) //Tail pointed down
                tailEllipse = DrawEllipse(Tile.Size, arcLength, Body.Last().X, Body.Last().Y, Tile.Size, 0, BodyColor, 1);
        }

        private Path DrawEllipse(int width, int height, int posX, int posY, int deltaX, int deltaY, SolidColorBrush color, int turn)
        {
            Path myPath = new Path();
            myPath.Stroke = color;
            myPath.Fill = color;
            StreamGeometry geometry = new StreamGeometry();

            using (StreamGeometryContext ctx = geometry.Open())
            {
                ctx.BeginFigure(new Point(posX, posY), true, true);
                ctx.LineTo(new Point(posX + deltaX, posY + deltaY), false, false);
                ctx.ArcTo(new Point(posX, posY), new Size(width, height + 3), 0, false, (SweepDirection)turn, false, false);
            }
            geometry.Freeze();
            myPath.Data = geometry;
            Tile.myCanvas.Children.Add(myPath);
            return myPath;
        }

    }
}
