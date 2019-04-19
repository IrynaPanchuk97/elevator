namespace LiftSimulator.Models
{
    public class PersonModel
    {
        public int Weight { get; set; }
        public int StartFloorIndex { get; set; }
        public int EndFloorIndex { get; set; }
        public BuildingModel Building { get; set; }
        public PersonStatus PersonStatus { get; set; }

        public PersonModel(BuildingModel building, int startFloorIndex, int endFloorIndex)
        {
            StartFloorIndex = startFloorIndex;
            EndFloorIndex = endFloorIndex;
            PersonStatus = PersonStatus.WaitingForAnElevator;
            Building = building;
            building.Floors[startFloorIndex].FloorPersonsQuery.AddPersonToTheQueue(this);
            //building.Floors[startFloorIndex].AddPersonToWaitingList(this);
        }
    }
}
