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
            Text = "Payment";
            Size = new Size(350, 280);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            BackColor = Color.White;
            StartPosition = FormStartPosition.CenterParent;

            // Title
            lblTitle = new Label
            {
                Text = "Complete Payment",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.MediumPurple,
                AutoSize = true,
                Location = new Point(20, 20)
            };

            // Amount Due
            lblAmount = new Label
            {
                Text = $"Amount Due: ${_total:0.00}",
                Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(20, 70)
            };

            // Cash input
            var lblCash = new Label
            {
                Text = "Cash Received:",
                Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(20, 110)
            };
            numCash = new NumericUpDown
            {
                DecimalPlaces = 2,
                Increment = 0.50M,
                Minimum = 0,
                Maximum = 10000,
                Size = new Size(120, 25),
                Location = new Point(160, 108)
            };
            numCash.ValueChanged += (s, e) => UpdateChange();

            // Change display
            lblChange = new Label
            {
                Text = "Change: $0.00",
                Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(20, 150)
            };

            // Confirm button
            btnConfirm = new Button
            {
                Text = "Pay Now",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                BackColor = Color.MediumPurple,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(280, 40),
                Location = new Point(20, 190)
            };
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.Click += BtnConfirm_Click;

            Controls.AddRange(new Control[] { lblTitle, lblAmount, lblCash, numCash, lblChange, btnConfirm });
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