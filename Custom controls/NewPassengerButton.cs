using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LiftSimulator
{
    public partial class NewPassengerButton : Button
    {

        private int floorIndex;
        [Description("Floor which button is assigned to.")]
        public int FloorIndex
        {
            get { return floorIndex; }
            set { floorIndex = value; }
        }

        public Form1 MyForm
        {
            get { return (Form1)this.FindForm(); }
            private set { }
        }

        public Floor MyFloor
        {
            get { return (MyForm.MyBuilding.arrayFloor[this.floorIndex]); }
            private set { }
        }

        public NewPassengerButton() => InitializeComponent();
        private void NewPassengerButton_Click(object sender, EventArgs e)
        {
            if (sender is NewPassengerButton)
            {
                NewPassengerButton ThisPassengerButton = (NewPassengerButton)sender;

                if (MyFloor.GetCurrentAmmountOfPeopleInTheQueue() >= MyFloor.GetMaximumAmmountOfPeopleInTheQueue())
                {
                    MessageBox.Show("Too many people on the floor");
                    return;
                }
                FloorSelectionDialog dialog = new FloorSelectionDialog();
                for (int i = 0; i < MyForm.MyBuilding.arrayFloor.Length; i++) if (i != FloorIndex) dialog.ListOfFloorsInComboBox.Add(i);
                if (FloorIndex == 0) dialog.SelectedFloorIndex = 1;
                else dialog.SelectedFloorIndex = 0;


                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    Passenger NewPassenger = new Passenger(MyForm.MyBuilding, this.MyFloor, dialog.SelectedFloorIndex);
                    this.MyFloor.OnNewPassengerAppeared(new PassengerEventArgs(NewPassenger));
                }
            }
        }
    }
}