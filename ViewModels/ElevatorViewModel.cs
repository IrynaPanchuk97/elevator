using System.Drawing;
using LiftSimulator.Mediators;

namespace LiftSimulator.ViewModels
{
    public class ElevatorViewModel : ElevatorColleague
    {
        private Point _elevatorPosition;
        private readonly Bitmap[] _elevatorFrames;
        private int _currentFrameNumber;
        private readonly int _elevatorAnimationDelay;
        private readonly System.Timers.Timer _elevatorTimer;

        public ElevatorViewModel(int elevatorXPosition, int elevatorYPosition, ElevatorMediator mediator) :
            base(mediator)
        {
            _elevatorPosition = new Point(elevatorXPosition, elevatorYPosition);
            _currentFrameNumber = 0;
            _elevatorFrames = new[]
            {
                Properties.Resources.LiftDoors_Open,
                Properties.Resources.LiftDoors_4,
                Properties.Resources.LiftDoors_3,
                Properties.Resources.LiftDoors_2,
                Properties.Resources.LiftDoors_1,
                Properties.Resources.LiftDoors_Closed
            };
        }

        public Bitmap GetCurrentFrame()
        {
            return _elevatorFrames[_currentFrameNumber];
        }
    }
}
