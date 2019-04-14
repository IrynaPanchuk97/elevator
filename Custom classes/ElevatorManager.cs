using LiftSimulator.Algorithm;
using LiftSimulator.Custom_classes;
using LiftSimulator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;

namespace LiftSimulator
{
    public class ElevatorManager:Mediator
    {
        private readonly object locker = new object();
        private Elevator[] _arrayElevator;
        public Elevator[] ArrayOfAllElevators{get; set;}
        private List<Elevator> _listElevator;
        private Floor[] _array_Floor;
        private System.Timers.Timer floorChecker;
        IAlgorithmChoiceElevator algorithmChoiceElevator = new AlgorithmChoiceElevator();

        public ElevatorManager(Elevator[] ArrayOfAllElevators, Floor[] ArrayOfAllFloors)
        {
            this._arrayElevator = ArrayOfAllElevators;
            for (int i = 0; i < _arrayElevator.Length; i++) {              
            }

            this._array_Floor = ArrayOfAllFloors;
            this._listElevator = new List<Elevator>();
            this.floorChecker = new System.Timers.Timer(1000);
            this.floorChecker.Elapsed += new ElapsedEventHandler(this.ElevatorManager_TimerElapsed);
            this.floorChecker.Start();
        }

        public void AlgorithmChoiceElevatorStrategy(IAlgorithmChoiceElevator algorithmChoiceElevator)
        {
            this.algorithmChoiceElevator = algorithmChoiceElevator;
        }

        public void PassengerNeedsAnElevator(Floor PassengersFloor, Direction PassengersDirection)
        {
            lock (locker)
            {
                if (PassengersDirection == Direction.up)
                {
                    PassengersFloor.LampUp = true;
                }
                else if (PassengersDirection == Direction.down)
                {
                    PassengersFloor.LampDown = true;
                }
                FreeElevator(PassengersFloor, PassengersDirection);

                Elevator ToGoElevator = algorithmChoiceElevator.ChooseOptimalElevatorToSend(PassengersFloor, _listElevator);

                if (ToGoElevator != null)
                {
                    SendAnElevator(ToGoElevator, PassengersFloor);
                }                
            }
        }
        private void FreeElevator(Floor floor, Direction direct)
        {
            _listElevator.Clear();
            for (int i = 0; i < _arrayElevator.Length; i++){
                List<Floor> ListOfFloorsToVisit = _arrayElevator[i].GetListOfAllFloorsToVisit();
                if (ListOfFloorsToVisit.Contains(floor)){
                    _listElevator.Clear();
                    return; 
                }
            }
            for (int i = 0; i < _arrayElevator.Length; i++)
            {
                if (_arrayElevator[i].GetElevatorStatus() == ElevatorStatus.Idle) 
                {
                    _listElevator.Add(_arrayElevator[i]);
                }
            }
        }
        private void SendAnElevator(Elevator elevator, Floor floor)
        {            
            elevator.AddNewFloorToTheList(floor);
            ThreadPool.QueueUserWorkItem(delegate { elevator.PrepareElevatorToGoToNextFloorOnTheList(); });            
        }

        public void ElevatorManager_TimerElapsed(object sender, ElapsedEventArgs e)
        {
            for (int i = 0; i < _array_Floor.Length; i++)
                {
                    if (_array_Floor[i].LampUp)
                    {
                        PassengerNeedsAnElevator(_array_Floor[i], Direction.up);
                        Thread.Sleep(200);
                    }
                    else if(_array_Floor[i].LampDown)
                    {
                        PassengerNeedsAnElevator(_array_Floor[i], Direction.down);
                        Thread.Sleep(200);
                    }   
                }
        }

    }
}
