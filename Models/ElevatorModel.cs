using System.Collections.Generic;

namespace LiftSimulator.Models
{
    public class ElevatorModel
    {
        public int LiftOrder { get; set; }

        public BuildingModel Building;
        public FloorModel CurrentFloor;

        private readonly List<FloorModel> _listOfFloorsToVisit;
        private Direction _elevatorDirection;
        private ElevatorStatus _elevatorStatus;

        private readonly int _maximumPeopleInside;
        private readonly List<Passenger> _listOfPeopleInside;
        private bool _isFull;

        public ElevatorModel(BuildingModel building, FloorModel currentFloor)
        {
            CurrentFloor = currentFloor;
            Building = building;

            _listOfFloorsToVisit = new List<FloorModel>();
            _elevatorDirection = Direction.stop;
            _elevatorStatus = ElevatorStatus.Idle;

            _maximumPeopleInside = 2;
            _listOfPeopleInside = new List<Passenger>();
            _isFull = false;
        }

    }
}
