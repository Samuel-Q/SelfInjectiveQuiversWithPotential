using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    public class PredefinedTriangleDialog : CustomDialog
    {
        private Label lblNumRows;
        private NumericUpDown nudNumRows;

        public int NumRows => (int)nudNumRows.Value;

        public PredefinedTriangleDialog()
        {
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            // Remark: The Text property is hidden by Intellisense because it has no "affect on the appearance of the NumericUpDown control"
            // Seems crazy for two reasons: Hiding anything seems fishy, and the property *does* seem to be useful to access!
            nudNumRows.Focus();
            nudNumRows.Select(0, nudNumRows.Text.Length);
        }

        private void InitializeComponent()
        {
            this.lblNumRows = new System.Windows.Forms.Label();
            this.nudNumRows = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumRows)).BeginInit();
            this.SuspendLayout();
            // 
            // lblNumRows
            // 
            this.lblNumRows.AutoSize = true;
            this.lblNumRows.Location = new System.Drawing.Point(12, 9);
            this.lblNumRows.Name = "lblNumRows";
            this.lblNumRows.Size = new System.Drawing.Size(81, 13);
            this.lblNumRows.TabIndex = 0;
            this.lblNumRows.Text = "Number of rows";
            // 
            // nudNumRows
            // 
            this.nudNumRows.Location = new System.Drawing.Point(15, 25);
            this.nudNumRows.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudNumRows.Name = "nudNumRows";
            this.nudNumRows.Size = new System.Drawing.Size(65, 20);
            this.nudNumRows.TabIndex = 1;
            this.nudNumRows.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // PredefinedTriangleDialog
            // 
            this.ClientSize = new System.Drawing.Size(180, 87);
            this.Controls.Add(this.nudNumRows);
            this.Controls.Add(this.lblNumRows);
            this.Name = "PredefinedTriangleDialog";
            this.Text = "Load triangle";
            this.Controls.SetChildIndex(this.lblNumRows, 0);
            this.Controls.SetChildIndex(this.nudNumRows, 0);
            ((System.ComponentModel.ISupportInitialize)(this.nudNumRows)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
