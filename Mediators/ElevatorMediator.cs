using LiftSimulator.ViewModels;

namespace LiftSimulator.Mediators
{
    public class ElevatorMediator
    {
        //public ElevatorViewModel ElevatorView { get; private set; }

        public ElevatorMediator(int elevatorOrder)
        {
            CreateComponents(elevatorOrder);
        }

        private void CreateComponents(int elevatorOrder)
        {
            //ElevatorView = new ElevatorViewModel(112, elevatorOrder * 94,this);
        }

        public void SomethingChangesOnUi(ElevatorColleague colleague)
        {
        }
    }
}
