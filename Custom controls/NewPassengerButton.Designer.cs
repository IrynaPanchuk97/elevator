namespace LiftSimulator
{
    partial class NewPassengerButton
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Size = new System.Drawing.Size(75, 60);
            this.Text = "New passenger";
            this.Click += new System.EventHandler(this.NewPassengerButton_Click);
            this.ResumeLayout(false);

        }
    }
}
