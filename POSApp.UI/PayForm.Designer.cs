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
            var lblTotalAmount = new Label { Name = "lblTotalAmount", Top = 20, Left = 20, Width = 200, Text = $"Total: ${_total:0.00}" };
            var txtCash = new TextBox { Name = "txtCash", Top = 60, Left = 20, Width = 200 };
            var btnConfirm = new Button { Name = "btnConfirm", Text = "Confirm", Top = 100, Left = 20 };
            btnConfirm.Click += (s, e) => btnConfirm_Click(txtCash);

            Controls.Add(lblTotalAmount);
            Controls.Add(txtCash);
            Controls.Add(btnConfirm);
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