using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;

namespace SnakeGame
{
    public class Tile
    {
        private readonly Shape myShape;
        public static int Size { get; set; }
        public static Canvas myCanvas { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public Brush GetFillColor => myShape.Fill;

        public Tile(int i, int j)
        {
            X = i * Size;
            Y = j * Size;
            RowIndex = i;
            ColumnIndex = j;

            myShape = new Rectangle();
            if ((i + j) % 2 == 0)
                myShape.Fill = Brushes.LightYellow;
            else
                myShape.Fill = Brushes.Wheat;

            myShape.Width = myShape.Height = Size;
            myCanvas.Children.Add(myShape);
            Canvas.SetLeft(myShape, X);
            Canvas.SetTop(myShape, Y);
        }

        public void Fill(SolidColorBrush brush) => myShape.Fill = brush;
        public void Unfill() => myShape.Fill = null;

    }
}
