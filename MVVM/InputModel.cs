using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using InteractiveGraphUserControl.Utility;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveGraphUserControl.MVVM
{
    public class DoliInput : ObservableObject, ISequencedObject
    {
        #region Fields
        private string _moveCtrl;
        private double _speed;
        private string _limMode;
        private double _limit;
        private string _destCtrl;
        private double _destination;
        private string _destMode;
        private int _seqNb;
        private bool _isChecked;
        #endregion

        #region Properties
        public string MoveCtrl
        {
            get => _moveCtrl;
            set
            {
                _moveCtrl = value;
                OnPropertyChanged("MoveCtrl");
                if (value == "Halt")
                {
                    Speed = 1;
                    LimMode = "Not Active";
                    Limit = double.NaN;
                    DestMode = "Maintain";
                }
            }
        }
        public double Speed
        {
            get => _speed;
            set
            {
                _speed = value;
                OnPropertyChanged("Speed");
            }
        }
        public string LimMode
        {
            get => _limMode;
            set
            {
                _limMode = value;
                OnPropertyChanged("LimMode");
            }
        }
        public double Limit
        {
            get => _limit;
            set
            {
                _limit = value;
                OnPropertyChanged("Limit");
            }
        }
        public string DestCtrl
        {
            get => _destCtrl;
            set
            {
                _destCtrl = value;
                OnPropertyChanged("DestCtrl");
            }
        }
        public double Destination
        {
            get => _destination;
            set
            {
                _destination = value;
                OnPropertyChanged("Destination");
            }
        }
        public string DestMode
        {
            get => _destMode;
            set
            {
                _destMode = value;
                OnPropertyChanged("DestMode");
            }
        }

        public int SequenceNumber
        {
            get => _seqNb;
            set
            {
                _seqNb = value;
                OnPropertyChanged("SequenceNumber");
            }
        }

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }



        #endregion

        #region Constructor

        public DoliInput() { }

        public DoliInput(bool IsChecked, int SequenceNumber, string MoveCtrl, double Speed, string LimMode, double Limit, string DestCtrl, double Destination, string DestMode)
        {
            this._isChecked = IsChecked;
            this._seqNb = SequenceNumber;
            this._moveCtrl = MoveCtrl;
            this._speed = Speed;
            this._limMode = LimMode;
            this._limit = Limit;
            this._destCtrl = DestCtrl;
            this._destination = Destination;
            this._destMode = DestMode;
        }
        #endregion


    }






}
