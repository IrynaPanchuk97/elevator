using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace LiftSimulator
{
    public partial class FloorSelectionDialog : Form
    {

        public ComboBox.ObjectCollection ListOfFloorsInComboBox{
            get
            {
                return this.floorsComboBox.Items;
            }

            set
            {
                this.floorsComboBox.Items.Clear();
                foreach (object floor in value)
                {
                    floorsComboBox.Items.Add(floor);
                }
            }
        }

        public int SelectedFloorIndex
        {
            get
            {
                return (int)this.floorsComboBox.SelectedItem;
            }

            set
            {
                this.floorsComboBox.SelectedItem = value;
            }
        }

        public FloorSelectionDialog()=>   InitializeComponent();
        private void floorsComboBox_SelectedIndexChanged(object sender, EventArgs e)=> buttonOK.Select();
        private void ButtonOK_Click(object sender, EventArgs e) { }
    }
}
