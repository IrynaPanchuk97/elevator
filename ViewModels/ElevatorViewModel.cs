using System.Drawing;

namespace LiftSimulator.ViewModels
{
    public class ElevatorViewModel
    {
        private readonly Bitmap[] _elevatorFrames;
        private int _currentFrameNumber;
        private readonly int _elevatorAnimationDelay;
        private readonly System.Timers.Timer _elevatorTimer;

        public Point ElevatorPosition { get; set; }

        public ElevatorViewModel(int elevatorXPosition, int elevatorYPosition)
        {
            ElevatorPosition = new Point(elevatorXPosition, elevatorYPosition);
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
