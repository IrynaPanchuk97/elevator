using System;

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
