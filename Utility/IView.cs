using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InteractiveGraphUserControl.Utility
{
    public interface IView
    {
        Point ToPixel(Point point);
        Point ToChartValue(int axisY);
        bool TopAreaMouseOver();
        bool BottomAreaMouseOver();
        void ChartUpdate();
    }
}
