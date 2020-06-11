using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace InteractiveGraphUserControl.Converters
{
    class CellBackgroundMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var MoveCtrl = values[0] as string;
                var LimMode = values[1] as string;
                if (LimMode == "Not Active")
                    return Brushes.Gray;
                else if (MoveCtrl == "Position")
                    return Brushes.DodgerBlue;
                else if (MoveCtrl == "Load")
                    return Brushes.IndianRed;
                else
                    return Brushes.White;
            }

            catch (Exception e)
            {
                Console.WriteLine("Erreur dans CellBackgroundConverter" + e);
                return Brushes.White;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
