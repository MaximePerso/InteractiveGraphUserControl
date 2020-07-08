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
    class CellBackgroundMultiConverterDestination : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var MoveCtrl = values[0] as string;
                var DestCtrl = values[1] as string;
                if (MoveCtrl == "Halt")
                {
                    return Brushes.Green;
                }
                else
                {
                    switch (DestCtrl)
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
            }

            catch (Exception e)
            {
                return Brushes.White;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
