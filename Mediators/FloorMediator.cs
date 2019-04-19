using LiftSimulator.Models;
using LiftSimulator.ViewModels;

namespace LiftSimulator.Mediators
{
    public class FloorMediator
    {
        public FloorViewModel FloorView { get; private set; }

        public FloorMediator()
        {
            CreateComponents();
        }

        private void CreateComponents()
        {
            FloorView = new FloorViewModel(this);
        }

        public void SomethingChangesOnUi(FloorColleague colleague)
        {
        }
    }
}
