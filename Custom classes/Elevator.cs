using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Timers;

namespace LiftSimulator
{
     public class Elevator
     {
        #region FIELDS

        private readonly object _locker = new object();

        private readonly Building _myBuilding;

        private Floor _currentFloor;
        private readonly List<Floor> _listOfFloorsToVisit;
        private Direction _elevatorDirection;
        private ElevatorStatus _elevatorStatus;

        private readonly int _maximumPeopleInside;
        private readonly List<Passenger> _listOfPeopleInside;
        private bool _isFull;

        private Point _elevatorPosition;
        private readonly Bitmap[] _elevatorFrames;
        private int _currentFrameNumber;
        private readonly int _elevatorAnimationDelay;
        private readonly System.Timers.Timer _elevatorTimer;

        #endregion


        #region METHODS

        public Elevator(Building myBuilding, int horizontalPosition, Floor startingFloor)
        {
            _myBuilding = myBuilding;

            _currentFloor = startingFloor;
            _listOfFloorsToVisit = new List<Floor>();
            _elevatorDirection = Direction.stop;
            _elevatorStatus = ElevatorStatus.Idle;

            _maximumPeopleInside = 2;
            _listOfPeopleInside = new List<Passenger>();
            _isFull = false;

            _elevatorPosition = new Point(horizontalPosition, _currentFloor.GetFloorLevelInPixels());
            _currentFrameNumber = 0;
            _elevatorFrames = new[]
            {
                Properties.Resources.LiftDoors_Open,
                Properties.Resources.LiftDoors_4,
                Properties.Resources.LiftDoors_3,
                Properties.Resources.LiftDoors_2,
                Properties.Resources.LiftDoors_1,
                Properties.Resources.LiftDoors_Closed
            };
            _elevatorAnimationDelay = 8;
            _elevatorTimer = new System.Timers.Timer(6000); //set timer to 6 seconds
            _elevatorTimer.Elapsed += Elevator_ElevatorTimerElapsed;

            PassengerEnteredTheElevator += Elevator_PassengerEnteredTheElevator;

            //Add new elevator to floor's list
            _currentFloor.AddRemoveElevatorToTheListOfElevatorsWaitingHere(this, true);
        }

        public void PrepareElevatorToGoToNextFloorOnTheList()
        {
            //Method can be invoked from ElevatorManager thread (SendAnElevator()) or elevator's timer thread (Elevator_ElevatorTimerElapsed())

            //Update elevator's status
            SetElevatorStatus(ElevatorStatus.PreparingForJob);

            //Disable the timer
            _elevatorTimer.Stop();

            //Remove this elevator from current floor's list
            _currentFloor.AddRemoveElevatorToTheListOfElevatorsWaitingHere(this, false);

            //Close the door
            CloseTheDoor();

            //Go!
            GoToNextFloorOnTheList();
        }

        private void GoToNextFloorOnTheList()
        {
            switch (_elevatorDirection)
            {
                //Move control on the UI                 
                //move down
                case Direction.down:
                    SetElevatorStatus(ElevatorStatus.GoingDown);
                    MoveTheElevatorGraphicDown(GetNextFloorToVisit().GetFloorLevelInPixels());
                    break;
                //move up
                case Direction.up:
                    SetElevatorStatus(ElevatorStatus.GoingUp);
                    MoveTheElevatorGraphicUp(GetNextFloorToVisit().GetFloorLevelInPixels());
                    break;
            }

            //Update currentFloor
            _currentFloor = GetNextFloorToVisit();

            //Remove current floor from the list of floors to visit
            _listOfFloorsToVisit.RemoveAt(0);

            //Update elevator's direction
            UpdateElevatorDirection();

            //If one of passengers inside wants to get out here or this is end of the road,
            //then finalize going to next floor on the list
            if (SomePassengersWantsToGetOutOnThisFloor() || (_elevatorDirection == Direction.stop))
            {
                FinalizeGoingToNextFloorOnTheList();
                return;
            }

            //If elevator is not full, then check lamps on the floor
            if (!_isFull)
            {
                if ((_elevatorDirection == Direction.up) && _currentFloor.LampUp ||
                ((_elevatorDirection == Direction.down) && (_currentFloor.LampDown)))
                {
                    FinalizeGoingToNextFloorOnTheList();
                    return;
                }
            }

            //If elevator doesn't stop here, let it go to next floor
            GoToNextFloorOnTheList();
        }

        private void FinalizeGoingToNextFloorOnTheList()
        {
            //Reset appropriate lamp on current floor
            switch (this._elevatorDirection)
            {
                case Direction.up:
                    _currentFloor.LampUp = false;
                    break;
                case Direction.down:
                    _currentFloor.LampDown = false;
                    break;
                case Direction.stop:
                    _currentFloor.LampUp = false;
                    _currentFloor.LampDown = false;
                    break;
            }

            //Open the door
            this.OpenTheDoor();

            //Update elevator's status
            SetElevatorStatus(ElevatorStatus.WaitingForPassengersToGetInAndGetOut);

            //Inform all passengers inside
            var passengersInsideTheElevator = new List<Passenger>(_listOfPeopleInside);
            foreach (var singlePassengerInsideTheElevator in passengersInsideTheElevator)
            {
                singlePassengerInsideTheElevator.ElevatorReachedNextFloor();
                Thread.Sleep(singlePassengerInsideTheElevator.GetAnimationDelay() * 40); //to make sure all passengers will be visible when leaving the building
            }

            //Add this elevator to next floor's list
            _currentFloor.AddRemoveElevatorToTheListOfElevatorsWaitingHere(this, true);

            //Rise an event on current floor to inform passengers, who await
            _currentFloor.OnElevatorHasArrivedOrIsNoteFullAnymore(new ElevatorEventArgs(this));

            //Enable the timer            
            _elevatorTimer.Start();
        }

        public void AddNewFloorToTheList(Floor floorToBeAdded)
        {
            lock (_locker) //Method can be invoked from ElevatorManager thread (SendAnElevator()) or passenger's thread (AddNewPassengerIfPossible())
            {
                //If FloorToBeAdded is already on the list, do nothing
                if (GetListOfAllFloorsToVisit().Contains(floorToBeAdded))
                {
                    return;
                }

                //If elevator is going up
                if (_currentFloor.FloorIndex < floorToBeAdded.FloorIndex)
                {
                    for (var i = _currentFloor.FloorIndex + 1; i <= floorToBeAdded.FloorIndex; i++)
                    {
                        if (!GetListOfAllFloorsToVisit().Contains(_myBuilding.ArrayFloors[i]))
                        {
                            GetListOfAllFloorsToVisit().Add(_myBuilding.ArrayFloors[i]);
                        }
                    }
                }

                //If elevator is going down
                if (_currentFloor.FloorIndex > floorToBeAdded.FloorIndex)
                {
                    for (var i = _currentFloor.FloorIndex - 1; i >= floorToBeAdded.FloorIndex; i--)
                    {
                        if (!GetListOfAllFloorsToVisit().Contains(_myBuilding.ArrayFloors[i]))
                        {
                            GetListOfAllFloorsToVisit().Add(_myBuilding.ArrayFloors[i]);
                        }
                    }
                }

                //Update ElevatorDirection
                UpdateElevatorDirection();
            }
        }

        private bool SomePassengersWantsToGetOutOnThisFloor()
        {
            foreach (var passengerInsideThElevator in _listOfPeopleInside)
            {
                if (passengerInsideThElevator.GetTargetFloor() == this._currentFloor)
                {
                    return true;
                }
            }
            return false;
        }

        public Floor GetCurrentFloor()
        {
            return _currentFloor;
        }

        private Floor GetNextFloorToVisit()
        {
            lock (_locker) //To avoid e.g. adding new element and checking whole list at the same time
            {
                return _listOfFloorsToVisit.Count > 0 ? this._listOfFloorsToVisit[0] : null;
            }
        }

        public List<Floor> GetListOfAllFloorsToVisit()
        {
            lock (_locker) //To avoid e.g. adding new element and checking whole list at the same time
            {
                return _listOfFloorsToVisit;
            }
        }

        private void UpdateElevatorDirection()
        {
            //Lock not needed:
            //AddNewFloorToTheList method is the only reference for this method and it has its own lock         
            if (GetNextFloorToVisit() == null)
            {
                this._elevatorDirection = Direction.stop;
                return;
            }

            this._elevatorDirection = _currentFloor.FloorIndex < GetNextFloorToVisit().FloorIndex ? Direction.up : Direction.down;
        }

        public bool AddNewPassengerIfPossible(Passenger newPassenger, Floor targetFloor)
        {
            //Passengers are added synchronically. Lock not needed.

            if (_isFull || ((GetElevatorStatus() != ElevatorStatus.Idle) &&
                            (GetElevatorStatus() != ElevatorStatus.WaitingForPassengersToGetInAndGetOut)))
            {
                return false; //new passenger not added due to lack of space in the elevator      
            }

            //Reset elevator timer, so the passenger has time to get in
            this.ResetElevatorTimer();

            this._listOfPeopleInside.Add(newPassenger); //add new passenger
            this.AddNewFloorToTheList(targetFloor); //add new floor                    
            if (this._listOfPeopleInside.Count < this._maximumPeopleInside)
            {
                return true; //new passenger added successfully
            }

            this._isFull = true;
            this.SetElevatorStatus(ElevatorStatus.PreparingForJob); // to prevent other passengers attempt to get in

            return true; //new passenger added successfully
        }

        public void RemovePassenger(Passenger passengerToRemove)
        {
            lock (_locker) //Can be invoked by multiple passengers at once
            {
                this._listOfPeopleInside.Remove(passengerToRemove);
                this._isFull = false;
            }
        }

        public void ResetElevatorTimer()
        {
            lock (_locker)
            {
                this._elevatorTimer.Stop();
                this._elevatorTimer.Start();
            }
        }

        private void MoveTheElevatorGraphicDown(int destinationLevel)
        {
            for (var i = this.GetElevatorYPosition(); i <= destinationLevel; i++)
            {
                Thread.Sleep(this._elevatorAnimationDelay);
                this._elevatorPosition = new Point(GetElevatorXPosition(), i);
            }
        }

        private void MoveTheElevatorGraphicUp(int destinationLevel)
        {
            for (int i = this.GetElevatorYPosition(); i >= destinationLevel; i--)
            {
                Thread.Sleep(this._elevatorAnimationDelay);
                this._elevatorPosition = new Point(GetElevatorXPosition(), i);
            }
        }

        private void CloseTheDoor()
        {
            for (var i = 0; i < 5; i++)
            {
                switch (this._currentFrameNumber)
                {
                    case (0):
                        this._currentFrameNumber = 1;
                        Thread.Sleep(100);
                        break;
                    case (1):
                        this._currentFrameNumber = 2;
                        Thread.Sleep(100);
                        break;
                    case (2):
                        this._currentFrameNumber = 3;
                        Thread.Sleep(100);
                        break;
                    case (3):
                        this._currentFrameNumber = 4;
                        Thread.Sleep(100);
                        break;
                    case (4):
                        this._currentFrameNumber = 5;
                        Thread.Sleep(100);
                        break;
                }
            }
        }

        private void OpenTheDoor()
        {
            for (var i = 0; i < 5; i++)
            {
                switch (this._currentFrameNumber)
                {
                    case (5):
                        this._currentFrameNumber = 4;
                        Thread.Sleep(100);
                        break;
                    case (4):
                        this._currentFrameNumber = 3;
                        Thread.Sleep(100);
                        break;
                    case (3):
                        this._currentFrameNumber = 2;
                        Thread.Sleep(100);
                        break;
                    case (2):
                        this._currentFrameNumber = 1;
                        Thread.Sleep(100);
                        break;
                    case (1):
                        this._currentFrameNumber = 0;
                        Thread.Sleep(100);
                        break;
                }
            }
        }

        public int GetElevatorXPosition()
        {
            return this._elevatorPosition.X;
        }

        public int GetElevatorYPosition()
        {
            return this._elevatorPosition.Y;
        }

        public Bitmap GetCurrentFrame()
        {
            return this._elevatorFrames[_currentFrameNumber];
        }

        public ElevatorStatus GetElevatorStatus()
        {
            lock (_locker) //To avoid e.g. setting and getting status at the same time
            {
                return this._elevatorStatus;
            }
        }

        private void SetElevatorStatus(ElevatorStatus status)
        {
            lock (_locker) //To avoid e.g. setting and getting status at the same time
            {
                this._elevatorStatus = status;
            }
        }

        public Direction GetElevatorDirection()
        {
            lock (_locker) //To avoid reading during updating the elevatorDirection
            {
                return _elevatorDirection;
            }
        }

        #endregion


        #region EVENTS

        public event EventHandler PassengerEnteredTheElevator;
        public void OnPassengerEnteredTheElevator(PassengerEventArgs e)
        {
            PassengerEnteredTheElevator?.Invoke(this, e);
        }

        public event EventHandler ElevatorIsFull;
        public void OnElevatorIsFullAndHasToGoDown(EventArgs e)
        {
            ElevatorIsFull?.Invoke(this, e);
        }

        #endregion


        #region EVENT HANDLERS

        public void Elevator_PassengerEnteredTheElevator(object sender, EventArgs e)
        {
            //Restart elevator's timer
            ResetElevatorTimer();
        }

        public void Elevator_ElevatorTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (GetNextFloorToVisit() == null)
            {
                _elevatorTimer.Stop();
                SetElevatorStatus(ElevatorStatus.Idle);
            }
            else
            {
                //ThreadPool.QueueUserWorkItem(delegate { this.PrepareElevatorToGoToNextFloorOnTheList(); });                
                this.PrepareElevatorToGoToNextFloorOnTheList();
            }
        }

        #endregion

    }
}
