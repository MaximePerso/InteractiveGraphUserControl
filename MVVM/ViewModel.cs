using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using InteractiveGraphUserControl.MVVM.Commands;
using InteractiveGraphUserControl.Utility;
using InteractiveGraphUserControl.Graph;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;
using LiveCharts.Events;

namespace InteractiveGraphUserControl.MVVM
{
    public class ViewModel : ViewModelBase
    {
        #region Events

        private void OnDoliCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Update item count
            ItemCount = DoliInputCollection.Count;

            // Resequence list
            SetCollectionSequence(DoliInputCollection);
        }

        #endregion

        #region Fields (partial, some of them were moved directly above their corresponding property)

        private ObservableCollection<DoliInput> _doliInputCollection;
        //graph visible items
        private bool _initialisationCheck;
        private bool _destLoadCheck;
        private bool _destPosCheck;
        private bool _limLoadCheck;
        private bool _limPosCheck;
        private bool _posSepCheck;
        private bool _loadSepCheck;
        //axis setting
        private double _loadAxisMax;
        private double _loadAxisMin;
        private double _positionAxisMax;
        private double _positionAxisMin;
        //mouse variables
        private int _mouseState = 0;
        private double _panelX;
        private double _panelY;
        //grid variable
        private int _itemCount;
        //shapes for drag animation on graph
        private double _xCoord;
        private int _selectedPointKey;
        private bool _bottomAreaMouseOver;
        private bool _topAreaMouseOver;
        private double _newPointValue;
        private double _ellipseCanvasLeft;
        private double _ellipseCanvasTop;
        private double _shapesOpacity;
        private SolidColorBrush _shapesColor;
        private double _leftX1;
        private double _leftX2;
        private double _leftY1;
        private double _leftY2;
        private double _rightX1;
        private double _rightX2;
        private double _rightY1;
        private double _rightY2;
        //private variables that have no properties associated to 
        private double _circleRadius = 10;
        private string _seriesName;
        private ChartValues<ObservablePoint> _selectedSeries;
        private readonly IView _chartView;
        //Max and min value to mange Y axis
        public double AllTimeLoadAxisMin = double.MaxValue;
        public double AllTimeLoadAxisMax = -double.MaxValue;
        public double AllTimePosAxisMin = double.MaxValue;
        public double AllTimePosAxisMax = -double.MaxValue;
        //shapes
        public Geometry SquareShape { get; set; }


        #endregion

        #region Constructor

        public ViewModel(IView chartView)
        {
            Initialise();
            _chartView = chartView;
        }

        #endregion

        #region Commands
        public ICommand DeleteItem { get; set; }
        public ICommand AddRow { get; set; }
        public ICommand MouseMove { get; set; }
        public ICommand MouseUp { get; set; }
        public ICommand Doli2Graph { get; set; }
        public ICommand CheckAll { get; set; }
        public ICommand CheckStatus { get; set; }
        public ICommand Duplicate { get; set; }
        public GraphCommand<ChartPoint> DataClickCommand { get; set; }
        public GraphCommand<LiveCharts.Wpf.CartesianChart> UpdaterTickCommand { get; set; }
        public GraphCommand<RangeChangedEventArgs> RangeChangedCommand { get; set; }
        public GraphCommand<ChartPoint> DataHoverCommand { get; set; }
        #endregion

        #region Properties

        #region DoliInputs
        public ObservableCollection<DoliInput> DoliInputCollection
        {
            get => _doliInputCollection;
            set
            {
                _doliInputCollection = value;
                OnPropertyChanged("DoliInputCollection");
            }
        }
        #endregion

        #region Grid
        public int ItemCount
        {
            get => _itemCount;
            set
            {
                _itemCount = value;
                OnPropertyChanged("ItemCount");
            }
        }

        private bool? _isAllChecked;
        public bool? IsAllChecked
        {
            get => _isAllChecked;
            set
            {
                _isAllChecked = value;
                OnPropertyChanged("IsAllChecked");
            }
        }
        public DoliInput SelectedItem { get; set; }

        #endregion

        #region Chart Values
        private ChartValues<ObservablePoint> _destLoadSeriesValues;
        public ChartValues<ObservablePoint> DestLoadSeriesValues
        {
            get => _destLoadSeriesValues;
            set
            {
                _destLoadSeriesValues = value;
                OnPropertyChanged("DestLoadSeriesValues");
            }
        }

        private ChartValues<ObservablePoint> _destPosSeriesValues;
        public ChartValues<ObservablePoint> DestPosSeriesValues
        {
            get => _destPosSeriesValues;
            set
            {
                _destPosSeriesValues = value;
                OnPropertyChanged("DestPosSeriesValues");
            }
        }

        private ChartValues<ObservablePoint> _limLoadSeriesValues;
        public ChartValues<ObservablePoint> LimLoadSeriesValues
        {
            get => _limLoadSeriesValues;
            set
            {
                _limLoadSeriesValues = value;
                OnPropertyChanged("LimLoadSeriesValues");
            }
        }

        private ChartValues<ObservablePoint> _limPosSeriesValues;
        public ChartValues<ObservablePoint> LimPosSeriesValues
        {
            get => _limPosSeriesValues;
            set
            {
                _limPosSeriesValues = value;
                OnPropertyChanged("LimPosSeriesValues");
            }
        }

        private int _ghostScalesYAt;
        public int GhostScalesYAt
        {
            get => _ghostScalesYAt;
            set
            {
                _ghostScalesYAt = value;
                OnPropertyChanged("GhostScalesYAt");
            }
        }

        public ChartValues<ObservablePoint> InitialisationSeries { get; set; }

        public string SeriesName
        {
            get => _seriesName;
            set
            {
                _seriesName = value;
                OnPropertyChanged("SeriesName");
            }
        }

        #endregion

        #region Axis
        public double LoadAxisMax
        {
            get => _loadAxisMax;
            set
            {
                _loadAxisMax = value;
                OnPropertyChanged("LoadAxisMax");
            }
        }

        public double LoadAxisMin
        {
            get => _loadAxisMin;
            set
            {
                _loadAxisMin = value;
                OnPropertyChanged("LoadAxisMin");
            }
        }

        public double PositionAxisMax
        {
            get => _positionAxisMax;
            set
            {
                _positionAxisMax = value;
                OnPropertyChanged("PositionAxisMax");
            }
        }

        public double PositionAxisMin
        {
            get => _positionAxisMin;
            set
            {
                _positionAxisMin = value;
                OnPropertyChanged("PositionAxisMin");
            }
        }

        public bool InitialisationCheck
        {
            get => _initialisationCheck;
            set
            {
                _initialisationCheck = value;
                OnPropertyChanged("InitialisationCheck");
            }
        }

        public bool DestPosCheck
        {
            get => _destPosCheck;
            set
            {
                _destPosCheck = value;
                OnPropertyChanged("DestPosCheck");
            }
        }

        public bool DestLoadCheck
        {
            get => _destLoadCheck;
            set
            {
                _destLoadCheck = value;
                OnPropertyChanged("DestLoadCheck");
            }
        }

        public bool LimLoadCheck
        {
            get => _limLoadCheck;
            set
            {
                _limLoadCheck = value;
                OnPropertyChanged("LimLoadCheck");
            }
        }

        public bool LimPosCheck
        {
            get => _limPosCheck;
            set
            {
                _limPosCheck = value;
                OnPropertyChanged("LimPosCheck");
            }
        }

        public bool PosSepCheck
        {
            get => _posSepCheck;
            set
            {
                _posSepCheck = value;
                OnPropertyChanged("PosSepCheck");
            }
        }

        public bool LoadSepCheck
        {
            get => _loadSepCheck;
            set
            {
                _loadSepCheck = value;
                OnPropertyChanged("LoadSepCheck");
            }
        }

        #endregion

        #region Mouse Properties
        public double PanelX
        {
            get { return _panelX; }
            set
            {
                if (value.Equals(_panelX)) return;
                _panelX = value;
                OnPropertyChanged("PanelX");
            }
        }

        public double PanelY
        {
            get { return _panelY; }
            set
            {
                if (value.Equals(_panelY)) return;
                _panelY = value;
                OnPropertyChanged("PanelY");
            }
        }

        public int MouseState
        {
            get => _mouseState;
            set
            {
                _mouseState = value;
                OnPropertyChanged("MouseState");
            }
        }
        #endregion

        #region Drag Points Shapes

        public double ShapesOpacity
        {
            get => _shapesOpacity;
            set
            {
                _shapesOpacity = value;
                OnPropertyChanged("ShapesOpacity");
            }
        }

        public SolidColorBrush ShapesColor
        {
            get => _shapesColor;
            set
            {
                _shapesColor = value;
                OnPropertyChanged("ShapesColor");
            }
        }

        public double EllipseCanvasLeft
        {
            get => _ellipseCanvasLeft;
            set
            {
                _ellipseCanvasLeft = value;
                OnPropertyChanged("EllipseCanvasLeft");
            }
        }


        public double EllipseCanvasTop
        {
            get => _ellipseCanvasTop;
            set
            {
                _ellipseCanvasTop = value;
                OnPropertyChanged("EllipseCanvasTop");
            }
        }

        public double LeftX1
        {
            get => _leftX1;
            set
            {
                _leftX1 = value;
                OnPropertyChanged("LeftX1");
            }
        }

        public double LeftX2
        {
            get => _leftX2;
            set
            {
                _leftX2 = value;
                OnPropertyChanged("LeftX2");
            }
        }

        public double LeftY1
        {
            get => _leftY1;
            set
            {
                _leftY1 = value;
                OnPropertyChanged("LeftY1");
            }
        }

        public double LeftY2
        {
            get => _leftY2;
            set
            {
                _leftY2 = value;
                OnPropertyChanged("LeftY2");
            }
        }

        public double RightX1
        {
            get => _rightX1;
            set
            {
                _rightX1 = value;
                OnPropertyChanged("RightX1");
            }
        }

        public double RightX2
        {
            get => _rightX2;
            set
            {
                _rightX2 = value;
                OnPropertyChanged("RightX2");
            }
        }

        public double RightY1
        {
            get => _rightY1;
            set
            {
                _rightY1 = value;
                OnPropertyChanged("RightY1");
            }
        }

        public double RightY2
        {
            get => _rightY2;
            set
            {
                _rightY2 = value;
                OnPropertyChanged("RightY2");
            }
        }

        public double XCoord
        {
            get => _xCoord;
            set
            {
                _xCoord = value;
                OnPropertyChanged("XCoord");
            }
        }

        public int SelectedPointKey
        {
            get => _selectedPointKey;
            set
            {
                _selectedPointKey = value;
                OnPropertyChanged("SelectedPointKey");
            }
        }

        public double NewPointValue
        {
            get => _newPointValue;
            set
            {
                _newPointValue = value;
                OnPropertyChanged("NewPointValue");
            }
        }

        public bool BottomAreaMouseOver
        {
            get => _bottomAreaMouseOver;
            set
            {
                _bottomAreaMouseOver = value;
                OnPropertyChanged("BottomAreaMouseOver");
            }
        }

        public bool TopAreaMouseOver
        {
            get => _topAreaMouseOver;
            set
            {
                _topAreaMouseOver = value;
                OnPropertyChanged("TopAreaMouseOver");
            }
        }
        #endregion

        #endregion

        #region Manage Sequencing

        /// <summary>
        ///     Resets the sequential order of a collection.
        /// </summary>
        /// <param name="targetCollection">The collection to be re-indexed.</param>
        public static ObservableCollection<T> SetCollectionSequence<T>(ObservableCollection<T> targetCollection)
            where T : ISequencedObject
        {
            // Initialize
            var sequenceNumber = 1;

            // Resequence
            foreach (ISequencedObject sequencedObject in targetCollection)
            {
                sequencedObject.SequenceNumber = sequenceNumber;
                sequenceNumber++;
            }

            // Set return value
            return targetCollection;
        }

        #endregion

        #region Private Methods

        #region Initialise

        private void Initialise()
        {
            //##############GRID AREA##############
            // Initialize commands
            //je laisse l'exemple avec la command complète dans MVVM/Commands/AddRowCommand
            //il faudrait alors remplacer la ligne suivante part:
            // AddRow = new AddRowCommand(this);
            AddRow = new RelayCommand(o => { DoliInputCollection.Add(new DoliInput(false, 0, "", 0, "", 0, "", 0, "")); }, o => true); //dans la version command il faut qu'une ligne soit sélectionnée pour que le bouton soit actif (CanExecute), la c'est tout le temps vrai
            //DeleteItem = new RelayCommand(o => { DoliInputCollection.Remove(SelectedItem); }, o => { return (SelectedItem != null); }); 
            DeleteItem = new RelayCommand(o => DeleteInput(), o => { return (IsAllChecked == null || IsAllChecked == true); }); 
            MouseMove = new RelayCommand(o => MouseMoveLogic(), o => { return (MouseState != 0); }); 
            MouseUp = new RelayCommand(o => MouseUpLogic(), o => { return (MouseState != 0); });
            Doli2Graph = new RelayCommand(o => DoliToGraph(), o => true);
            CheckAll = new RelayCommand(o => CheckAllFunc(), o => true);
            CheckStatus = new RelayCommand(o => StatusCheck(), o => true);
            Duplicate = new RelayCommand(o => DuplicateInput(), o => { return (IsAllChecked == null || IsAllChecked == true); });

            //Create inputList
            _doliInputCollection = new ObservableCollection<DoliInput>();

            //Set tha all selected to uncheck
            _isAllChecked = false;

            //Set _selectedSeries to avoid problems
            _selectedSeries = DestLoadSeriesValues;

            //Add items
            _doliInputCollection.Add(new DoliInput(false, 1,"Load", 500, "Absolute",10,"Position", 3, "Approach"));
            _doliInputCollection.Add(new DoliInput(false, 0,"Load", 500, "Not Active",10,"Load", 3000, "Approach"));
            _doliInputCollection.Add(new DoliInput(false, 0,"Load", 250, "Not Active", 10,"Load", 4000, "Approach"));
            _doliInputCollection.Add(new DoliInput(false, 0,"Load", 500, "Not Active", 10,"Load", 10000, "Approach"));
            _doliInputCollection.Add(new DoliInput(false, 1,"Load", 1000, "Absolute",10,"Position", 3, "Approach"));
            _doliInputCollection.Add(new DoliInput(false, 1,"Position", 1, "Not Active", 10,"Position", 14, "Approach"));

            //Subscribe to the event that gets trigger when change occurs
            _doliInputCollection.CollectionChanged += OnDoliCollectionChanged;

            //Start indexing items
            DoliInputCollection = SetCollectionSequence(DoliInputCollection);

            //Update if changes
            OnPropertyChanged("DoliInputCollection");
            OnPropertyChanged("GridParam");


            //##############GRAPH AREA##############
            SquareShape = DefaultGeometries.Square;

            DestLoadSeriesValues = new ChartValues<ObservablePoint>();

            DestPosSeriesValues = new ChartValues<ObservablePoint>();

            LimLoadSeriesValues = new ChartValues<ObservablePoint>();

            LimPosSeriesValues = new ChartValues<ObservablePoint>();

            InitialisationSeries = new ChartValues<ObservablePoint>
            {
                new ObservablePoint(-5,3),
                new ObservablePoint(0,3)
            };

            GhostScalesYAt = 0;

            //set grpah item visibility
            _initialisationCheck = true;
            _destLoadCheck = true;
            _destPosCheck = true;
            _limLoadCheck = true;
            _limPosCheck = true;
            _posSepCheck = false;
            _loadSepCheck = true;

            //set axis values
            _positionAxisMax = double.NaN;
            _positionAxisMin = double.NaN;
            _loadAxisMax = double.NaN;
            _loadAxisMin = double.NaN;

            //Graph commands
            DataClickCommand = new GraphCommand<ChartPoint>()
            {
                ExecuteDelegate = p => {
                    DataClick(p);
                }

            };

            UpdaterTickCommand = new GraphCommand<LiveCharts.Wpf.CartesianChart>
            {
                ExecuteDelegate = c =>
                {
                    UpdateGraph();
                }
            };

            RangeChangedCommand = new GraphCommand<RangeChangedEventArgs>
            {
                //ExecuteDelegate = e => Console.WriteLine("[COMMAND] Axis range changed")
            };

            DataHoverCommand = new GraphCommand<ChartPoint>
            {
                //ExecuteDelegate = p => Console.WriteLine(
                //    "[COMMAND] you hovered over " + p.X + ", " + p.Y)
            };

        }


        #endregion

        #region Private Graph Methods
        //finds max in chart point list
        private double MaxValue(ChartValues<ObservablePoint> list, string CTRL)
        {
            double max;
            if(CTRL == "load")
            {
                max = AllTimeLoadAxisMax;
            }
            else
            {
                max = AllTimePosAxisMax;
            }
            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i].Y > max)
                {
                    max = list[i].Y;
                }
            }
            if (CTRL == "load")
            {
                AllTimeLoadAxisMax = max;
            }
            else
            {
                 AllTimePosAxisMax = max;
            }
            max++;
            return max;
        }
        //finds min in chart point list
        private double MinValue(ChartValues<ObservablePoint> list, string CTRL)
        {
            double min;
            if (CTRL == "load")
            {
                min = AllTimeLoadAxisMin;
            }
            else
            {
                min = AllTimePosAxisMin;
            }
            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i].Y < min)
                {
                    min = list[i].Y;
                }
            }
            if (CTRL == "load")
            {
                AllTimeLoadAxisMin = min;
            }
            else
            {
                AllTimePosAxisMin = min;
            }
            min--;
            return min;
        }

        private void MouseUpLogic()
        {
            //error can appear if the user's cursor is on the graph before it is fully loaded
            try
            {
                MouseState = 0;
                ShapesOpacity = 0;
                //ChartUpdate();
                //Graph back to autoscaling
                PositionAxisMax = double.NaN;
                PositionAxisMin = double.NaN;
                LoadAxisMax = double.NaN;
                LoadAxisMin = double.NaN;
                //Reset max
                AllTimeLoadAxisMin = double.MaxValue;
                AllTimeLoadAxisMax = -double.MaxValue;
                AllTimePosAxisMin = double.MaxValue;
                AllTimePosAxisMax = -double.MaxValue;
            }
            catch { }
            
        }

        private void MouseMoveLogic()
        {
            try
            {
                Point mousePoint = new Point(XCoord, PanelY);
                PlotDragAnimation(mousePoint.X, mousePoint.Y);

                if (TopAreaMouseOver)
                {
                    OperationOnValue("Add", 1);
                }
                else if (BottomAreaMouseOver)
                {
                    OperationOnValue("Add", -1);
                }
                else
                {
                    OperationOnValue("Set");
                }
                PlotDragAnimation(mousePoint.X, mousePoint.Y);
                if (AllTimePosAxisMax != -double.MaxValue)
                    PositionAxisMax = AllTimePosAxisMax;
                if (AllTimePosAxisMin != double.MaxValue)
                    PositionAxisMin = AllTimePosAxisMin;
                if (AllTimeLoadAxisMax != -double.MaxValue)
                    LoadAxisMax = AllTimeLoadAxisMax;
                if (AllTimeLoadAxisMin != double.MaxValue)
                    LoadAxisMin = AllTimeLoadAxisMin;

            }
            catch(Exception e) { Console.WriteLine(e); }
        }

        #region Graph Logic
        private void UpdateGraph()
        {
            if (MouseState == 1)
            {
                Point mousePoint = new Point(XCoord, PanelY);
                PlotDragAnimation(mousePoint.X, mousePoint.Y);

                if (TopAreaMouseOver)
                {
                    OperationOnValue("Add",1);
                }
                else if (BottomAreaMouseOver)
                {
                    OperationOnValue("Add",-1);
                }
                else
                {
                    OperationOnValue("Set");
                }
                PlotDragAnimation(mousePoint.X, mousePoint.Y);
                if (AllTimePosAxisMax != -double.MaxValue)
                    PositionAxisMax = AllTimePosAxisMax;
                if (AllTimePosAxisMin != double.MaxValue)
                    PositionAxisMin = AllTimePosAxisMin;
                if (AllTimeLoadAxisMax != -double.MaxValue)
                    LoadAxisMax = AllTimeLoadAxisMax;
                if (AllTimeLoadAxisMin != double.MaxValue)
                    LoadAxisMin = AllTimeLoadAxisMin;
                _selectedSeries[SelectedPointKey].Y = NewPointValue;
            }
        }

        private void DataClick(ChartPoint p)
        {
            // This event gets fire before MouseDown
            //Specify data has been clicked
            MouseState = 1;
            SelectedPointKey = p.Key;
            _selectedSeries = SeriesSelection(p);
            SeriesName = p.SeriesView.Title;
            //duplicates the series we want to modify
            if (SeriesName.Contains("Load"))
            {
                GhostScalesYAt = 1;
            }
            else
            {
                GhostScalesYAt = 0;
            }
            Point mousePoint = new Point(PanelX, PanelY);
            //get current mouse X position to block X displacement (only Y coord of the point can be edited through drag and drop)
            XCoord = mousePoint.X;
        }

        private ChartValues<ObservablePoint> SeriesSelection(ChartPoint p)
        {
            var series = DestPosSeriesValues;
            ShapesColor = Brushes.DodgerBlue;
            if (p.SeriesView.Title.Contains("Load"))
            {
                ShapesColor = Brushes.IndianRed;
                if (p.SeriesView.Title.Contains("Dest"))
                    series = DestLoadSeriesValues;
                else
                    series = LimLoadSeriesValues;
            }
            else
            {
                if (p.SeriesView.Title.Contains("Lim"))
                    series = LimPosSeriesValues;
            }
            return series;
        }

        private Point GetPreviousPoint()
        {
            Point point = new Point();
            if (SelectedPointKey != 0)
            {
                //get the chart values of the point on the left of the selected point
                int x1 = SelectedPointKey - 1;
                double y1 = _selectedSeries[x1].Y;
                point = new Point(x1, y1);
                //convert point into pixel
                //prevPoint = _myChart.ConvertToPixels(point);
            }
            else
            {
                int x1 = SelectedPointKey;
                point = new Point(x1, _selectedSeries[x1].Y);
                //prevPoint = _myChart.ConvertToPixels(point);
            }

            Point prevPoint = _chartView.ToPixel(point);
            return prevPoint;
        }

        private Point GetNextPoint()
        {
            Point point = new Point();
            if (Convert.ToInt32(SelectedPointKey) != _selectedSeries.Count() - 1)
            {
                //get the chart values of the point on the right of the selected point
                int x1 = Convert.ToInt32(SelectedPointKey) + 1;
                double y1 = _selectedSeries[x1].Y;
                point = new Point(x1, y1);
                //convert point into pixel
                //nextPoint = _myChart.ConvertToPixels(point);

            }
            else
            {
                int x1 = Convert.ToInt32(SelectedPointKey);
                point = new Point(x1, _selectedSeries[x1].Y);
                //nextPoint = _myChart.ConvertToPixels(point);
            }
            Point nextPoint = _chartView.ToPixel(point);
            return nextPoint;
        }

        private double GetMax(double A, double B)
        {
            return (A > B && A != double.NaN) ? A : (B != double.NaN) ? B : double.NaN;
        }

        private double GetMin(double A, double B)
        {
            return (A < B && A != double.NaN) ? A : (B != double.NaN) ? B : double.NaN;
        }

        private void ClearAllSeries()
        {
            //clear all SeriesValues
            DestLoadSeriesValues.Clear();
            DestPosSeriesValues.Clear();
            LimLoadSeriesValues.Clear();
            LimPosSeriesValues.Clear();
        }

        private void DeleteInput()
        {
            List<int> toRemove = new List<int>();
            for(int i=0; i<DoliInputCollection.Count();i++)
            {
                DoliInput input = DoliInputCollection[i];
                if (input.IsChecked)
                    toRemove.Add(input.SequenceNumber - 1);
            }
            while (toRemove.Count() != 0)
            {
                DoliInputCollection.RemoveAt(toRemove[0]);
                toRemove.RemoveAt(0);
                toRemove = toRemove.Select(x => x - 1).ToList();
            }
            StatusCheck();
        }

        //Mother of All Function (MoAF)
        private void DoliToGraph()
        {
            double defaultLoadSpeed = 100; //[N/s]
            double defaultPosSpeed = 1; //[mm/s]
            //double epsilonTime = 0.000000001;
            double initialGraphOffset = 0;
            ClearAllSeries();

            for(int i =0; i<DoliInputCollection.Count();i++)
            {
                var input = DoliInputCollection[i];
                //first input has to be treated differently
                if (input.SequenceNumber == 1)
                {
                    ObservablePoint NewDestLoad = new ObservablePoint(initialGraphOffset, double.NaN);
                    ObservablePoint NewDestPos = new ObservablePoint(initialGraphOffset, input.Destination);
                    ObservablePoint NewLimLoad = new ObservablePoint(initialGraphOffset, double.NaN);
                    ObservablePoint NewLimPos = new ObservablePoint(initialGraphOffset, input.Limit);
                    if (input.DestCtrl == "Load")
                    {
                        NewDestLoad.Y = input.Destination;
                        NewDestPos.Y = double.NaN;
                    }
                    if (input.MoveCtrl == "Load")
                    {
                        NewLimLoad.Y = input.Limit;
                        NewLimPos.Y = double.NaN;
                    }

                    if (input.LimMode == "Not active")
                    {
                        NewLimLoad.Y = double.NaN;
                        NewLimPos.Y = double.NaN;
                    }
                    DestLoadSeriesValues.Add(NewDestLoad);
                    DestPosSeriesValues.Add(NewDestPos);
                    LimLoadSeriesValues.Add(NewLimLoad);
                    LimPosSeriesValues.Add(NewLimPos);
                }
                else
                {
                    //previous input
                    var prevInput = DoliInputCollection[i - 1];
                    double prevDest;
                    if(prevInput.DestCtrl == "Load")
                        prevDest = DestLoadSeriesValues[i - 1].Y;
                    else
                        prevDest = DestPosSeriesValues[i - 1].Y;
                    
                    //slope determination
                    //representation of the time required to perform the command. IT IS JUST A REPRESENTATION BASED ON A DEFAULT SPEED, IT CANNOT BE ACCURATE FOR OBVIOUS REASONS
                    double timeSpan = input.Speed / defaultPosSpeed;
                    if (input.MoveCtrl == "Load")
                        timeSpan = input.Speed / defaultLoadSpeed;
                    if (prevInput.DestCtrl == input.DestCtrl && input.DestCtrl == input.MoveCtrl)
                    {
                        double deltaDest = Math.Abs(input.Destination - prevDest);
                        timeSpan = deltaDest / input.Speed;
                    }
                    else if (prevInput.DestCtrl != input.DestCtrl)
                        timeSpan = 1;

                    //all point from load and position curve share the same point X coordinate (each segment is an input)
                    double timeCoord = DestLoadSeriesValues[i - 1].X + timeSpan;
                    //Je ne supprimme pas les prochaines lignes. J'avais une idée, mais je ne la retrouve plus #triseDansMonCoeur
                    //les 4 prochains IF sont là pour permettre de tracer les courbes en cas de non continuité du MoveCtrl. Manupulation du comportement de lvchart
                    //if (prevInput.MoveCtrl == "Load" && input.MoveCtrl == "Position")
                    //    LimPosSeriesValues.Add(new ObservablePoint(LimPosSeriesValues[i - 1].X + epsilonTime, LimPosSeriesValues[i - 1].Y));
                    //if (prevInput.MoveCtrl == "Position" && input.MoveCtrl == "Load")
                    //    LimLoadSeriesValues.Add(new ObservablePoint(LimLoadSeriesValues[i - 1].X + epsilonTime, LimLoadSeriesValues[i - 1].Y));
                    //if (prevInput.DestCtrl == "Load" && input.DestCtrl == "Position")
                    //    DestLoadSeriesValues.Add(new ObservablePoint(DestLoadSeriesValues[i - 1].X + epsilonTime, DestLoadSeriesValues[i - 1].Y));
                    //if (prevInput.DestCtrl == "Position" && input.DestCtrl == "Load")
                    //    DestPosSeriesValues.Add(new ObservablePoint(DestPosSeriesValues[i - 1].X + epsilonTime, DestPosSeriesValues[i - 1].Y));

                    ObservablePoint NewDestLoad = new ObservablePoint(timeCoord, double.NaN);
                    ObservablePoint NewDestPos = new ObservablePoint(timeCoord, input.Destination);
                    ObservablePoint NewLimLoad = new ObservablePoint(timeCoord, double.NaN);
                    ObservablePoint NewLimPos = new ObservablePoint(timeCoord, input.Limit);

                    if (input.DestCtrl == "Load")
                    {
                        NewDestLoad.Y = input.Destination;
                        NewDestPos.Y = double.NaN;
                    }

                    if(input.LimMode == "Not Active")
                    {
                        NewLimLoad.Y = double.NaN;
                        NewLimPos.Y = double.NaN;
                    }

                    if (input.LimMode == "Absolute" && input.MoveCtrl == "Load")
                    {
                        NewLimLoad.Y = input.Limit;
                        NewLimPos.Y = double.NaN;
                    }
                    
                    if (input.LimMode == "Relative")
                    {
                        NewLimPos.Y = LimPosSeriesValues[i - 1].Y + input.Limit;
                        if (input.MoveCtrl == "Load")
                        {
                            NewLimLoad.Y = LimLoadSeriesValues[i - 1].Y + input.Limit;
                            NewLimPos.Y = double.NaN; 
                        }
                    }
                    DestLoadSeriesValues.Add(NewDestLoad);
                    DestPosSeriesValues.Add(NewDestPos);
                    LimLoadSeriesValues.Add(NewLimLoad);
                    LimPosSeriesValues.Add(NewLimPos);
                }
            }
        }

        private void CheckAllFunc()
        {
            if(IsAllChecked == true)
            {
                foreach(var input in DoliInputCollection)
                {
                    input.IsChecked = true;
                }
            }
            else
            {
                foreach (var input in DoliInputCollection)
                {
                    input.IsChecked = false;
                }
            }
        }

        private void StatusCheck()
        {
            int i = 0;
            foreach (var input in DoliInputCollection)
            {
                if (input.IsChecked)
                {
                    i++;
                }
            }
            if (i == 0)
                IsAllChecked = false;
            else if (i < DoliInputCollection.Count())
                IsAllChecked = null;
            else
                IsAllChecked = true;
        }

        private void DuplicateInput()
        {
            //sexier way to code the same idea as in DeleteInput()
            foreach(var input in DoliInputCollection.ToList())
            {
                if (input.IsChecked)
                {
                    input.IsChecked = false;
                    DoliInputCollection.Add(new DoliInput(false,0,input.MoveCtrl, input.Speed, input.LimMode, input.Limit, input.DestCtrl, input.Destination, input.DestMode));
                }
            }
            StatusCheck();
        }
        #endregion
        #endregion

        #endregion

        #region Public Methods

        public void AddInput(bool isChecked, int SeqNb, string MoveCtrl, double Speed, string LimMode, double Limit, string DestCtrl, double Destination, string DestMode)
        {
            DoliInputCollection.Add(new DoliInput(isChecked, SeqNb, MoveCtrl, Speed, LimMode, Limit, DestCtrl, Destination, DestMode));
        }

        //c'pas très bô, faudrait pas hardcoder, mais il est 2h du mat'
        public void OperationOnValue(string operation, double offset = 0.0)
        {
            if (operation == "Add")
            {
                if (SeriesName.Contains("Load"))
                {
                    if (SeriesName.Contains("Dest"))
                        DestLoadSeriesValues[SelectedPointKey].Y += offset;
                    else
                        LimLoadSeriesValues[SelectedPointKey].Y += offset;
                }
                else
                {
                    if (SeriesName.Contains("Dest"))
                        DestPosSeriesValues[SelectedPointKey].Y += offset;
                    else
                        LimPosSeriesValues[SelectedPointKey].Y += offset;
                }
            }
            else if (operation == "Set")
            {
                double Y = _chartView.ToChartValue(GhostScalesYAt).Y;
                if (SeriesName.Contains("Dest"))
                {
                    DoliInputCollection[SelectedPointKey].Destination = Math.Round(Y,1) ;
                    if (SeriesName.Contains("Load"))
                    {
                        DestLoadSeriesValues[SelectedPointKey].Y = Y;
                    }
                    else
                        DestPosSeriesValues[SelectedPointKey].Y = Y;
                }
                else
                {
                    DoliInputCollection[SelectedPointKey].Limit = Math.Round(Y,1);
                    if (SeriesName.Contains("Load"))
                    {
                        LimLoadSeriesValues[SelectedPointKey].Y = Y;
                    }
                    else
                        LimPosSeriesValues[SelectedPointKey].Y = Y;
                }
            }
            //Freezes the chart axis range
            PositionAxisMax = GetMax(MaxValue(DestPosSeriesValues, "position"), MaxValue(LimPosSeriesValues, "position"));
            PositionAxisMin = GetMin(MinValue(DestPosSeriesValues, "position"), MinValue(LimPosSeriesValues, "position"));
            LoadAxisMax = GetMax(MaxValue(DestLoadSeriesValues, "load"), MaxValue(LimLoadSeriesValues, "load"));
            LoadAxisMin = GetMin(MinValue(DestLoadSeriesValues, "load"), MinValue(LimLoadSeriesValues, "load"));
        }

        public void PlotDragAnimation(double X, double Y, double radius = 20.0)
        {
            //get what would be the graph value of the current point. I put it here so it updates as the mouse moves
            NewPointValue = _chartView.ToChartValue(GhostScalesYAt).Y;
            //update mouse position relative to top and bottom invisible rectangles
            BottomAreaMouseOver = _chartView.BottomAreaMouseOver();
            TopAreaMouseOver = _chartView.TopAreaMouseOver();
            ShapesOpacity = 1;
            _circleRadius = radius;
            EllipseCanvasLeft = X - _circleRadius / 2;
            EllipseCanvasTop = Y - _circleRadius / 2;

            //assign coordinates for the left line
            LeftX1 = GetPreviousPoint().X;
            LeftY1 = GetPreviousPoint().Y;

            LeftX2 = X;
            LeftY2 = Y;

            //assign coordinates for the right line
            RightX1 = X;
            RightY1 = Y;
            RightX2 = GetNextPoint().X;
            RightY2 = GetNextPoint().Y;
        }

        public void ChartUpdate()
        {
            _chartView.ChartUpdate();
        }


        #endregion









    }
}