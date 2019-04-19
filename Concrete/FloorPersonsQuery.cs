using System;
using LiftSimulator.Models;

namespace LiftSimulator.Concrete
{
    public class FloorPersonsQuery
    {
        public FloorModel Floor { get; set; }
        public PersonModel[] ArrayOfPeopleWaitingForElevator { get; set; }
        public int MaximumAmountOfPeopleInTheQueue { get; set; }

        public FloorPersonsQuery(FloorModel floor)
        {
            Floor = floor;
            MaximumAmountOfPeopleInTheQueue = 8;
            ArrayOfPeopleWaitingForElevator = new PersonModel[MaximumAmountOfPeopleInTheQueue];
        }

        public bool AddPersonToTheQueue(PersonModel personToAdd)
        {
            var firstFreeSlotInQueue = FindFirstFreeSlotInQueue();
            if (firstFreeSlotInQueue == null)
            {
                return false;
            }

            ArrayOfPeopleWaitingForElevator[firstFreeSlotInQueue.Value] = personToAdd;
            Floor.Building.AddPersonToListOfWhoNeedAnimation(firstFreeSlotInQueue.Value, Floor.FloorNumber);
            return true;
        }

        public bool RemovePersonFromTheQueue(PersonModel personToRemove)
        {
            var passengerToRemoveIndex = Array.IndexOf(ArrayOfPeopleWaitingForElevator, personToRemove);
            ArrayOfPeopleWaitingForElevator[passengerToRemoveIndex] = null;
            return true;
        }

        //public void AddRemoveElevatorToTheListOfElevatorsWaitingHere(Elevator ElevatorToAddOrRemove, bool AddFlag)
        //{
        //    lock (_locker) //Few elevators can try to add/remove themselfs at the same time
        //    {
        //        if (AddFlag) //Add elevator
        //        {
        //            //Add elevator to the list
        //            _listOfElevatorsWaitingHere.Add(ElevatorToAddOrRemove);

        //            //Subscribe to an event, rised when passenger entered the elevator
        //            ElevatorToAddOrRemove.PassengerEnteredTheElevator += new EventHandler(this.Floor_PassengerEnteredTheElevator);
        //        }
        //        else //Remove elevator
        //        {
        //            //Remove elevator from the list
        //            _listOfElevatorsWaitingHere.Remove(ElevatorToAddOrRemove);

        //            //Unsubscribe from an event, rised when passenger entered the elevator
        //            ElevatorToAddOrRemove.PassengerEnteredTheElevator -= this.Floor_PassengerEnteredTheElevator;
        //        }
        //    }
        //}

        private int? FindFirstFreeSlotInQueue()
        {
            //Lock not needed. Only one reference, already locked.
            for (var i = 0; i < MaximumAmountOfPeopleInTheQueue; i++)
            {
                if (ArrayOfPeopleWaitingForElevator[i] == null)
                {
                    return i;
                }
            }

            return null;
        }

    }
}
