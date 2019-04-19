using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LiftSimulator.Mediators;
using LiftSimulator.ViewModels;

namespace LiftSimulator
{
    public partial class Form1 : Form
    {

        public Building MyBuilding;
        public BuildingViewModel Building;

        public Form1()
        {
            InitializeComponent();
            MyBuilding = new Building();
            Building = new BuildingViewModel(new BuildingMediator());
        }

        private void PaintBuilding(Graphics g)
        {
            g.DrawImage(Properties.Resources.Building1, 12, 12, 734, 472);
        }

        private void PaintElevators(Graphics g)
        {
            for (int i = 0; i < Building.Elevators.Count; i++)
            {
                var elevator = Building.Elevators[i];
                g.DrawImage(elevator.GetCurrentFrame(), elevator.ElevatorPosition.X, elevator.ElevatorPosition.Y, 54, 90);
            }


            //for (int i = 0; i < MyBuilding.ArrayOfAllElevators.Length; i++)
            //{
            //    Elevator ElevatorToPaint = MyBuilding.ArrayOfAllElevators[i];
            //    g.DrawImage(ElevatorToPaint.GetCurrentFrame(), ElevatorToPaint.GetElevatorXPosition(), ElevatorToPaint.GetElevatorYPosition(), 54, 90);
            //}
        }

        private void PaintPassengers(Graphics g)
        {
            var copyOfListOfAllPeopleWhoNeedAnimation = Building.ListOfAllPeopleWhoNeedAnimation;

            foreach (var passengerToPaint in copyOfListOfAllPeopleWhoNeedAnimation)
            {
                g.DrawImage(passengerToPaint.PersonGraphic, passengerToPaint.PersonPosition.X,
                    passengerToPaint.PersonPosition.Y + 15, 35, 75);
            }
        }



        private void timerRefresh_Tick(object sender, EventArgs e)=> this.Invalidate();


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            PaintBuilding(g);
            PaintElevators(g);
            PaintPassengers(g);
        }
    }
}
