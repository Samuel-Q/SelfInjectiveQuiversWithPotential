using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    public class PredefinedOddFlowerDialog : CustomDialog
    {
        private Label lblNumVerticesInCenterPolygon;
        private NumericUpDown nudNumVerticesInCenterPolygon;

        public int NumVerticesInCenterPolygon => (int)nudNumVerticesInCenterPolygon.Value;

        public PredefinedOddFlowerDialog()
        {
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            // Remark: The Text property is hidden by Intellisense because it has no "affect on the appearance of the NumericUpDown control"
            // Seems crazy for two reasons: Hiding anything seems fishy, and the property *does* seem to be useful to access!
            nudNumVerticesInCenterPolygon.Focus();
            nudNumVerticesInCenterPolygon.Select(0, nudNumVerticesInCenterPolygon.Text.Length);
        }

        private void InitializeComponent()
        {
            this.lblNumVerticesInCenterPolygon = new System.Windows.Forms.Label();
            this.nudNumVerticesInCenterPolygon = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumVerticesInCenterPolygon)).BeginInit();
            this.SuspendLayout();
            // 
            // lblNumVerticesInCenterPolygon
            // 
            this.lblNumVerticesInCenterPolygon.AutoSize = true;
            this.lblNumVerticesInCenterPolygon.Location = new System.Drawing.Point(12, 9);
            this.lblNumVerticesInCenterPolygon.Name = "lblNumVerticesInCenterPolygon";
            this.lblNumVerticesInCenterPolygon.Size = new System.Drawing.Size(180, 13);
            this.lblNumVerticesInCenterPolygon.TabIndex = 0;
            this.lblNumVerticesInCenterPolygon.Text = "Number of vertices in center polygon";
            // 
            // nudNumVerticesInCenterPolygon
            // 
            this.nudNumVerticesInCenterPolygon.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudNumVerticesInCenterPolygon.Location = new System.Drawing.Point(12, 25);
            this.nudNumVerticesInCenterPolygon.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nudNumVerticesInCenterPolygon.Name = "nudNumVerticesInCenterPolygon";
            this.nudNumVerticesInCenterPolygon.Size = new System.Drawing.Size(65, 20);
            this.nudNumVerticesInCenterPolygon.TabIndex = 1;
            this.nudNumVerticesInCenterPolygon.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // PredefinedOddFlowerDialog
            // 
            this.ClientSize = new System.Drawing.Size(204, 87);
            this.Controls.Add(this.nudNumVerticesInCenterPolygon);
            this.Controls.Add(this.lblNumVerticesInCenterPolygon);
            this.Name = "PredefinedOddFlowerDialog";
            this.Text = "Load odd flower";
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.lblNumVerticesInCenterPolygon, 0);
            this.Controls.SetChildIndex(this.nudNumVerticesInCenterPolygon, 0);
            ((System.ComponentModel.ISupportInitialize)(this.nudNumVerticesInCenterPolygon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
