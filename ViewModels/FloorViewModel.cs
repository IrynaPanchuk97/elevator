using LiftSimulator.Mediators;

namespace LiftSimulator.ViewModels
{
    public class FloorViewModel : FloorColleague
    {
        public int FloorLevel { get; set; }

        private readonly int _beginOfTheQueue;

        private readonly int _widthOfSlotForSinglePassenger;   

        public FloorViewModel(FloorMediator mediator) : base(mediator)
        {
            _beginOfTheQueue = 367;
            _widthOfSlotForSinglePassenger = 46;
        }
    }
}
