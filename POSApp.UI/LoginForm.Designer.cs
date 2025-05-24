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
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.txtUsername = new System.Windows.Forms.TextBox
            {
                Name = "txtUsername",
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(200, 22),
                TabIndex = 0
            };
            this.Controls.Add(this.txtUsername);
            this.txtPassword = new System.Windows.Forms.TextBox
            {
                Name = "txtPassword",
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(200, 22),
                UseSystemPasswordChar = true,
                TabIndex = 1
            };
            this.Controls.Add(this.txtPassword);
            this.btnLogin = new System.Windows.Forms.Button
            {
                Name = "btnLogin",
                Text = "Login",
                Location = new System.Drawing.Point(20, 100),
                Size = new System.Drawing.Size(75, 25),
                TabIndex = 2,
            };
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            this.Controls.Add(this.btnLogin);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var username = Controls["txtUsername"] as TextBox;
            var password = Controls["txtPassword"] as TextBox;

            var user = _authService.Login(username.Text.Trim(), password.Text);

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