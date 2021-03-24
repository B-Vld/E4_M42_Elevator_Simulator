using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace Elevator_M42_Echipa4
{

    public enum ElevatorStates
    {
        GoingUp,
        GoingDown,
        Idle,
        Stopped
    }


    public class Elevator : INotifyPropertyChanged
    {
        #region SettersAndGetters

        public double CurrentPosition { get; set; }
        public ElevatorStates State { get; set; }
        public double ElevatorSpeed { get; set; }

        private bool _isGoingDown;
        private bool _isGoingUp;
        private bool _isStopped;
        private bool _isIdle;

        public bool isElevatorStateIdle
        {
            get { return _isIdle; }
            set
            {
                if (_isIdle != value)
                {
                    _isIdle = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public bool isElevatorStateGoingUp 
        {
            get { return _isGoingUp; }
            set
            {
                if (_isGoingUp != value)
                {
                    _isGoingUp = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public bool isElevatorStateGoingDown 
        {
            get { return _isGoingDown; }
            set
            {
                if (_isGoingDown != value)
                {
                    _isGoingDown = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public bool isElevatorStateStopped
        {
            get { return _isStopped; }
            set 
            { 
                if (_isStopped != value)
                {
                    _isStopped = value;
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region HelperElevatorMethods


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private bool isDestinationTheSame(Elevator e, int destinationPos)
        {
            if (destinationPos == e.CurrentPosition)
                return true;
            return false;
        }

        public void determineDirection(Elevator e, int destinationPos)
        {
            bool t = isDestinationTheSame(e, destinationPos);
            if (t)
            {
                e.State = ElevatorStates.Idle;
            }
            else
            {
                if (e.CurrentPosition > destinationPos)
                    e.State = ElevatorStates.GoingDown;
                else
                    e.State = ElevatorStates.GoingUp;
            }
        }

        #endregion


    }
}
