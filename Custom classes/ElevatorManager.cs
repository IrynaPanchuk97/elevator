using LiftSimulator.Algorithm;
using LiftSimulator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;

namespace LiftSimulator
{
    public class ElevatorManager
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
                _arrayElevator[i].ElevatorIsFull += new EventHandler(ElevatorManager_ElevatorIsFull); 
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
                if (PassengersDirection == Direction.Up)
                {
                    PassengersFloor.LampUp = true;
                }
                else if (PassengersDirection == Direction.Down)
                {
                    PassengersFloor.LampDown = true;
                }
                FindAllElevatorsWhichCanBeSent(PassengersFloor, PassengersDirection);

                Elevator ElevatorToSend = algorithmChoiceElevator.ChooseOptimalElevatorToSend(PassengersFloor, _listElevator);

                if (ElevatorToSend != null)
                {
                    SendAnElevator(ElevatorToSend, PassengersFloor);
                }                
            }
        }

        private void FindAllElevatorsWhichCanBeSent(Floor PassengersFloor, Direction PassengersDirection)
        {
            _listElevator.Clear();

            for (int i = 0; i < _arrayElevator.Length; i++)
            {
                List<Floor> ListOfFloorsToVisit = _arrayElevator[i].GetListOfAllFloorsToVisit();
                if (ListOfFloorsToVisit.Contains(PassengersFloor))
                {
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

        private void SendAnElevator(Elevator ElevatorToSend, Floor TargetFloor)
        {            
            ElevatorToSend.AddNewFloorToTheList(TargetFloor);
            ThreadPool.QueueUserWorkItem(delegate { ElevatorToSend.PrepareElevatorToGoToNextFloorOnTheList(); });            
        }


        #region EVENT HANDLERS

        public void ElevatorManager_TimerElapsed(object sender, ElapsedEventArgs e)
        {
            //Check if some floor doesn't need an elevator
            for (int i = 0; i < _array_Floor.Length; i++)
                {
                    if (_array_Floor[i].LampUp)
                    {
                        PassengerNeedsAnElevator(_array_Floor[i], Direction.Up);
                        Thread.Sleep(500); //delay to avoid sending two elevators at a time
                    }
                    else if(_array_Floor[i].LampDown)
                    {
                        PassengerNeedsAnElevator(_array_Floor[i], Direction.Down);
                        Thread.Sleep(500); //delay to avoid sending two elevators at a time
                    }   
                }
        }

        public void ElevatorManager_ElevatorIsFull(object sender, EventArgs e)
        {    
            //TO DO: Implement or remove!
        }
        
        #endregion EVENT HANDLERS
    }
}
