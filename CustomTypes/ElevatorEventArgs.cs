using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiftSimulator
{
    public class ElevatorEventArgs : EventArgs
    {
        public Elevator ElevatorWhichRisedAnEvent;

        public ElevatorEventArgs(Elevator ElevatorWhichArrived)
        {
            this.ElevatorWhichRisedAnEvent = ElevatorWhichArrived;            
        }
    }
}
