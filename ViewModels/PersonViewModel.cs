using System;
using System.Drawing;
using System.Threading;
using LiftSimulator.Mediators;

namespace LiftSimulator.ViewModels
{
    public class PersonViewModel : PersonColleague
    {
        public Point PersonPosition;
        public Bitmap PersonGraphic { get; set; }

        private bool _visible;
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

        public PersonViewModel(PersonMediator mediator) : base(mediator)
        {
            PersonPosition = new Point();
            var random = new Random();
            _visible = true;
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
