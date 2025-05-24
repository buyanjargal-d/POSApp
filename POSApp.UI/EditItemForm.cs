using System;
using System.Windows.Forms;
using POSApp.Core.Models;

namespace POSApp.UI
{
    public partial class EditItemForm : Form
    {
        public string ItemName => txtName.Text;
        public string ItemCategory => txtCategory.Text;
        public decimal ItemPrice => decimal.TryParse(txtPrice.Text, out var p) ? p : 0;

        private TextBox txtName;
        private TextBox txtCategory;
        private TextBox txtPrice;

        public EditItemForm(Item item)
        {
            this.Text = "Edit Item";
            this.Width = 320;
            this.Height = 240;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblName = new Label { Text = "Name", Left = 20, Top = 20, Width = 80 };
            txtName = new TextBox { Text = item.Name, Left = 110, Top = 20, Width = 170 };

            Label lblCategory = new Label { Text = "Category", Left = 20, Top = 60, Width = 80 };
            txtCategory = new TextBox { Text = item.Category, Left = 110, Top = 60, Width = 170 };

            Label lblPrice = new Label { Text = "Price", Left = 20, Top = 100, Width = 80 };
            txtPrice = new TextBox { Text = item.UnitPrice.ToString("0.00"), Left = 110, Top = 100, Width = 170 };

            Button btnOK = new Button { Text = "OK", Left = 110, Width = 90, Top = 140, DialogResult = DialogResult.OK };
            btnOK.Click += (s, e) => this.Close();

            Controls.AddRange(new Control[] { lblName, txtName, lblCategory, txtCategory, lblPrice, txtPrice, btnOK });
            this.AcceptButton = btnOK;
        }
    }
}
