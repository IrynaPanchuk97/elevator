using LiftSimulator.Custom_classes;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using LiftSimulator.Abstracts;
using LiftSimulator.Concrete;
using Timer = System.Timers.Timer;

namespace LiftSimulator
{
    public class ElevatorManager : Mediator
    {
        private readonly object _locker = new object();
        private readonly Elevator[] _arrayElevator;
        private readonly List<Elevator> _listElevator;
        private readonly Floor[] _arrayFloor;
        IAlgorithmChoiceElevator _algorithmChoiceElevator = new AlgorithmChoiceElevator();

        public ElevatorManager(Elevator[] arrayOfAllElevators, Floor[] arrayOfAllFloors)
        {
            _arrayElevator = arrayOfAllElevators;
            _arrayFloor = arrayOfAllFloors;
            _listElevator = new List<Elevator>();
            var floorChecker = new Timer(1000);
            floorChecker.Elapsed += ElevatorManager_TimerElapsed;
            floorChecker.Start();
        }

        public void AlgorithmChoiceElevatorStrategy(IAlgorithmChoiceElevator algorithmChoiceElevator)
        {
            _algorithmChoiceElevator = algorithmChoiceElevator;
        }

        public void PassengerNeedsAnElevator(Floor passengersFloor, Direction passengersDirection)
        {
            lock (_locker)
            {
                switch (passengersDirection)
                {
                    case Direction.up:
                        passengersFloor.LampUp = true;
                        break;
                    case Direction.down:
                        passengersFloor.LampDown = true;
                        break;
                }

                FreeElevator(passengersFloor);

                var toGoElevator = _algorithmChoiceElevator.ChooseOptimalElevatorToSend(passengersFloor, _listElevator);

                if (toGoElevator != null)
                {
                    SendAnElevator(toGoElevator, passengersFloor);
                }
            }
        }

        private void FreeElevator(Floor floor)
        {
            _listElevator.Clear();
            if (_arrayElevator.Select(t => t.GetListOfAllFloorsToVisit())
                .Any(listOfFloorsToVisit => listOfFloorsToVisit.Contains(floor)))
            {
                _listElevator.Clear();
                return;
            }

            foreach (var t in _arrayElevator)
            {
                if (t.GetElevatorStatus() == ElevatorStatus.Idle)
                {
                    _listElevator.Add(t);
                }
            }
        }

        private static void SendAnElevator(Elevator elevator, Floor floor)
        {
            elevator.AddNewFloorToTheList(floor);
            ThreadPool.QueueUserWorkItem(delegate { elevator.PrepareElevatorToGoToNextFloorOnTheList(); });
        }

        public void ElevatorManager_TimerElapsed(object sender, ElapsedEventArgs e)
        {
            foreach (var t in _arrayFloor)
            {
                if (t.LampUp)
                {
                    PassengerNeedsAnElevator(t, Direction.up);
                    Thread.Sleep(200);
                }
                else if (t.LampDown)
                {
                    PassengerNeedsAnElevator(t, Direction.down);
                    Thread.Sleep(200);
                }
            }
        }
    }
}
