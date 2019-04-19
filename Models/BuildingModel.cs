using System.Collections.Generic;
using LiftSimulator.Mediators;

namespace LiftSimulator.Models
{
    public class BuildingModel : BuildingColleague
    {
        public BuildingModel(BuildingMediator mediator) : base(mediator)
        {
            InitializeFloors(mediator);
            InitializeElevators(mediator);
            NeedUiChanges();
        }

        public List<FloorModel> Floors { get; set; }
        public List<ElevatorModel> Elevators { get; set; }

        private void InitializeFloors(BuildingMediator mediator)
        {
            for (var i = 0; i < 4; ++i)
            {
                Floors.Add(new FloorModel(i, this, new FloorMediator()));
            }
        }

        private void InitializeElevators(BuildingMediator mediator)
        {
            for (var i = 0; i < 3; ++i)
            {
                Elevators.Add(new ElevatorModel(this, Floors[0], new ElevatorMediator(i)));
            }
        }
    }
}
