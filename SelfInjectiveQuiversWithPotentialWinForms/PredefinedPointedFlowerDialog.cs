using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    class PredefinedPointedFlowerDialog : CustomDialog
    {
        private NumericUpDown nudNumPeriods;
        private Label lblNumPeriods;

        public int NumPeriods => (int)nudNumPeriods.Value;

        public PredefinedPointedFlowerDialog()
        {
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            // Remark: The Text property is hidden by Intellisense because it has no "affect on the appearance of the NumericUpDown control"
            // Seems crazy for two reasons: Hiding anything seems fishy, and the property *does* seem to be useful to access!
            nudNumPeriods.Focus();
            nudNumPeriods.Select(0, nudNumPeriods.Text.Length);
        }

        private void InitializeComponent()
        {
            this.nudNumPeriods = new System.Windows.Forms.NumericUpDown();
            this.lblNumPeriods = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumPeriods)).BeginInit();
            this.SuspendLayout();
            // 
            // nudNumPeriods
            // 
            this.nudNumPeriods.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudNumPeriods.Location = new System.Drawing.Point(12, 25);
            this.nudNumPeriods.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nudNumPeriods.Name = "nudNumPeriods";
            this.nudNumPeriods.Size = new System.Drawing.Size(65, 20);
            this.nudNumPeriods.TabIndex = 3;
            this.nudNumPeriods.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // lblNumPeriods
            // 
            this.lblNumPeriods.AutoSize = true;
            this.lblNumPeriods.Location = new System.Drawing.Point(12, 9);
            this.lblNumPeriods.Name = "lblNumPeriods";
            this.lblNumPeriods.Size = new System.Drawing.Size(93, 13);
            this.lblNumPeriods.TabIndex = 2;
            this.lblNumPeriods.Text = "Number of periods";
            // 
            // PredefinedPointedFlowerDialog
            // 
            this.ClientSize = new System.Drawing.Size(180, 87);
            this.Controls.Add(this.nudNumPeriods);
            this.Controls.Add(this.lblNumPeriods);
            this.Name = "PredefinedPointedFlowerDialog";
            this.Text = "Load pointed flower";
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.lblNumPeriods, 0);
            this.Controls.SetChildIndex(this.nudNumPeriods, 0);
            ((System.ComponentModel.ISupportInitialize)(this.nudNumPeriods)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
