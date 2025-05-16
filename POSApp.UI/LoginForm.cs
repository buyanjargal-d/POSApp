using POSApp.Core.Interfaces;
using System;
using System.Windows.Forms;
using POSApp.Core.Models;
using System.ComponentModel;

namespace POSApp.UI
{
    public partial class LoginForm : Form
    {
        private readonly IAuthService _authService;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public User LoggedInUser { get; private set; }

        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;

        public LoginForm(IAuthService authService)
        {
            _authService = authService;
            InitializeComponent();
        }
    }
}
