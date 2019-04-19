using System;
using System.Collections.Generic;
using System.Timers;

namespace LiftSimulator
{
    public class Building
    {
        public ElevatorManager ElevatorManager;
        public List<Passenger> ListOfAllPeopleWhoNeedAnimation;
        public Floor[] ArrayFloors { get; }
        public Elevator[] ArrayOfAllElevators { get; }
        public int ExitLocation { get; }

        private static Timer aTimer;

        public void CreatePassenger(object source, ElapsedEventArgs e)
        {
            var randStartIndex = new Random();
            var randEndIndex = new Random();
            var startIndex = randStartIndex.Next(0, 4);
            var endIndex = randEndIndex.Next(0, 4);

            while (endIndex == startIndex)
            {
                endIndex = randEndIndex.Next(0, 4);
            }


            var pas = new Passenger(this, ArrayFloors[startIndex], endIndex);
            ArrayFloors[startIndex].OnNewPassengerAppeared(new PassengerEventArgs(pas));
        }


        public Building()
        {
            aTimer = new Timer(10000);

            // Hook up the Elapsed event for the timer.
            aTimer.Elapsed += CreatePassenger;

            // Set the Interval to 2 seconds (2000 milliseconds).
            aTimer.Interval = 5000;
            aTimer.Enabled = true;

            ExitLocation = 677;
            ArrayFloors = new Floor[4];
            ArrayFloors[0] = new Floor(this, 0, 373);
            ArrayFloors[1] = new Floor(this, 1, 254);
            ArrayFloors[2] = new Floor(this, 2, 144);
            ArrayFloors[3] = new Floor(this, 3, 32);

            ArrayOfAllElevators = new Elevator[3];
            ArrayOfAllElevators[0] = new Elevator(this, 124, ArrayFloors[0]);
            ArrayOfAllElevators[1] = new Elevator(this, 210, ArrayFloors[0]);
            ArrayOfAllElevators[2] = new Elevator(this, 295, ArrayFloors[0]);

            ListOfAllPeopleWhoNeedAnimation = new List<Passenger>();
            ElevatorManager = new ElevatorManager(ArrayOfAllElevators, ArrayFloors);
        }

    }
}
