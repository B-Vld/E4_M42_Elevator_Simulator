using System;
using System.Collections.Generic;
using System.Linq;
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


    public class Elevator
    {
        public static int numberOfFloors = 5;
        public static double elevatorSpeed = 0.025;

        public double CurrentPosition { get; set; }
        public ElevatorStates State { get; set; }


        private bool isDestinationTheSame(Elevator e,int destinationPos)
        {
            if (destinationPos == e.CurrentPosition)
                return true;
            return false;
        }

        public void determineDirection(Elevator e, int destinationPos)
        {
            bool t = isDestinationTheSame(e,destinationPos);
            if (t)
            {
                e.State = (ElevatorStates)2; 
            }
            else
            {
                if (e.CurrentPosition > destinationPos)
                    e.State = (ElevatorStates)1;
                else
                    e.State = 0;
            }
        }

        public ElevatorStates GetElevatorStates()
        {
            return this.State;
        }




    }
}
