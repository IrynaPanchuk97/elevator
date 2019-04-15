using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace LiftSimulator
{
    public class Floor
    {

        private readonly object locker = new object();

        private Building myBuilding;

        private int maximumAmmountOfPeopleInTheQueue; // depends on graphic

        private Passenger[] arrayOfPeopleWaitingForElevator;

        private List<Elevator> listOfElevatorsWaitingHere;

        private int floorIndex;
        public int FloorIndex
        {
            get { return floorIndex; }
            private set { }
        }

        private int floorLevel; 
        private int beginOfTheQueue; 
        private int widthOfSlotForSinglePassenger; 

        public bool LampUp;
        public bool LampDown; 



        public Floor(Building myBuilding, int floorNumber, int floorLevel)
        {
            this.myBuilding = myBuilding;

            maximumAmmountOfPeopleInTheQueue = 8; //only 8 passengers at once can be visible in current layout
            this.arrayOfPeopleWaitingForElevator = new Passenger[maximumAmmountOfPeopleInTheQueue];
            this.floorIndex = floorNumber;

            listOfElevatorsWaitingHere = new List<Elevator>();

            //Initialize variables, which depend on graphics:
            this.floorLevel = floorLevel;
            beginOfTheQueue = 367;
            widthOfSlotForSinglePassenger = 46;

            //Turn off both lamps
            LampUp = false;
            LampDown = false;
        }

        private int? FindFirstFreeSlotInQueue()
        {
            for (int i = 0; i < maximumAmmountOfPeopleInTheQueue; i++)
            {
                if (arrayOfPeopleWaitingForElevator[i] == null)
                {
                    return i;
                }
            }

            return null;
        }

        private void AddRemoveNewPassengerToTheQueue(Passenger PassengerToAddOrRemvove, bool AddFlag)
        {
            if (AddFlag) //Add passenger
            {
                int? FirstFreeSlotInQueue = FindFirstFreeSlotInQueue();
                if (FirstFreeSlotInQueue != null)
                {
                    this.arrayOfPeopleWaitingForElevator[(int)FirstFreeSlotInQueue] = PassengerToAddOrRemvove;

                    int NewPassengerVerticalPosition = this.beginOfTheQueue + (this.widthOfSlotForSinglePassenger * (int)FirstFreeSlotInQueue);
                    PassengerToAddOrRemvove.PassengerPosition = new Point(NewPassengerVerticalPosition, GetFloorLevelInPixels());

                    myBuilding.listeople.Add(PassengerToAddOrRemvove);
                }
            }
            else 
            {
                int PassengerToRemoveIndex = Array.IndexOf<Passenger>(GetArrayOfPeopleWaitingForElevator(), PassengerToAddOrRemvove);
                this.GetArrayOfPeopleWaitingForElevator()[PassengerToRemoveIndex] = null;
            }            
        }

        public void AddRemoveElevatorToTheListOfElevatorsWaitingHere(Elevator ElevatorToAddOrRemove, bool AddFlag)
        {
            lock (locker) 
            {
                if (AddFlag) 
                {
                    listOfElevatorsWaitingHere.Add(ElevatorToAddOrRemove);

                    ElevatorToAddOrRemove.PassengerEnteredTheElevator += new EventHandler(this.Floor_PassengerEnteredTheElevator);
                }
                else
                {
                    listOfElevatorsWaitingHere.Remove(ElevatorToAddOrRemove);
                    ElevatorToAddOrRemove.PassengerEnteredTheElevator -= this.Floor_PassengerEnteredTheElevator;
                }
            }
        }

        public int GetMaximumAmmountOfPeopleInTheQueue()
        {
            return maximumAmmountOfPeopleInTheQueue;
        }

        public int GetCurrentAmmountOfPeopleInTheQueue()
        {
            lock (locker) 
            {
                int CurrentAmmountOfPeopleInTheQueue = 0;
                for (int i = 0; i < maximumAmmountOfPeopleInTheQueue; i++)
                {
                    if (this.arrayOfPeopleWaitingForElevator[i] != null)
                    {
                        CurrentAmmountOfPeopleInTheQueue++;
                    }
                }
                return CurrentAmmountOfPeopleInTheQueue;
            }
        }

        public Passenger[] GetArrayOfPeopleWaitingForElevator()
        {
            return arrayOfPeopleWaitingForElevator;
        }

        public List<Elevator> WaitElevator()
        {
            lock (locker)
            {
                return this.listOfElevatorsWaitingHere;
            }
        }

        public int GetFloorLevelInPixels()
        {
            return this.floorLevel;
        }


        public event EventHandler NewPassengerAppeared;
        public void OnNewPassengerAppeared(EventArgs e)=> NewPassengerAppeared?.Invoke(this, e);

        public event EventHandler ElevatorHasArrivedOrIsNotFullAnymore;
        public void OnElevatorHasArrivedOrIsNoteFullAnymore(ElevatorEventArgs e)
        {
            ElevatorHasArrivedOrIsNotFullAnymore?.Invoke(this, e);
        }
        public void Floor_NewPassengerAppeared(object sender, EventArgs e)
        {
            lock (locker)
            {
                this.NewPassengerAppeared -= this.Floor_NewPassengerAppeared;

                Passenger NewPassenger = ((PassengerEventArgs)e).PassengerWhoRisedAnEvent;

                AddRemoveNewPassengerToTheQueue(NewPassenger, true);
            }
        }

        public void Floor_PassengerEnteredTheElevator(object sender, EventArgs e)
        {
            lock (locker)
            {
                Passenger PassengerWhoEnteredOrLeftTheElevator = ((PassengerEventArgs)e).PassengerWhoRisedAnEvent;
                AddRemoveNewPassengerToTheQueue(PassengerWhoEnteredOrLeftTheElevator, false);
            }
        }

    }
}
