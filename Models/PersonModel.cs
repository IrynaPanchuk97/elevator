using LiftSimulator.Mediators;

namespace LiftSimulator.Models
{
    public class PersonModel : PersonColleague
    {
        public PersonModel(BuildingModel building, int startFloorIndex, int endFloorIndex, PersonMediator mediator) :
            base(mediator)
        {
            StartFloorIndex = startFloorIndex;
            EndFloorIndex = endFloorIndex;
            PersonStatus = PersonStatus.WaitingForAnElevator;
            Building = building;
            //building.Floors[startFloorIndex].AddPersonToWaitingList(this);
            NeedUiChanges();
        }

        public int Weight { get; set; }
        public int StartFloorIndex { get; set; }
        public int EndFloorIndex { get; set; }
        public BuildingModel Building { get; set; }
        public PersonStatus PersonStatus { get; set; }
    }
}
