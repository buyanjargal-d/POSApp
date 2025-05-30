namespace POSApp.UI
{
    partial class LoginForm
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
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Login";
            this.Size = new System.Drawing.Size(700, 400);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            Panel leftPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 350,
                BackColor = Color.White
            };
            this.Controls.Add(leftPanel);

            Label lblWelcome = new Label
            {
                Text = "Welcome back",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Location = new Point(30, 40),
                AutoSize = true
            };
            leftPanel.Controls.Add(lblWelcome);

            txtUsername = new TextBox
            {
                PlaceholderText = "Username",
                Location = new Point(30, 100),
                Size = new Size(280, 30)
            };
            leftPanel.Controls.Add(txtUsername);

            txtPassword = new TextBox
            {
                PlaceholderText = "Password",
                UseSystemPasswordChar = true,
                Location = new Point(30, 150),
                Size = new Size(280, 30)
            };
            leftPanel.Controls.Add(txtPassword);

            btnLogin = new Button
            {
                Text = "Login",
                BackColor = Color.MediumPurple,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(30, 200),
                Size = new Size(280, 35)
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            leftPanel.Controls.Add(btnLogin);
            

            Panel rightPanel = new Panel
            {
                Dock = DockStyle.Right,
                Width = 350,
                BackColor = Color.MediumPurple
            };
            this.Controls.Add(rightPanel);

            PictureBox picture = new PictureBox
            {
                Image = Image.FromFile("images/illustration.png"),
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Fill

            };
            rightPanel.Controls.Add(picture);

            this.ResumeLayout(false);
        }



        private void btnLogin_Click(object sender, EventArgs e)
        {
            var usernameText = txtUsername.Text.Trim();
            var passwordText = txtPassword.Text;

            var user = _authService.Login(usernameText, passwordText);

            if (user != null)
            {
                LoggedInUser = user;
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Invalid credentials", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}