using LiftSimulator.Concrete;
using LiftSimulator.Mediators;

namespace LiftSimulator.Models
{
    public class FloorModel : FloorColleague
    {
        public int FloorNumber;
        public BuildingModel Building { get; set; }
        public FloorPersonsQuery FloorPersonsQuery { get; set; }

        public FloorModel(int floorNumber, BuildingModel building, FloorMediator mediator) : base(mediator)
        {
            FloorNumber = floorNumber;
            Building = building;
            FloorPersonsQuery = new FloorPersonsQuery();
        }

    }
}
