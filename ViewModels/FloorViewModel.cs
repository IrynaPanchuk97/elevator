namespace LiftSimulator.ViewModels
{
    public class FloorViewModel
    {
        public int FloorLevel { get; set; }

        private readonly int _beginOfTheQueue;

        private readonly int _widthOfSlotForSinglePassenger;   

        public FloorViewModel()
        {
            _beginOfTheQueue = 367;
            _widthOfSlotForSinglePassenger = 46;
        }
    }
}
