using System;

namespace LiftSimulator
{
    public class PassengerEventArgs : EventArgs
    {
        public Passenger PassengerWhoRisedAnEvent;        

        public PassengerEventArgs(Passenger PassengerWhoRisedAnEvent)
        {
            this.PassengerWhoRisedAnEvent = PassengerWhoRisedAnEvent;
        }
    }
}
