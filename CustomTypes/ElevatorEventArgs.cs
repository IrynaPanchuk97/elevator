using System;

namespace LiftSimulator
{
    /// <summary>
    /// EventArgs needed for Floor class to send reference elevator, which arrived
    /// or reference to passenger, who entered the elevator
    /// </summary>
    public class ElevatorEventArgs : EventArgs
    {
        public Elevator ElevatorWhichRisedAnEvent;

        public ElevatorEventArgs(Elevator ElevatorWhichArrived)
        {
            this.ElevatorWhichRisedAnEvent = ElevatorWhichArrived;            
        }
    }
}
