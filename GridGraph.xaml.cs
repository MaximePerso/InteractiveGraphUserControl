using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using InteractiveGraphUserControl.MVVM;
using InteractiveGraphUserControl.Utility;
using LiveCharts.Wpf;

namespace InteractiveGraphUserControl
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class GridGraph : UserControl, IView
    {
        private MVVM.ViewModel _viewModel;

        public GridGraph()
        {
            _viewModel = new ViewModel(this);
            this.DataContext = _viewModel;
            InitializeComponent();
        }

        #region IView
        public Point ToPixel(Point point)
        {
            Point asPixel = Chart.ConvertToPixels(point);
            return asPixel;
        }

        public Point ToChartValue(int axisY)
        {
            Point asGraphValue = Chart.ConvertToChartValues(Mouse.GetPosition(Chart), 0, axisY);
            return asGraphValue;
        }

        public bool TopAreaMouseOver()
        {
            bool mouseOver = false;
            if (GraphTopBorder.IsMouseOver)
                mouseOver = true;
            //Console.WriteLine(mouseOver);
            return mouseOver;
        }

        public bool BottomAreaMouseOver()
        {
            bool mouseOver = false;
            if (GraphBottomBorder.IsMouseOver)
                mouseOver = true;
            return mouseOver;
        }

        public void ChartUpdate()
        {
            Chart.Update(true, true);
        }

        #endregion

        #region GRID

        #region Fields

        private int _originalIndex;
        private DataGridRow _oldRow;
        private DoliInput _targetItem;


        #endregion


        #region Events -- Could be translated into command to keep a tigh codebehind

        #region Drag and Drop

        /// <summary>
        /// Updates the grid as a drag progresses
        /// </summary>
        private void OnMainGridCheckDropTarget(object sender, DragEventArgs e)
        {
            var row = VisualHelper.FindParent<DataGridRow>(e.OriginalSource as UIElement);

            /* If we are over a row that contains a DoliInput, set 
             * the drop-line above that row. Otherwise, do nothing. */

            // Set the DragDropEffects 
            if ((row == null) || !(row.Item is DoliInput))
            {
                e.Effects = DragDropEffects.None;
            }
            else
            {
                var currentIndex = row.GetIndex();

                // Erase old drop-line
                if (_oldRow != null) _oldRow.BorderThickness = new Thickness(0);

                // Draw new drop-line
                var direction = (currentIndex - _originalIndex);
                if (direction < 0) row.BorderThickness = new Thickness(0, 2, 0, 0);
                else if (direction > 0) row.BorderThickness = new Thickness(0, 0, 0, 2);

                // Reset old row
                _oldRow = row;
            }
        }

        /// <summary>
        /// Gets the view model from the data Context and assigns it to a member variable.
        /// </summary>
        private void OnMainGridDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _viewModel = (MVVM.ViewModel)this.DataContext?.GetType().GetProperty("gridVM")?.GetValue(this.DataContext, null);
            //_viewModel = (Grid.ViewModel)this.DataContext;
        }

        /// <summary>
        /// Process a row drop on the DataGrid.
        /// </summary>
        private void OnMainGridDrop(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;

            // Verify that this is a valid drop and then store the drop target
            var row = VisualHelper.FindParent<DataGridRow>(e.OriginalSource as UIElement);
            if (row != null)
            {
                _targetItem = row.Item as DoliInput;
                if (_targetItem != null)
                {
                    e.Effects = DragDropEffects.Move;
                }
            }

            // Erase last drop-line
            if (_oldRow != null) _oldRow.BorderThickness = new Thickness(0, 0, 0, 0);
        }

        /// <summary>
        /// Processes a drag in the main grid.
        /// </summary>
        private void OnMainGridMouseMove(object sender, MouseEventArgs e)
        {
            // Exit if shift key and left mouse button aren't pressed
            if (e.LeftButton != MouseButtonState.Pressed) return;
            if (Keyboard.Modifiers != ModifierKeys.Shift) return;

            /* We use the m_MouseDirection value in the 
             * OnMainGridCheckDropTarget() event handler. */

            // Find the row the mouse button was pressed on
            var row = VisualHelper.FindParent<DataGridRow>(e.OriginalSource as FrameworkElement);
            _originalIndex = row.GetIndex();

            // If the row was already selected, begin drag
            if ((row != null) && row.IsSelected)
            {
                // Get the grocery item represented by the selected row
                var selectedItem = (DoliInput)row.Item;
                var finalDropEffect = DragDrop.DoDragDrop(row, selectedItem, DragDropEffects.Move);
                if ((finalDropEffect == DragDropEffects.Move) && (_targetItem != null))
                {
                    /* A drop was accepted. Determine the index of the item being 
                     * dragged and the drop location. If they are different, then 
                     * move the selectedItem to the new location. */

                    // Move the dragged item to its drop position
                    var oldIndex = _viewModel.DoliInputCollection.IndexOf(selectedItem);
                    var newIndex = _viewModel.DoliInputCollection.IndexOf(_targetItem);
                    if (oldIndex != newIndex) _viewModel.DoliInputCollection.Move(oldIndex, newIndex);
                    _targetItem = null;
                }
            }
        }


        #endregion

        #endregion

        #endregion

        #region Debug
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in _viewModel.DoliInputCollection)
            {
                Console.WriteLine("IsChecked = " + item.IsChecked + ", DestCTRL = " + item.DestCtrl.ToString() + ", destination = " + item.Destination.ToString() + ", speed = " + item.Speed.ToString() + ", SeqNum = " + item.SequenceNumber.ToString());
            }
            Console.WriteLine("#############");
        }
        #endregion
    }
}
