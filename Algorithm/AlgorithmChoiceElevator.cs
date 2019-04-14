using LiftSimulator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiftSimulator.Algorithm
{
    class AlgorithmChoiceElevator : IAlgorithmChoiceElevator
    {
        public Elevator ChooseOptimalElevatorToSend(Floor CallFloor, List<Elevator> listElevator)
        {
            if (listElevator.Count == 0)
            {
                return null;
            }
            return listElevator[0];
        }
    }
    class AlgorithmChoiceElevator2 : IAlgorithmChoiceElevator
    {
        public Elevator ChooseOptimalElevatorToSend(Floor CallFloor, List<Elevator> listElevator)
        {
            if (listElevator.Count == 0)
            {
                return null;
            }
            int k = 0;
            int min = listElevator[0].GetCurrentFloor().FloorIndex - CallFloor.FloorIndex;
            for(int i=0;i<listElevator.Count;i++)
            {
                if(listElevator[0].GetCurrentFloor().FloorIndex - CallFloor.FloorIndex< min)
                {
                    min = listElevator[0].GetCurrentFloor().FloorIndex - CallFloor.FloorIndex;
                    k = i;
                }
            }
            return listElevator[k];
        }
    }
}
