using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LiftSimulator
{
    public partial class Form1 : Form
    {

        public Building MyBuilding;
        public Form1()
        {
            InitializeComponent();
            MyBuilding = new Building();
        }

        private void PaintBuilding(Graphics g)
        {
            g.DrawImage(Properties.Resources.Building1, 12, 12, 734, 472);
        }

        private void PaintElevators(Graphics g)
        {
            for (int i = 0; i < MyBuilding.arrayElevator.Length; i++)
            {
                Elevator ElevatorToPaint = MyBuilding.arrayElevator[i];
                g.DrawImage(ElevatorToPaint.GetCurrentFrame(), ElevatorToPaint.GetElevatorXPosition(), ElevatorToPaint.GetElevatorYPosition(), 54, 90);
            }
        }

        private void PaintPassengers(Graphics g)
        {
            List<Passenger> CopyOfListOfAllPeopleWhoNeedAnimation = new List<Passenger>(MyBuilding.listeople);

            foreach (Passenger PassengerToPaint in CopyOfListOfAllPeopleWhoNeedAnimation)
            {
                if ((PassengerToPaint != null) && (PassengerToPaint.GetPassengerVisibility()))
                {
                    g.DrawImage(PassengerToPaint.GetCurrentFrame(), PassengerToPaint.PassengerPosition.X, PassengerToPaint.PassengerPosition.Y + 15, 35, 75); 
                }
            }
        }


        #region EVENT HANDLERS

        private void timerRefresh_Tick(object sender, EventArgs e)=> this.Invalidate();


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            PaintBuilding(g);
            PaintElevators(g);
            PaintPassengers(g);
        }

        #endregion EVENT HANDLERS

    }
}
