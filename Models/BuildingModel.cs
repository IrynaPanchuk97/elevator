using System.Collections.Generic;
using LiftSimulator.ConcreteServices;
using LiftSimulator.Mediators;

namespace LiftSimulator.Models
{
    public class BuildingModel : BuildingColleague
    {
        public List<FloorModel> Floors { get; set; }
        public List<ElevatorModel> Elevators { get; set; }

        public BuildingModel(BuildingMediator mediator) : base(mediator)
        {
            InitializeFloors();
            InitializeElevators();

            var personGenerator = new PersonGenerator(this);
            personGenerator.GeneratePerson();
        }

        private void InitializeFloors()
        {
            Floors = new List<FloorModel>();
            for (var i = 0; i < 4; ++i)
            {
                Floors.Add(new FloorModel(i, this));
            }
        }

        private void InitializeElevators()
        {
            Elevators = new List<ElevatorModel>();
            for (var i = 0; i < 3; ++i)
            {
                Elevators.Add(new ElevatorModel(this, Floors[0]));
            }
        }
    }
}
