using System;
using System.Drawing;
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
        }
    }
}
