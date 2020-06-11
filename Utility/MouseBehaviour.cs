using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InteractiveGraphUserControl.Utility
{
    public class MouseBehaviour : System.Windows.Interactivity.Behavior<FrameworkElement>
    {
        #region MouseMouve
        public static readonly DependencyProperty MouseYProperty = DependencyProperty.Register(
            "MouseY", typeof(double), typeof(MouseBehaviour), new PropertyMetadata(default(double)));

        public double MouseY
        {
            get => (double)GetValue(MouseYProperty);
            set => SetValue(MouseYProperty, value);
        }

        public static readonly DependencyProperty MouseXProperty = DependencyProperty.Register(
            "MouseX", typeof(double), typeof(MouseBehaviour), new PropertyMetadata(default(double)));

        public double MouseX
        {
            get => (double)GetValue(MouseXProperty);
            set => SetValue(MouseXProperty, value);
        }

        protected override void OnAttached()
        {
            AssociatedObject.MouseMove += AssociatedObjectOnMouseMove;
        }

        private void AssociatedObjectOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            var pos = mouseEventArgs.GetPosition(AssociatedObject);
            MouseX = pos.X;
            MouseY = pos.Y;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseMove -= AssociatedObjectOnMouseMove;
        }
        #endregion 

        //#region MouseDown

        //public static readonly DependencyProperty MouseDownCommandProperty =
        //    DependencyProperty.RegisterAttached("MouseDownCommand", typeof(ICommand), typeof(MouseBehaviour), new FrameworkPropertyMetadata(new PropertyChangedCallback(MouseDownCommandChanged)));

        //private static void MouseDownCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    FrameworkElement element = (FrameworkElement)d;

        //    element.MouseDown += element_MouseDown;
        //}

        //static void element_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    FrameworkElement element = (FrameworkElement)sender;

        //    ICommand command = GetMouseDownCommand(element);

        //    command.Execute(e);
        //}

        //public static void SetMouseDownCommand(UIElement element, ICommand value)
        //{
        //    element.SetValue(MouseDownCommandProperty, value);
        //}

        //public static ICommand GetMouseDownCommand(UIElement element)
        //{
        //    return (ICommand)element.GetValue(MouseDownCommandProperty);
        //}

        ////#endregion
    }
}
