namespace POSApp.UI
{
    partial class PayForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.lblTotalAmount = new Label
            {
                Name = "lblTotalAmount",
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(200, 22),
                Text = $"Total: ${_total:0.00}"
            };
            this.Controls.Add(this.lblTotalAmount);
            this.txtCash = new TextBox
            {
                Name = "txtCash",
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(200, 22),
                TabIndex = 0
            };
            this.Controls.Add(this.txtCash);
            this.btnConfirm = new Button
            {
                Name = "btnConfirm",
                Text = "Confirm",
                Location = new System.Drawing.Point(20, 100),
                Size = new System.Drawing.Size(75, 25),
                TabIndex = 1
            };
            this.btnConfirm.Click += (s, e) => btnConfirm_Click(this.txtCash);
            this.Controls.Add(this.btnConfirm);
            this.ClientSize = new System.Drawing.Size(250, 150);
            this.Name = "PayForm";
            this.Text = "Payment";

            this.ResumeLayout(false);
            this.PerformLayout();
        }


        private void btnConfirm_Click(TextBox txtCash)
        {
            if (decimal.TryParse(txtCash.Text, out decimal cashGiven))
            {
                if (cashGiven >= _total)
                {
                    var change = cashGiven - _total;
                    MessageBox.Show($"Change: ${change:0.00}", "Payment Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Insufficient cash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Invalid amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}