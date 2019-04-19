using System.Collections.Generic;

namespace LiftSimulator.AbstractServices
{
    public interface IAlgorithmChoiceElevator
    {
       Elevator ChooseOptimalElevatorToSend(Floor callFloor, List<Elevator> listElevator);
    }
}
