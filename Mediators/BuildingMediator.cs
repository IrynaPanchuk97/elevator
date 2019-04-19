using LiftSimulator.Models;
using LiftSimulator.ViewModels;

namespace LiftSimulator.Mediators
{
    public class BuildingMediator
    {
        public BuildingViewModel BuildingView { get; private set; }
        public BuildingModel BuildingModel { get; private set; }

        public BuildingMediator()
        {
            CreateComponents();
        }

        private void CreateComponents()
        {
            BuildingView = new BuildingViewModel(this);
            BuildingModel = new BuildingModel(this);
        }


        public void AddPersonToListOfWhoNeedAnimation(BuildingColleague colleague)
        {
            if (colleague is BuildingModel building)
            {
                //int newPassengerVerticalPosition = this._beginOfTheQueue + (this._widthOfSlotForSinglePassenger * (int)firstFreeSlotInQueue);
                //passengerToAddOrRemvove.PassengerPosition = new Point(newPassengerVerticalPosition, GetFloorLevelInPixels());

                //if (building.)
            }
        }

        public void SomethingChangesOnUi(BuildingColleague colleague)
        {
            if (colleague is BuildingModel building)
            {

            }
        }
    }
}
