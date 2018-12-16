using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    public class PredefinedGeneralizedCobwebDialog : CustomDialog
    {
        private Label lblNumVerticesInCenterPolygon;
        private Label lblNumLayers;
        private NumericUpDown nudNumLayers;
        private NumericUpDown nudNumVerticesInCenterPolygon;

        public int NumVerticesInCenterPolygon => (int)nudNumVerticesInCenterPolygon.Value;

        public int NumLayers => (int)nudNumLayers.Value;

        public PredefinedGeneralizedCobwebDialog()
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
            this.lblNumLayers = new System.Windows.Forms.Label();
            this.nudNumLayers = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumVerticesInCenterPolygon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumLayers)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(12, 111);
            this.btnOK.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(93, 111);
            this.btnCancel.TabIndex = 5;
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
            // lblNumLayers
            // 
            this.lblNumLayers.AutoSize = true;
            this.lblNumLayers.Location = new System.Drawing.Point(12, 60);
            this.lblNumLayers.Name = "lblNumLayers";
            this.lblNumLayers.Size = new System.Drawing.Size(86, 13);
            this.lblNumLayers.TabIndex = 2;
            this.lblNumLayers.Text = "Number of layers";
            // 
            // nudNumLayers
            // 
            this.nudNumLayers.Location = new System.Drawing.Point(12, 76);
            this.nudNumLayers.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudNumLayers.Name = "nudNumLayers";
            this.nudNumLayers.Size = new System.Drawing.Size(65, 20);
            this.nudNumLayers.TabIndex = 3;
            this.nudNumLayers.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // PredefinedGeneralizedCobwebDialog
            // 
            this.ClientSize = new System.Drawing.Size(204, 146);
            this.Controls.Add(this.nudNumLayers);
            this.Controls.Add(this.lblNumLayers);
            this.Controls.Add(this.nudNumVerticesInCenterPolygon);
            this.Controls.Add(this.lblNumVerticesInCenterPolygon);
            this.Name = "PredefinedGeneralizedCobwebDialog";
            this.Text = "Load generalized cobweb";
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.lblNumVerticesInCenterPolygon, 0);
            this.Controls.SetChildIndex(this.nudNumVerticesInCenterPolygon, 0);
            this.Controls.SetChildIndex(this.lblNumLayers, 0);
            this.Controls.SetChildIndex(this.nudNumLayers, 0);
            ((System.ComponentModel.ISupportInitialize)(this.nudNumVerticesInCenterPolygon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumLayers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
