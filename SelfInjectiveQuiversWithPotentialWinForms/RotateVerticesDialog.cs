using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    public class RotateVerticesDialog : CustomDialog
    {
        private System.Windows.Forms.TextBox txtVertices;
        private System.Windows.Forms.Label lblDegrees;
        private System.Windows.Forms.NumericUpDown nudDegrees;
        private System.Windows.Forms.NumericUpDown nudCenterX;
        private System.Windows.Forms.Label lblCenter;
        private System.Windows.Forms.NumericUpDown nudCenterY;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.Label lblY;
        private System.Windows.Forms.Label lblVertices;

        public string VerticesString => txtVertices.Text;
        public int CenterX => (int)nudCenterX.Value;
        public int CenterY => (int)nudCenterY.Value;
        public double Degrees => (double)nudDegrees.Value;

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            txtVertices.Focus();
        }

        public RotateVerticesDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.lblVertices = new System.Windows.Forms.Label();
            this.txtVertices = new System.Windows.Forms.TextBox();
            this.lblDegrees = new System.Windows.Forms.Label();
            this.nudDegrees = new System.Windows.Forms.NumericUpDown();
            this.nudCenterX = new System.Windows.Forms.NumericUpDown();
            this.lblCenter = new System.Windows.Forms.Label();
            this.nudCenterY = new System.Windows.Forms.NumericUpDown();
            this.lblX = new System.Windows.Forms.Label();
            this.lblY = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudDegrees)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCenterX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCenterY)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(12, 95);
            this.btnOK.TabIndex = 9;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(125, 95);
            this.btnCancel.TabIndex = 10;
            // 
            // lblVertices
            // 
            this.lblVertices.AutoSize = true;
            this.lblVertices.Location = new System.Drawing.Point(12, 9);
            this.lblVertices.Name = "lblVertices";
            this.lblVertices.Size = new System.Drawing.Size(45, 13);
            this.lblVertices.TabIndex = 0;
            this.lblVertices.Text = "Vertices";
            // 
            // txtVertices
            // 
            this.txtVertices.Location = new System.Drawing.Point(80, 6);
            this.txtVertices.Name = "txtVertices";
            this.txtVertices.Size = new System.Drawing.Size(120, 20);
            this.txtVertices.TabIndex = 1;
            // 
            // lblDegrees
            // 
            this.lblDegrees.AutoSize = true;
            this.lblDegrees.Location = new System.Drawing.Point(12, 60);
            this.lblDegrees.Name = "lblDegrees";
            this.lblDegrees.Size = new System.Drawing.Size(47, 13);
            this.lblDegrees.TabIndex = 7;
            this.lblDegrees.Text = "Degrees";
            // 
            // nudDegrees
            // 
            this.nudDegrees.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudDegrees.Location = new System.Drawing.Point(80, 58);
            this.nudDegrees.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.nudDegrees.Minimum = new decimal(new int[] {
            9999999,
            0,
            0,
            -2147483648});
            this.nudDegrees.Name = "nudDegrees";
            this.nudDegrees.Size = new System.Drawing.Size(120, 20);
            this.nudDegrees.TabIndex = 8;
            // 
            // nudCenterX
            // 
            this.nudCenterX.Location = new System.Drawing.Point(80, 32);
            this.nudCenterX.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudCenterX.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.nudCenterX.Name = "nudCenterX";
            this.nudCenterX.Size = new System.Drawing.Size(48, 20);
            this.nudCenterX.TabIndex = 4;
            // 
            // lblCenter
            // 
            this.lblCenter.AutoSize = true;
            this.lblCenter.Location = new System.Drawing.Point(12, 34);
            this.lblCenter.Name = "lblCenter";
            this.lblCenter.Size = new System.Drawing.Size(38, 13);
            this.lblCenter.TabIndex = 2;
            this.lblCenter.Text = "Center";
            // 
            // nudCenterY
            // 
            this.nudCenterY.Location = new System.Drawing.Point(152, 32);
            this.nudCenterY.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudCenterY.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.nudCenterY.Name = "nudCenterY";
            this.nudCenterY.Size = new System.Drawing.Size(48, 20);
            this.nudCenterY.TabIndex = 6;
            // 
            // lblX
            // 
            this.lblX.AutoSize = true;
            this.lblX.Location = new System.Drawing.Point(62, 34);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(12, 13);
            this.lblX.TabIndex = 3;
            this.lblX.Text = "x";
            // 
            // lblY
            // 
            this.lblY.AutoSize = true;
            this.lblY.Location = new System.Drawing.Point(134, 34);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(12, 13);
            this.lblY.TabIndex = 5;
            this.lblY.Text = "y";
            // 
            // RotateVerticesDialog
            // 
            this.ClientSize = new System.Drawing.Size(212, 130);
            this.Controls.Add(this.lblY);
            this.Controls.Add(this.lblX);
            this.Controls.Add(this.nudCenterY);
            this.Controls.Add(this.nudCenterX);
            this.Controls.Add(this.lblCenter);
            this.Controls.Add(this.nudDegrees);
            this.Controls.Add(this.lblDegrees);
            this.Controls.Add(this.txtVertices);
            this.Controls.Add(this.lblVertices);
            this.Name = "RotateVerticesDialog";
            this.Text = "Rotate vertices";
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.lblVertices, 0);
            this.Controls.SetChildIndex(this.txtVertices, 0);
            this.Controls.SetChildIndex(this.lblDegrees, 0);
            this.Controls.SetChildIndex(this.nudDegrees, 0);
            this.Controls.SetChildIndex(this.lblCenter, 0);
            this.Controls.SetChildIndex(this.nudCenterX, 0);
            this.Controls.SetChildIndex(this.nudCenterY, 0);
            this.Controls.SetChildIndex(this.lblX, 0);
            this.Controls.SetChildIndex(this.lblY, 0);
            ((System.ComponentModel.ISupportInitialize)(this.nudDegrees)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCenterX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCenterY)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
