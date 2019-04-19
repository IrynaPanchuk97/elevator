using System.Collections.Generic;
using LiftSimulator.AbstractServices;

namespace LiftSimulator.ConcreteServices
{
    internal class AlgorithmChoiceElevator : IAlgorithmChoiceElevator
    {
        public Elevator ChooseOptimalElevatorToSend(Floor callFloor, List<Elevator> listElevator)
        {
            return listElevator.Count == 0 ? null : listElevator[0];
        }
    }

    internal class AlgorithmChoiceElevator2 : IAlgorithmChoiceElevator
    {
        public Elevator ChooseOptimalElevatorToSend(Floor callFloor, List<Elevator> listElevator)
        {
            if (listElevator.Count == 0)
            {
                return null;
            }

            var k = 0;
            var min = listElevator[0].GetCurrentFloor().FloorIndex - callFloor.FloorIndex;
            for (var i = 0; i < listElevator.Count; i++)
            {
                if (listElevator[0].GetCurrentFloor().FloorIndex - callFloor.FloorIndex >= min)
                {
                    continue;
                }

                min = listElevator[0].GetCurrentFloor().FloorIndex - callFloor.FloorIndex;
                k = i;
            }

            return listElevator[k];
        }
    }
}
