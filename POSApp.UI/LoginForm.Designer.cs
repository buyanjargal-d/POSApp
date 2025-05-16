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
            var txtUsername = new TextBox { Name = "txtUsername", Top = 20, Left = 20, Width = 200 };
            var txtPassword = new TextBox { Name = "txtPassword", Top = 60, Left = 20, Width = 200, UseSystemPasswordChar = true };
            var btnLogin = new Button { Name = "btnLogin", Text = "Login", Top = 100, Left = 20 };
            btnLogin.Click += btnLogin_Click;

            Controls.Add(txtUsername);
            Controls.Add(txtPassword);
            Controls.Add(btnLogin);
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