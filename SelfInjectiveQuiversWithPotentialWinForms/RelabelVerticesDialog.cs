using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    public class RelabelVerticesDialog : CustomDialog
    {
        private Label lblOldVertices;
        private Label lblNewVertices;
        private TextBox txtOldVertices;
        private TextBox txtNewVertices;

        public string OldVerticesString => txtOldVertices.Text;
        public string NewVerticesString => txtNewVertices.Text;

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            txtOldVertices.Focus();
        }

        public RelabelVerticesDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.lblOldVertices = new System.Windows.Forms.Label();
            this.txtOldVertices = new System.Windows.Forms.TextBox();
            this.txtNewVertices = new System.Windows.Forms.TextBox();
            this.lblNewVertices = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(12, 58);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(106, 58);
            // 
            // lblOldVertices
            // 
            this.lblOldVertices.AutoSize = true;
            this.lblOldVertices.Location = new System.Drawing.Point(12, 9);
            this.lblOldVertices.Name = "lblOldVertices";
            this.lblOldVertices.Size = new System.Drawing.Size(63, 13);
            this.lblOldVertices.TabIndex = 2;
            this.lblOldVertices.Text = "Old vertices";
            // 
            // txtOldVertices
            // 
            this.txtOldVertices.Location = new System.Drawing.Point(87, 6);
            this.txtOldVertices.Name = "txtOldVertices";
            this.txtOldVertices.Size = new System.Drawing.Size(100, 20);
            this.txtOldVertices.TabIndex = 3;
            // 
            // txtNewVertices
            // 
            this.txtNewVertices.Location = new System.Drawing.Point(87, 32);
            this.txtNewVertices.Name = "txtNewVertices";
            this.txtNewVertices.Size = new System.Drawing.Size(100, 20);
            this.txtNewVertices.TabIndex = 5;
            // 
            // lblNewVertices
            // 
            this.lblNewVertices.AutoSize = true;
            this.lblNewVertices.Location = new System.Drawing.Point(12, 35);
            this.lblNewVertices.Name = "lblNewVertices";
            this.lblNewVertices.Size = new System.Drawing.Size(69, 13);
            this.lblNewVertices.TabIndex = 4;
            this.lblNewVertices.Text = "New vertices";
            // 
            // RelabelVerticesDialog
            // 
            this.ClientSize = new System.Drawing.Size(199, 93);
            this.Controls.Add(this.txtNewVertices);
            this.Controls.Add(this.lblNewVertices);
            this.Controls.Add(this.txtOldVertices);
            this.Controls.Add(this.lblOldVertices);
            this.Name = "RelabelVerticesDialog";
            this.Text = "Relabel vertices";
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.lblOldVertices, 0);
            this.Controls.SetChildIndex(this.txtOldVertices, 0);
            this.Controls.SetChildIndex(this.lblNewVertices, 0);
            this.Controls.SetChildIndex(this.txtNewVertices, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
