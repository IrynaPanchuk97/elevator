using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;

namespace LiftSimulator
{
    public class Passenger
    {

        private readonly object locker = new object();

        static Bitmap[] PeopleListImage = 
        {
            new Bitmap(Properties.Resources.One),
            new Bitmap(Properties.Resources.Two),
            new Bitmap(Properties.Resources.Three),
            new Bitmap(Properties.Resources.Four),
            new Bitmap(Properties.Resources.Five),
            new Bitmap(Properties.Resources.Six),
            new Bitmap(Properties.Resources.Seven),
        };
        
        private Building _building;
        private Floor _currentFloor;
        private int currentFloor;
        public Direction direct;
        private PassengerStatus status;  

        private Floor targetFloor;
        private int targetFloorIndex;

        public Point PassengerPosition;
        private Bitmap thisPassengerGraphic;
        private bool visible;
        private int passengerAnimationDelay;

        private Elevator myElevator;

        public Passenger(Building MyBuilding, Floor CurrentFloor, int TargetFloorIndex)
        {            
            this._building = MyBuilding;

            this._currentFloor = CurrentFloor;
            this.currentFloor = CurrentFloor.FloorIndex;            
            this.status = PassengerStatus.WaitingForAnElevator;

            this.targetFloor = MyBuilding.arrayFloor[TargetFloorIndex];
            this.targetFloorIndex = TargetFloorIndex;

            this.PassengerPosition = new Point();

            Random random = new Random();
            this.thisPassengerGraphic = new Bitmap(Passenger.PeopleListImage[random.Next(PeopleListImage.Length)]);

            this.visible = true;
            this.passengerAnimationDelay = 3;
            this._currentFloor.NewPassengerAppeared += new EventHandler(_currentFloor.Floor_NewPassengerAppeared);
            this._currentFloor.NewPassengerAppeared += new EventHandler(this.Passenger_NewPassengerAppeared);
            this._currentFloor.ElevatorHasArrivedOrIsNotFullAnymore += new EventHandler(this.Passenger_ElevatorHasArrivedOrIsNoteFullAnymore); 
        }

        private void FindAnElevatorOrCallForANewOne()
        {            
            UpdateElevator();
            List<Elevator> listElevatorOnFloor = _currentFloor.WaitElevator();
            foreach (Elevator elevator in listElevatorOnFloor) {
                if (ElevatorsDirectionIsNoneOrOk(elevator))
                {
                    if (elevator.AddNewPassengerIfPossible(this, this.targetFloor))
                    {
                        this.status = PassengerStatus.GettingInToTheElevator;
                        ThreadPool.QueueUserWorkItem(delegate { GetInToTheElevator(elevator); });                        
                        return;
                    }
                }
            }

            _building.ElevatorManager.PassengerNeedsAnElevator(_currentFloor, this.direct);            
        }

        private void GetInToTheElevator(Elevator ElevatorToGetIn)
        {
            ElevatorToGetIn.OnPassengerEnteredTheElevator(new PassengerEventArgs(this));
            this._currentFloor.ElevatorHasArrivedOrIsNotFullAnymore -= this.Passenger_ElevatorHasArrivedOrIsNoteFullAnymore;
            this.MovePassengersGraphicHorizontally(ElevatorToGetIn.GetElevatorXPosition());
            this.visible = false;
            this.myElevator = ElevatorToGetIn;
        }

        public void ElevatorReachedNextFloor()
        {
            if (this.myElevator.GetCurrentFloor() == this.targetFloor)
            {
                this.status = PassengerStatus.LeavingTheBuilding;                
                ThreadPool.QueueUserWorkItem(delegate { GetOutOfTheElevator(this.myElevator); });
            }
        }

        private void GetOutOfTheElevator(Elevator ElevatorWhichArrived)
        {
            ElevatorWhichArrived.RemovePassenger(this);
            this.LeaveTheBuilding();
        }

        private void UpdateElevator()
        {
            if (currentFloor < targetFloorIndex) this.direct = Direction.up;
            else  this.direct = Direction.down;
        }

        private bool ElevatorsDirectionIsNoneOrOk(Elevator ElevatorOnMyFloor)
        {
            if (ElevatorOnMyFloor.GetElevatorDirection() == this.direct) return true;
            else if (ElevatorOnMyFloor.GetElevatorDirection() == Direction.stop) return true;
            return false;
        }

        private void LeaveTheBuilding()
        {
            this.PassengerPosition = new Point(PassengerPosition.X,  myElevator.GetElevatorYPosition());
            this.FlipPassengerGraphicHorizontally();         
            this.visible = true;
            this.MovePassengersGraphicHorizontally(_building.ExitLocation);
            this.visible = false;
            _building.listeople.Remove(this);
        }

        private void MovePassengersGraphicHorizontally (int DestinationPosition)
        {
            if (this.PassengerPosition.X > DestinationPosition) {
                for (int i = this.PassengerPosition.X; i > DestinationPosition; i--){
                    Thread.Sleep(this.passengerAnimationDelay);                    
                    this.PassengerPosition = new Point(i, this.PassengerPosition.Y);                    
                }
            }
            else {
                for (int i = this.PassengerPosition.X; i < DestinationPosition; i++)
                {
                    Thread.Sleep(this.passengerAnimationDelay);
                    this.PassengerPosition = new Point(i, this.PassengerPosition.Y);
                }
            }
        }

        private void FlipPassengerGraphicHorizontally()=> this.thisPassengerGraphic.RotateFlip(RotateFlipType.Rotate180FlipY);            
        public Floor GetTargetFloor()=>this.targetFloor;
        public bool GetPassengerVisibility() => this.visible;
        public int GetAnimationDelay()=>this.passengerAnimationDelay;
        public Bitmap GetCurrentFrame()=> this.thisPassengerGraphic;

        public void Passenger_NewPassengerAppeared(object sender, EventArgs e){
            this._currentFloor.NewPassengerAppeared -= this.Passenger_NewPassengerAppeared;
            FindAnElevatorOrCallForANewOne();            
        }

        public void Passenger_ElevatorHasArrivedOrIsNoteFullAnymore(object sender, EventArgs e)
        {            
            lock (locker){
                Elevator ElevatorWhichRisedAnEvent = ((ElevatorEventArgs)e).ElevatorWhichRisedAnEvent;
                if (this.status == PassengerStatus.GettingInToTheElevator)return;
                if (this.status == PassengerStatus.WaitingForAnElevator)
                {
                    if ((ElevatorsDirectionIsNoneOrOk(ElevatorWhichRisedAnEvent) && (ElevatorWhichRisedAnEvent.AddNewPassengerIfPossible(this, targetFloor))))
                    {
                        status = PassengerStatus.GettingInToTheElevator;
                        ThreadPool.QueueUserWorkItem(delegate { GetInToTheElevator(ElevatorWhichRisedAnEvent); });
                    }
                    else   FindAnElevatorOrCallForANewOne();
                    
                }                 
            }    
        }
    }
}
