using Microsoft.EntityFrameworkCore;
using POSApp.Core.Interfaces;
using POSApp.Core.Models;
using POSApp.Core.Services;
using POSApp.Data;
using POSApp.UI;
using System;
using System.Windows.Forms;

static class Program
{
    [STAThread]
    static void Main()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseSqlite("Data Source=pos.db")
            .Options;

        var dbContext = new DatabaseContext(options);
        dbContext.Database.Migrate();

        IItemService itemService = new ItemService(dbContext);
        IOrderService orderService = new OrderService(itemService);
        IAuthService authService = new AuthService(); 

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        using (var loginForm = new LoginForm(authService))
        {
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                var user = loginForm.LoggedInUser;
                Application.Run(new MainForm(authService, itemService, orderService, user));
            }
        }
    }
}
