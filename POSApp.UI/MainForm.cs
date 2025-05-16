using System;
using System.Linq;
using System.Windows.Forms;
using POSApp.Core.Interfaces;
using POSApp.Core.Models;
using System.Collections.Generic;

namespace POSApp.UI
{
    public partial class MainForm : Form
    {
        private readonly IAuthService _authService;
        private readonly IItemService _itemService;
        private readonly IOrderService _orderService;
        private readonly User _currentUser;

        private MenuStrip menuStrip;
        private ComboBox cmbCategories;
        private DataGridView dgvItems;
        private TextBox txtItemCode;
        private Button btnPay;
        private OrderForm orderForm;

        public MainForm(IAuthService authService, IItemService itemService, IOrderService orderService, User currentUser)
        {
            _authService = authService;
            _itemService = itemService;
            _orderService = orderService;
            _currentUser = currentUser;

            InitializeComponent();
            SetupMenu();
            SetupUI();
        }

        private void SetupMenu()
        {
            menuStrip = new MenuStrip();

            var itemMenu = new ToolStripMenuItem("Item");
            var categoryMenu = new ToolStripMenuItem("Category");
            var helpMenu = new ToolStripMenuItem("Help");

            if (_currentUser.Role == Role.Manager)
            {
                var editItem = new ToolStripMenuItem("Edit Item");
                var deleteItem = new ToolStripMenuItem("Delete Item");

                editItem.Click += EditItem_Click;
                deleteItem.Click += DeleteItem_Click;

                itemMenu.DropDownItems.Add(editItem);
                itemMenu.DropDownItems.Add(deleteItem);
                menuStrip.Items.Add(itemMenu);
                menuStrip.Items.Add(categoryMenu);
                menuStrip.Items.Add(helpMenu);
            }
            else
            {
                menuStrip.Items.Add(itemMenu);
                menuStrip.Items.Add(helpMenu);
                itemMenu.Enabled = false;
            }
            helpMenu.Click += (s, e) =>
            {
                MessageBox.Show("Need help? Call: 9999-8888", "Support");
            };


            this.MainMenuStrip = menuStrip;
            Controls.Add(menuStrip);
        }

        private void SetupUI()
        {
            // Category dropdown
            cmbCategories = new ComboBox
            {
                Left = 10,
                Top = menuStrip.Bottom + 10,
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCategories.SelectedIndexChanged += (s, e) => LoadItems();
            Controls.Add(cmbCategories);

            // Item grid
            dgvItems = new DataGridView
            {
                Name = "dgvItems",
                Left = 10,
                Top = cmbCategories.Bottom + 10,
                Width = 300,
                Height = 300,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = false
            };

            dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "Code", HeaderText = "Code", DataPropertyName = "Code" });
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Name", DataPropertyName = "Name" });
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "Category", HeaderText = "Category", DataPropertyName = "Category" });

            dgvItems.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    var code = dgvItems.Rows[e.RowIndex].Cells["Code"].Value.ToString();
                    _orderService.AddToOrder(code);
                    orderForm.RefreshOrderTable();
                }
            };
            Controls.Add(dgvItems);

            // Code textbox
            txtItemCode = new TextBox
            {
                Left = dgvItems.Right + 20,
                Top = dgvItems.Top,
                Width = 200
            };
            txtItemCode.KeyDown += txtItemCode_KeyDown;
            Controls.Add(txtItemCode);

            // Pay button
            btnPay = new Button
            {
                Text = "Pay",
                Top = txtItemCode.Bottom + 20,
                Left = txtItemCode.Left
            };
            btnPay.Click += btnPay_Click;
            Controls.Add(btnPay);

            // Order panel
            orderForm = new OrderForm(_orderService)
            {
                Top = dgvItems.Top,
                Left = txtItemCode.Right + 30
            };
            Controls.Add(orderForm);

            LoadCategories();
            LoadItems();
        }

        private void LoadCategories()
        {
            var all = _itemService.GetAllItems();
            var cats = all.Select(i => i.Category).Distinct().OrderBy(c => c).ToList();
            cats.Insert(0, "All");
            cmbCategories.DataSource = cats;
        }

        private void LoadItems()
        {
            var selectedCat = cmbCategories.SelectedItem?.ToString();
            var allItems = _itemService.GetAllItems();

            var filtered = string.IsNullOrWhiteSpace(selectedCat) || selectedCat == "All"
                ? allItems
                : allItems.Where(i => i.Category == selectedCat);

            dgvItems.DataSource = filtered.ToList();
        }

        private void EditItem_Click(object sender, EventArgs e)
        {
            string code = Prompt.ShowDialog("Enter item code to edit:", "Edit Item");
            var item = _itemService.GetItemByCode(code);
            if (item != null)
            {
                string newName = Prompt.ShowDialog("Enter new name:", "Edit Item");
                string newCategory = Prompt.ShowDialog("Enter category:", "Edit Item");
                string priceStr = Prompt.ShowDialog("Enter new price:", "Edit Item");

                if (decimal.TryParse(priceStr, out decimal newPrice))
                {
                    item.Name = newName;
                    item.Category = newCategory;
                    item.UnitPrice = newPrice;
                    _itemService.UpdateItem(item);
                    MessageBox.Show("Item updated.");
                    LoadCategories();
                    LoadItems();
                }
                else
                {
                    MessageBox.Show("Invalid price.");
                }
            }
            else
            {
                MessageBox.Show("Item not found.");
            }
        }

        private void DeleteItem_Click(object sender, EventArgs e)
        {
            string code = Prompt.ShowDialog("Enter item code to delete:", "Delete Item");
            var item = _itemService.GetItemByCode(code);
            if (item != null)
            {
                _itemService.DeleteItem(item.Id);
                MessageBox.Show("Item deleted.");
                LoadItems();
            }
            else MessageBox.Show("Item not found.");
        }

        private void txtItemCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && sender is TextBox txt && !string.IsNullOrWhiteSpace(txt.Text))
            {
                _orderService.AddToOrder(txt.Text.Trim());
                orderForm.RefreshOrderTable();
                txt.Clear();
            }
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            var total = _orderService.CalculateTotal();
            if (total == 0)
            {
                MessageBox.Show("Cart is empty.");
                return;
            }

            using (var payForm = new PayForm(total, _orderService))
            {
                if (payForm.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Receipt printed.\nThank you for your purchase!", "Receipt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _orderService.ClearOrder();
                    orderForm.RefreshOrderTable();
                }
            }
        }


    }

    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form
            {
                Width = 300,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };

            Label textLabel = new Label { Left = 20, Top = 20, Text = text, Width = 240 };
            TextBox textBox = new TextBox { Left = 20, Top = 50, Width = 240 };
            Button confirmation = new Button { Text = "OK", Left = 170, Width = 90, Top = 80, DialogResult = DialogResult.OK };

            confirmation.Click += (sender, e) => prompt.Close();

            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}
