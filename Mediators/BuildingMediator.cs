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

        public void SomethingChangesOnUi(BuildingColleague colleague)
        {
            if (colleague is BuildingModel building)
            {

            }
        }
    }
}
