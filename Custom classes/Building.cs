using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;

namespace LiftSimulator
{
    public class Building
    {
        private readonly Floor[]  _arrayFloors;
        private readonly Elevator[] _arrayElevators;
        public ElevatorManager ElevatorManager;
        public List<Passenger> listeople;
        private readonly int exitLocation;
        private static System.Timers.Timer aTimer;

        public Floor[] arrayFloor
        {
            get=> _arrayFloors; 
            private set { }
        }

        public Elevator[] arrayElevator{
            get => _arrayElevators; 
            private set { }
        }
        public int ExitLocation{
            get => exitLocation; 
            private set { }
        }

        public void createPassenger(object source, ElapsedEventArgs e)
        {
            var randStartIndex = new Random();
            var randEndIndex = new Random();
            var startIndex = randStartIndex.Next(0, 4);
            var endIndex = randEndIndex.Next(0, 4);

            while (endIndex == startIndex)
            {
                endIndex = randEndIndex.Next(0, 4);
            }


            var pas = new Passenger(this, this.arrayFloor[startIndex], endIndex);
            this.arrayFloor[startIndex].OnNewPassengerAppeared(new PassengerEventArgs(pas));
        }


        public Building()
        {
            aTimer = new System.Timers.Timer(10000);

            // Hook up the Elapsed event for the timer.
            aTimer.Elapsed += new ElapsedEventHandler(createPassenger);

            // Set the Interval to 2 seconds (2000 milliseconds).
            aTimer.Interval = 5000;
            aTimer.Enabled = true;


            exitLocation = 677;
            _arrayFloors = new Floor[4];
            _arrayFloors[0] = new Floor(this, 0, 373);
            _arrayFloors[1] = new Floor(this, 1, 254);
            _arrayFloors[2] = new Floor(this, 2, 144);
            _arrayFloors[3] = new Floor(this, 3, 32);

            _arrayElevators = new Elevator[3];
            _arrayElevators[0] = new Elevator(this, 124, _arrayFloors[0]);
            _arrayElevators[1] = new Elevator(this, 210, _arrayFloors[0]);
            _arrayElevators[2] = new Elevator(this, 295, _arrayFloors[0]);


            listeople = new List<Passenger>();
            ElevatorManager = new ElevatorManager(arrayElevator, arrayFloor);
        }

    }
}
