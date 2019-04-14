using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiftSimulator.Interface
{
    public interface IAlgorithmChoiceElevator
    {
       Elevator ChooseOptimalElevatorToSend(Floor CallFloor, List<Elevator> listElevator);
    }
}
