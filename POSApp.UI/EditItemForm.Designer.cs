namespace POSApp.UI
{
    partial class EditItemForm
    {
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
            Label lblName = new Label
            {
                Text = "Name:",
                Font = new Font("Segoe UI", 10F),
                Location = new Point(20, 20),
                AutoSize = true
            };
            txtName = new TextBox
            {
                Location = new Point(120, 18),
                Size = new Size(200, 24)
            };

            Label lblCategory = new Label
            {
                Text = "Category:",
                Font = new Font("Segoe UI", 10F),
                Location = new Point(20, 60),
                AutoSize = true
            };
            txtCategory = new TextBox
            {
                Location = new Point(120, 58),
                Size = new Size(200, 24)
            };

            Label lblPrice = new Label
            {
                Text = "Price:",
                Font = new Font("Segoe UI", 10F),
                Location = new Point(20, 100),
                AutoSize = true
            };
            txtPrice = new TextBox
            {
                Location = new Point(120, 98),
                Size = new Size(200, 24)
            };

            btnSave = new Button
            {
                Text = "Save",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.MediumPurple,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 35),
                Location = new Point(120, 150)
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 35),
                Location = new Point(230, 150)
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => DialogResult = DialogResult.Cancel;

            Controls.AddRange(new Control[] { lblName, txtName, lblCategory, txtCategory, lblPrice, txtPrice, btnSave, btnCancel });
        }

        #endregion
    }
}