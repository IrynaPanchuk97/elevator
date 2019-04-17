using System.Collections.Generic;

namespace LiftSimulator
{
    public class Building
    {
        public ElevatorManager ElevatorManager;
        public List<Passenger> ListOfAllPeopleWhoNeedAnimation;
        public Floor[] ArrayFloors { get; }
        public Elevator[] ArrayOfAllElevators { get; }
        public int ExitLocation { get; }

        public Building()
        {
            ExitLocation = 677;
            ArrayFloors = new Floor[4];
            ArrayFloors[0] = new Floor(this, 0, 373);
            ArrayFloors[1] = new Floor(this, 1, 254);
            ArrayFloors[2] = new Floor(this, 2, 144);
            ArrayFloors[3] = new Floor(this, 3, 32);

            ArrayOfAllElevators = new Elevator[3];
            ArrayOfAllElevators[0] = new Elevator(this, 124, ArrayFloors[0], ElevatorManager);
            ArrayOfAllElevators[1] = new Elevator(this, 210, ArrayFloors[0], ElevatorManager);
            ArrayOfAllElevators[2] = new Elevator(this, 295, ArrayFloors[0], ElevatorManager);

            ListOfAllPeopleWhoNeedAnimation = new List<Passenger>();
            ElevatorManager = new ElevatorManager(ArrayOfAllElevators, ArrayFloors);
        }

    }
}
