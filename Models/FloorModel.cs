using LiftSimulator.Concrete;

namespace LiftSimulator.Models
{
    public class FloorModel
    {
        public int FloorNumber;
        public BuildingModel Building { get; set; }
        public FloorPersonsQuery FloorPersonsQuery { get; set; }

        public FloorModel(int floorNumber, BuildingModel building)
        {
            FloorNumber = floorNumber;
            Building = building;
            FloorPersonsQuery = new FloorPersonsQuery(this);
        }
    }
}
