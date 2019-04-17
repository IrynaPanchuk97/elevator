using System;
using System.Collections.Generic;
using System.Drawing;

namespace LiftSimulator
{
    public class Floor
    {
        #region FIELDS

        private readonly object _locker = new object();

        private readonly Building _myBuilding;

        private readonly int _maximumAmmountOfPeopleInTheQueue; // depends on graphic

        private readonly Passenger[] _arrayOfPeopleWaitingForElevator;

        private readonly List<Elevator> _listOfElevatorsWaitingHere;

        public int FloorIndex { get; }

        private readonly int _floorLevel; //determines (in pixels) where passengers should stand; depends on building graphic

        private readonly int _beginOfTheQueue; //determines, where queue of paasengers begins; depends on building graphic

        private readonly int _widthOfSlotForSinglePassenger; //ammount of pixels reserved for single passenger; depends on passenger graphic        

        public bool LampUp; //indicates, that at least one of passengers wants to up
        public bool LampDown; //indicates, that at least one of passengers wants to down

        #endregion


        #region METHODS

        public Floor(Building myBuilding, int floorNumber, int floorLevel)
        {
            this._myBuilding = myBuilding;

            _maximumAmmountOfPeopleInTheQueue = 8; //only 8 passengers at once can be visible in current layout
            this._arrayOfPeopleWaitingForElevator = new Passenger[_maximumAmmountOfPeopleInTheQueue];
            this.FloorIndex = floorNumber;

            _listOfElevatorsWaitingHere = new List<Elevator>();

            //Initialize variables, which depend on graphics:
            this._floorLevel = floorLevel;
            _beginOfTheQueue = 367;
            _widthOfSlotForSinglePassenger = 46;

            //Turn off both lamps
            LampUp = false;
            LampDown = false;
        }

        private int? FindFirstFreeSlotInQueue()
        {
            //Lock not needed. Only one reference, already locked.
            for (int i = 0; i < _maximumAmmountOfPeopleInTheQueue; i++)
            {
                if (_arrayOfPeopleWaitingForElevator[i] == null)
                {
                    return i;
                }
            }

            return null;
        }

        private void AddRemoveNewPassengerToTheQueue(Passenger passengerToAddOrRemvove, bool addFlag)
        {
            //Lock not needed. Only two references (from this), both already locked                        
            if (addFlag) //Add passenger
            {
                var firstFreeSlotInQueue = FindFirstFreeSlotInQueue(); //Make sure there is a space to add new passenger
                if (firstFreeSlotInQueue == null)
                {
                    return;
                }
                //Add passenger object to an array                    
                this._arrayOfPeopleWaitingForElevator[(int)firstFreeSlotInQueue] = passengerToAddOrRemvove;

                //Add passenger control to the UI
                int newPassengerVerticalPosition = this._beginOfTheQueue + (this._widthOfSlotForSinglePassenger * (int)firstFreeSlotInQueue);
                passengerToAddOrRemvove.PassengerPosition = new Point(newPassengerVerticalPosition, GetFloorLevelInPixels());

                //Add passenger to Building's list
                _myBuilding.ListOfAllPeopleWhoNeedAnimation.Add(passengerToAddOrRemvove);
            }
            else //Remove passenger
            {
                int passengerToRemoveIndex = Array.IndexOf(GetArrayOfPeopleWaitingForElevator(), passengerToAddOrRemvove);
                this.GetArrayOfPeopleWaitingForElevator()[passengerToRemoveIndex] = null;
            }            
        }

        public void AddRemoveElevatorToTheListOfElevatorsWaitingHere(Elevator ElevatorToAddOrRemove, bool AddFlag)
        {
            lock (_locker) //Few elevators can try to add/remove themselfs at the same time
            {
                if (AddFlag) //Add elevator
                {
                    //Add elevator to the list
                    _listOfElevatorsWaitingHere.Add(ElevatorToAddOrRemove);

                    //Subscribe to an event, rised when passenger entered the elevator
                    ElevatorToAddOrRemove.PassengerEnteredTheElevator += new EventHandler(this.Floor_PassengerEnteredTheElevator);
                }
                else //Remove elevator
                {
                    //Remove elevator from the list
                    _listOfElevatorsWaitingHere.Remove(ElevatorToAddOrRemove);

                    //Unsubscribe from an event, rised when passenger entered the elevator
                    ElevatorToAddOrRemove.PassengerEnteredTheElevator -= this.Floor_PassengerEnteredTheElevator;
                }
            }
        }

        public int GetMaximumAmmountOfPeopleInTheQueue()
        {
            return _maximumAmmountOfPeopleInTheQueue;
        }

        public int GetCurrentAmmountOfPeopleInTheQueue()
        {
            lock (_locker) //The same lock is on add/remove passenger to the queue
            {
                int CurrentAmmountOfPeopleInTheQueue = 0;
                for (int i = 0; i < _maximumAmmountOfPeopleInTheQueue; i++)
                {
                    if (this._arrayOfPeopleWaitingForElevator[i] != null)
                    {
                        CurrentAmmountOfPeopleInTheQueue++;
                    }
                }
                return CurrentAmmountOfPeopleInTheQueue;
            }
        }

        public Passenger[] GetArrayOfPeopleWaitingForElevator()
        {
            return _arrayOfPeopleWaitingForElevator;
        }

        public List<Elevator> GetListOfElevatorsWaitingHere()
        {
            //Lock not needed. Method for passengers only.
            lock (_locker)
            {
                return this._listOfElevatorsWaitingHere;
            }
        }

        public int GetFloorLevelInPixels()
        {
            return this._floorLevel;
        }

        #endregion


        #region EVENTS

        public event EventHandler NewPassengerAppeared;
        public void OnNewPassengerAppeared(EventArgs e)
        {
            EventHandler newPassengerAppeared = NewPassengerAppeared;
            if (newPassengerAppeared != null)
            {
                newPassengerAppeared(this, e);
            }
        }

        public event EventHandler ElevatorHasArrivedOrIsNotFullAnymore;
        public void OnElevatorHasArrivedOrIsNoteFullAnymore(ElevatorEventArgs e)
        {
            EventHandler elevatorHasArrivedOrIsNoteFullAnymore = ElevatorHasArrivedOrIsNotFullAnymore;
            if (elevatorHasArrivedOrIsNoteFullAnymore != null)
            {
                elevatorHasArrivedOrIsNoteFullAnymore(this, e);
            }
        }

        #endregion


        #region EVENT HADNLERS

        public void Floor_NewPassengerAppeared(object sender, EventArgs e)
        {
            lock (_locker)
            {
                //Unsubscribe from this event (not needed anymore)
                this.NewPassengerAppeared -= this.Floor_NewPassengerAppeared;

                Passenger NewPassenger = ((PassengerEventArgs)e).PassengerWhoRisedAnEvent;

                AddRemoveNewPassengerToTheQueue(NewPassenger, true);
            }
        }

        public void Floor_PassengerEnteredTheElevator(object sender, EventArgs e)
        {
            lock (_locker)
            {
                Passenger PassengerWhoEnteredOrLeftTheElevator = ((PassengerEventArgs)e).PassengerWhoRisedAnEvent;

                //Remove passenger from queue                
                AddRemoveNewPassengerToTheQueue(PassengerWhoEnteredOrLeftTheElevator, false);
            }
        }

        #endregion
    }
}
