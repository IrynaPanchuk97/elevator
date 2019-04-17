using System.Collections.Generic;

namespace LiftSimulator.Abstracts
{
    public interface IAlgorithmChoiceElevator
    {
       Elevator ChooseOptimalElevatorToSend(Floor callFloor, List<Elevator> listElevator);
    }
}
