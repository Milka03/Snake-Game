using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;


namespace SnakeGame.ViewModel
{
    public class BoardSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string tmp = "";
            if (!(value is BoardSizeEnum)) { return null; }
            tmp += (int)(value);// as BoardSizeClass).BoardSize;
            tmp += " x ";
            tmp += (int)(value);// as BoardSizeClass).BoardSize;
            return tmp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
