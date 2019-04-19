using System;
using System.Drawing;
using System.Threading;

namespace LiftSimulator.ViewModels
{
    public class PersonViewModel
    {
        public Point PersonPosition;
        public Bitmap PersonGraphic { get; set; }
        public bool Visible { get; set; }

        private readonly int _passengerAnimationDelay;

        static readonly Bitmap[] ArrayOfAllPersonGraphics =
        {
            new Bitmap(Properties.Resources.One),
            new Bitmap(Properties.Resources.Two),
            new Bitmap(Properties.Resources.Three),
            new Bitmap(Properties.Resources.Four),
            new Bitmap(Properties.Resources.Five),
            new Bitmap(Properties.Resources.Six),
            new Bitmap(Properties.Resources.Seven)
        };

        public PersonViewModel(int personPositionInQueue, int floorIndex)
        {
            var beginOfTheQueue = 367;
            var widthOfSlotForSinglePassenger = 46;
            PersonPosition = new Point(beginOfTheQueue + widthOfSlotForSinglePassenger * personPositionInQueue, 32+ 112*floorIndex );
            Visible = true;
            var random = new Random();
            _passengerAnimationDelay = 8;
            PersonGraphic = new Bitmap(ArrayOfAllPersonGraphics[random.Next(ArrayOfAllPersonGraphics.Length)]);
        }

        public void MovePassengersGraphicHorizontally(int destinationPosition)
        {
            if (PersonPosition.X > destinationPosition) //go left
            {
                for (var i = PersonPosition.X; i > destinationPosition; i--)
                {
                    Thread.Sleep(_passengerAnimationDelay);
                    PersonPosition = new Point(i, PersonPosition.Y);
                }
            }
            else //go right
            {
                for (var i = PersonPosition.X; i < destinationPosition; i++)
                {
                    Thread.Sleep(_passengerAnimationDelay);
                    PersonPosition = new Point(i, PersonPosition.Y);
                }
            }
        }

        private void FlipPassengerGraphicHorizontally()
        {
            PersonGraphic.RotateFlip(RotateFlipType.Rotate180FlipY);
        }
    }
}
