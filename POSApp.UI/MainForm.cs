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
        private string _selectedItemCode;

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

            var helpMenu = new ToolStripMenuItem("Help");

            helpMenu.Click += (s, e) =>
            {
                MessageBox.Show("Need help? Call: 9999-8888", "Support");
            };
            menuStrip.Items.Add(helpMenu);

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
                Width = 500,
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
            dgvItems.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    var hit = dgvItems.HitTest(e.X, e.Y);
                    if (hit.RowIndex >= 0)
                    {
                        dgvItems.ClearSelection();
                        dgvItems.Rows[hit.RowIndex].Selected = true;
                        _selectedItemCode = dgvItems.Rows[hit.RowIndex].Cells["Code"].Value.ToString();
                    }
                }
            };

            // ✅ Only assign context menu AFTER dgvItems is initialized
            if (_currentUser.Role == Role.Manager)
            {
                var contextMenu = new ContextMenuStrip();
                var editMenu = new ToolStripMenuItem("Edit");
                var deleteMenu = new ToolStripMenuItem("Delete");
                editMenu.Click += EditMenu_Click;
                deleteMenu.Click += DeleteMenu_Click;
                contextMenu.Items.AddRange(new[] { editMenu, deleteMenu });

                dgvItems.ContextMenuStrip = contextMenu;
            }

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

        private void EditMenu_Click(object sender, EventArgs e)
        {
            if (_selectedItemCode == null) return;
            var item = _itemService.GetItemByCode(_selectedItemCode);
            if (item == null) return;

            using (var form = new EditItemForm(item))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    item.Name = form.ItemName;
                    item.Category = form.ItemCategory;
                    item.UnitPrice = form.ItemPrice;
                    _itemService.UpdateItem(item);
                    LoadCategories();
                    LoadItems();
                }
            }
        }

        private void DeleteMenu_Click(object sender, EventArgs e)
        {
            if (_selectedItemCode == null) return;
            var item = _itemService.GetItemByCode(_selectedItemCode);
            if (item == null) return;

            var confirm = MessageBox.Show("Are you sure you want to delete this item?", "Confirm", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                _itemService.DeleteItem(item.Id);
                LoadCategories();
                LoadItems();
            }
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
