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


        public void AddPersonToListOfWhoNeedAnimation(BuildingColleague colleague, int personPositionInQueue, int floorIndex)
        {
            if (colleague is BuildingModel)
            {
                BuildingView.AddPersonView(personPositionInQueue, floorIndex);
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
