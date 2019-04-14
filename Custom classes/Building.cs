using LiftSimulator.Custom_classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LiftSimulator
{
    public class Building
    {
        private Floor[] _arrayFloors;
        private Elevator[] _arrayElevators;
        public ElevatorManager ElevatorManager;
        public List<Passenger> ListOfAllPeopleWhoNeedAnimation;
        public Floor[] arrayFloors
        {
            get { return _arrayFloors; }
            private set { }
        }

        public Elevator[] ArrayOfAllElevators
        {
            get { return _arrayElevators; }
            private set { }
        }

        private int exitLocation;
        public int ExitLocation
        {
            get { return exitLocation; }
            private set { }
        }


        public Building()
        {
            exitLocation = 677;
            _arrayFloors = new Floor[4];
            _arrayFloors[0] = new Floor(this, 0, 373);
            _arrayFloors[1] = new Floor(this, 1, 254);
            _arrayFloors[2] = new Floor(this, 2, 144);
            _arrayFloors[3] = new Floor(this, 3, 32);

            _arrayElevators = new Elevator[3];
            _arrayElevators[0] = new Elevator(this, 124, _arrayFloors[0], ElevatorManager );
            _arrayElevators[1] = new Elevator(this, 210, _arrayFloors[0], ElevatorManager);
            _arrayElevators[2] = new Elevator(this, 295, _arrayFloors[0], ElevatorManager);

            ListOfAllPeopleWhoNeedAnimation = new List<Passenger>();
            ElevatorManager = new ElevatorManager(ArrayOfAllElevators, arrayFloors);
        }

    }
}
