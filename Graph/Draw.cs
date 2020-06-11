using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace InteractiveGraphUserControl.Graph
{
    class Draw
    {
        public static void circle(double x, double y, int width, int height, Canvas cv)
        {

            Ellipse circle = new Ellipse()
            {
                Name = "myCricle",
                Tag = "pouet",
                Width = width,
                Height = height,
                Stroke = Brushes.Red,
                StrokeThickness = 6
            };

            cv.Children.Add(circle);

            circle.SetValue(Canvas.LeftProperty, x-width/2);
            circle.SetValue(Canvas.TopProperty, y - height / 2);
        }
    }
}
