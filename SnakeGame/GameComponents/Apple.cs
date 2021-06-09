using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace SnakeGame
{
    public static class Apple
    {
        private static SolidColorBrush color = Brushes.Red;
        private static Shape shape = null;
        public static int Row { get; set; }
        public static int Col { get; set; }
        public static int Diameter { get; set; }

        public static void DrawApple(Canvas canvas)
        {
            if (shape != null) canvas.Children.Remove(shape);
            shape = new Ellipse();
            shape.Width = shape.Height = Diameter;
            shape.Fill = color;
            canvas.Children.Add(shape);
            Canvas.SetLeft(shape, Col * Diameter);
            Canvas.SetTop(shape, Row * Diameter);
        }
    }
}
