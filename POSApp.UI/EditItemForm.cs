using System;
using System.Drawing;
using System.Windows.Forms;
using POSApp.Core.Models;

namespace POSApp.UI
{
    public partial class EditItemForm : Form
    {
        private readonly Item _item;
        private TextBox txtName;
        private TextBox txtCategory;
        private TextBox txtPrice;
        private Button btnSave;
        private Button btnCancel;

        public string ItemName => txtName.Text;
        public string ItemCategory => txtCategory.Text;
        public decimal ItemPrice => decimal.TryParse(txtPrice.Text, out var p) ? p : 0;

        public EditItemForm(Item item)
        {
            _item = item;
            Text = "Edit Item";
            Size = new Size(350, 260);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            BackColor = Color.White;
            StartPosition = FormStartPosition.CenterParent;

            InitializeComponent();
            LoadValues();
        }


        private void LoadValues()
        {
            txtName.Text = _item.Name;
            txtCategory.Text = _item.Category;
            txtPrice.Text = _item.UnitPrice.ToString("0.00");
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // simple validation
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtCategory.Text) || !decimal.TryParse(txtPrice.Text, out _))
            {
                MessageBox.Show("Please enter valid values.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult = DialogResult.OK;
        }
    }
}