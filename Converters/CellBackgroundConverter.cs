using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace InteractiveGraphUserControl.Converters
{
    class CellBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                string input = value as string;
                switch (input)
                {
                    case "Load":
                        return Brushes.IndianRed;
                    case "Position":
                        return Brushes.DodgerBlue;
                    case "Not Active":
                        return Brushes.Gray;
                    default:
                        return Brushes.White;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Erreur dans CellBackgroundConverter" + e);
                return Brushes.White;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
