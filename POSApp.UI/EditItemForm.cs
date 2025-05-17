using System;
using System.Drawing;
using System.Windows.Forms;
using POSApp.Core.Models;


namespace POSApp.UI
{
    public partial class EditItemForm : Form
    {
        private TextBox txtName;
        private TextBox txtCategory;
        private TextBox txtPrice;

        public EditItemForm(Item item)
        {
            Text = "Edit Item";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(340, 220);
            Font = new Font("Segoe UI", 9f);

            var layout = new TableLayoutPanel
            {
                RowCount = 4,
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                AutoSize = true
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80f));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30f));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30f));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30f));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            layout.Controls.Add(new Label
            {
                Text = "Name:",
                Anchor = AnchorStyles.Right,
                AutoSize = true
            }, 0, 0);
            txtName = new TextBox
            {
                Text = item.Name,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            layout.Controls.Add(txtName, 1, 0);
            layout.Controls.Add(new Label
            {
                Text = "Category:",
                Anchor = AnchorStyles.Right,
                AutoSize = true
            }, 0, 1);
            txtCategory = new TextBox
            {
                Text = item.Category,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            layout.Controls.Add(txtCategory, 1, 1);
            layout.Controls.Add(new Label
            {
                Text = "Price:",
                Anchor = AnchorStyles.Right,
                AutoSize = true
            }, 0, 2);
            txtPrice = new TextBox
            {
                Text = item.UnitPrice.ToString("0.00"),
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            layout.Controls.Add(txtPrice, 1, 2);
            var btnPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 10, 0, 0),
                AutoSize = true
            };
            var btnOK = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                AutoSize = true,
                Margin = new Padding(3)
            };
            var btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                AutoSize = true,
                Margin = new Padding(3)
            };
            btnPanel.Controls.AddRange(new Control[] { btnOK, btnCancel });

            layout.Controls.Add(btnPanel, 0, 3);
            layout.SetColumnSpan(btnPanel, 2);
            AcceptButton = btnOK;
            CancelButton = btnCancel;

            Controls.Add(layout);
        }
        public string ItemName => txtName.Text.Trim();
        public string ItemCategory => txtCategory.Text.Trim();
        public decimal ItemPrice => decimal.TryParse(txtPrice.Text, out var p) ? p : 0m;
    }
}
