using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    public class PredefinedCycleDialog : CustomDialog
    {
        private Label lblCycleLength;
        private NumericUpDown nudCycleLength;

        public int CycleLength => (int)nudCycleLength.Value;

        public PredefinedCycleDialog()
        {
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            // Remark: The Text property is hidden by Intellisense because it has no "affect on the appearance of the NumericUpDown control"
            // Seems crazy for two reasons: Hiding anything seems fishy, and the property *does* seem to be useful to access!
            nudCycleLength.Focus();
            nudCycleLength.Select(0, nudCycleLength.Text.Length);
        }

        private void InitializeComponent()
        {
            this.nudCycleLength = new System.Windows.Forms.NumericUpDown();
            this.lblCycleLength = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudCycleLength)).BeginInit();
            this.SuspendLayout();
            // 
            // nudCycleLength
            // 
            this.nudCycleLength.Location = new System.Drawing.Point(12, 25);
            this.nudCycleLength.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nudCycleLength.Name = "nudCycleLength";
            this.nudCycleLength.Size = new System.Drawing.Size(65, 20);
            this.nudCycleLength.TabIndex = 1;
            this.nudCycleLength.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // lblCycleLength
            // 
            this.lblCycleLength.AutoSize = true;
            this.lblCycleLength.Location = new System.Drawing.Point(12, 9);
            this.lblCycleLength.Name = "lblCycleLength";
            this.lblCycleLength.Size = new System.Drawing.Size(65, 13);
            this.lblCycleLength.TabIndex = 0;
            this.lblCycleLength.Text = "Cycle length";
            // 
            // PredefinedCycleDialog
            // 
            this.ClientSize = new System.Drawing.Size(180, 87);
            this.Controls.Add(this.lblCycleLength);
            this.Controls.Add(this.nudCycleLength);
            this.Name = "PredefinedCycleDialog";
            this.Text = "Load cycle";
            this.Controls.SetChildIndex(this.nudCycleLength, 0);
            this.Controls.SetChildIndex(this.lblCycleLength, 0);
            ((System.ComponentModel.ISupportInitialize)(this.nudCycleLength)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
